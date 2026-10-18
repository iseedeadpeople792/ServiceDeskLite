
using System.ComponentModel.DataAnnotations;

namespace ServiceDeskLite.Models;

public class Ticket
{
    public int Id { get; set; }

    
    public string Subject { get; set; } = string.Empty;

    [Required(ErrorMessage = "Введите тему заявки.")]
    [StringLength(100, ErrorMessage = "Subject cannot exceed 100 characters.")]
    public string Description { get; set; } = string.Empty;

    [Required(ErrorMessage = "Введите описание заявки.")]
    [StringLength(100, ErrorMessage = "Описание не должно превышать 100 символов.")]
    public string RequesterName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Введите имя заявителя.")]
    [StringLength(80, ErrorMessage = "Имя не должно превышать 80 символов.")]
    public string Category { get; set; } = "Software";

    [Required]
    public string Priority { get; set; } = "Medium";

    public string Status { get; set; } = "Open";

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public List<TicketComment> Comments { get; set; } = new();
}
