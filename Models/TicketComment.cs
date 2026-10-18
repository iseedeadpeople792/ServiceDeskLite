
using System.ComponentModel.DataAnnotations;

namespace ServiceDeskLite.Models;

public class TicketComment
{
    public int Id { get; set; }

    public int TicketId { get; set; }

    [Required]
    [StringLength(80)]
    public string Author { get; set; } = string.Empty;

    [Required]
    [StringLength(2000)]
    public string Message { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public Ticket? Ticket { get; set; }
}
