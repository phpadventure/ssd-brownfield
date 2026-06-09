using System;
namespace ContosoDashboard.Models;

public class DocumentActivity
{
    public int ActivityId { get; set; }
    public int DocumentId { get; set; }
    public string Action { get; set; } = string.Empty; // upload/download/delete/share
    public int ActorUserId { get; set; }
    public DateTime TimestampUtc { get; set; }
    public string? Details { get; set; }
}
