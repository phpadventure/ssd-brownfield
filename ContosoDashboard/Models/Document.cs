using System;
namespace ContosoDashboard.Models;

public class Document
{
    public int DocumentId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string Category { get; set; } = string.Empty;
    public string? Tags { get; set; }
    public int? AssociatedProjectId { get; set; }
    public int UploadedByUserId { get; set; }
    public DateTime UploadDateUtc { get; set; }
    public long FileSizeBytes { get; set; }
    public string FileType { get; set; } = string.Empty;
    public string StoragePath { get; set; } = string.Empty;
}
