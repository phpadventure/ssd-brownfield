using System;
namespace ContosoDashboard.Models;

public class DocumentShare
{
    public int ShareId { get; set; }
    public int DocumentId { get; set; }
    public int? SharedWithUserId { get; set; }
    public int? SharedWithTeamId { get; set; }
    public int SharedByUserId { get; set; }
    public DateTime SharedAtUtc { get; set; }
}
