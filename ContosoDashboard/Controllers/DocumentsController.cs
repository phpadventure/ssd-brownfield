using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using ContosoDashboard.Services;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace ContosoDashboard.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class DocumentsController : ControllerBase
{
    private readonly IDocumentService _documentService;

    public DocumentsController(IDocumentService documentService)
    {
        _documentService = documentService;
    }

    [HttpPost("upload")]
    public async Task<IActionResult> Upload([FromForm] IFormFile file, [FromForm] string title, [FromForm] string category, [FromForm] int? associatedProjectId)
    {
        if (file == null || file.Length == 0) return BadRequest("No file");
        // For training, infer uploader from claims nameidentifier if present, else 0
        var userIdClaim = User.FindFirst("NameIdentifier")?.Value;
        int uploaderId = 0;
        if (!string.IsNullOrEmpty(userIdClaim) && int.TryParse(userIdClaim, out var parsed)) uploaderId = parsed;

        var doc = await _documentService.SaveAsync(file, title, category, uploaderId, associatedProjectId);
        return CreatedAtAction(nameof(Get), new { id = doc.DocumentId }, new { documentId = doc.DocumentId });
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        var doc = await _documentService.GetAsync(id);
        if (doc == null) return NotFound();
        return Ok(doc);
    }

    [HttpGet("{id}/download")]
    public async Task<IActionResult> Download(int id)
    {
        var doc = await _documentService.GetAsync(id);
        if (doc == null) return NotFound();

        var stream = await _documentService.DownloadAsync(doc.StoragePath);
        return File(stream, doc.FileType, doc.Title + System.IO.Path.GetExtension(doc.StoragePath));
    }

    [HttpGet]
    public async Task<IActionResult> List([FromQuery] string? q, [FromQuery] int page = 1, [FromQuery] int pageSize = 50)
    {
        // Parse optional filters
        var projectId = int.TryParse(Request.Query["projectId"], out var pid) ? pid : (int?)null;
        var uploadedBy = int.TryParse(Request.Query["uploadedBy"], out var uid) ? uid : (int?)null;

        // Resolve requesting user id from claims
        var userIdClaim = User.FindFirst("NameIdentifier")?.Value;
        int requestingUserId = 0;
        if (!string.IsNullOrEmpty(userIdClaim) && int.TryParse(userIdClaim, out var parsed)) requestingUserId = parsed;

        var (total, items) = await _documentService.SearchAsync(q, projectId, uploadedBy, page, pageSize, requestingUserId);
        return Ok(new { total, items });
    }
}
