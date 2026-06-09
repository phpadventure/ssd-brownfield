using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using ContosoDashboard.Data;
using ContosoDashboard.Models;
using ContosoDashboard.Services.Storage;

namespace ContosoDashboard.Services;

public interface IDocumentService
{
    Task<Document> SaveAsync(IFormFile file, string title, string category, int uploadedByUserId, int? projectId);
    Task<Document?> GetAsync(int id);
    Task<Stream> DownloadAsync(string storagePath);
}

public class DocumentService : IDocumentService
{
    private readonly ApplicationDbContext _db;
    private readonly IFileStorageService _storage;
    private readonly IFileScanner _scanner;

    public DocumentService(ApplicationDbContext db, IFileStorageService storage, IFileScanner scanner)
    {
        _db = db;
        _storage = storage;
        _scanner = scanner;
    }

    public async Task<Document> SaveAsync(IFormFile file, string title, string category, int uploadedByUserId, int? projectId)
    {
        using var ms = new MemoryStream();
        await file.CopyToAsync(ms);
        ms.Position = 0;
        var clean = await _scanner.IsCleanAsync(ms);
        if (!clean) throw new InvalidOperationException("File failed security scan");

        ms.Position = 0;
        var storagePath = await _storage.UploadAsync(ms, file.FileName, file.ContentType);

        var doc = new Document
        {
            Title = title,
            Category = category,
            Description = null,
            Tags = null,
            AssociatedProjectId = projectId,
            UploadedByUserId = uploadedByUserId,
            UploadDateUtc = DateTime.UtcNow,
            FileSizeBytes = file.Length,
            FileType = file.ContentType,
            StoragePath = storagePath
        };

        _db.Documents.Add(doc);
        await _db.SaveChangesAsync();
        return doc;
    }

    public Task<Document?> GetAsync(int id)
    {
        return _db.Documents.FirstOrDefaultAsync(d => d.DocumentId == id);
    }

    public Task<Stream> DownloadAsync(string storagePath)
    {
        return _storage.DownloadAsync(storagePath);
    }

    public async Task<(int total, Document[] items)> SearchAsync(string? q, int? projectId, int? uploadedBy, int page, int pageSize, int requestingUserId)
    {
        var query = _db.Documents.AsQueryable();

        if (!string.IsNullOrWhiteSpace(q))
        {
            query = query.Where(d => d.Title.Contains(q) || (d.Description != null && d.Description.Contains(q)) || (d.Tags != null && d.Tags.Contains(q)));
        }

        if (projectId.HasValue)
        {
            query = query.Where(d => d.AssociatedProjectId == projectId.Value);
        }

        if (uploadedBy.HasValue)
        {
            query = query.Where(d => d.UploadedByUserId == uploadedBy.Value);
        }

        // Access control: owner OR project member OR shared OR admin
        // Determine projects the requesting user is a member of
        var memberProjectIds = await _db.ProjectMembers
            .Where(pm => pm.UserId == requestingUserId)
            .Select(pm => pm.ProjectId)
            .ToListAsync();

        // Documents accessible: uploaded by user OR associated with a project the user is a member of OR shared with user
        var accessibleQuery = query.Where(d => d.UploadedByUserId == requestingUserId
            || (d.AssociatedProjectId != null && memberProjectIds.Contains(d.AssociatedProjectId.Value))
            || _db.DocumentShares.Any(ds => ds.DocumentId == d.DocumentId && ds.SharedWithUserId == requestingUserId)
        );

        var total = await accessibleQuery.CountAsync();
        var items = await accessibleQuery
            .OrderByDescending(d => d.UploadDateUtc)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToArrayAsync();

        return (total, items);
    }
}
