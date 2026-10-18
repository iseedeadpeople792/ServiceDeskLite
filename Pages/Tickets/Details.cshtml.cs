
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ServiceDeskLite.Models;
using ServiceDeskLite.Services;

namespace ServiceDeskLite.Pages.Tickets;

public class DetailsModel : PageModel
{
    private readonly TicketStore _ticketStore;

    public DetailsModel(TicketStore ticketStore)
    {
        _ticketStore = ticketStore;
    }

    public Ticket? Ticket { get; set; }

    public void OnGet(int id)
    {
        Ticket = _ticketStore.GetById(id);
        LoadComments(id);
    }

    public IActionResult OnPostUpdateStatus(int id, string status)
    {
        bool updated = _ticketStore.UpdateStatus(id, status);

        if (!updated)
        {
            return BadRequest("Invalid ticket ID or status.");
        }

        return RedirectToPage(new { id });
    }

    
[BindProperty]
public string CommentAuthor { get; set; } = string.Empty;

[BindProperty]
public string CommentMessage { get; set; } = string.Empty;

public List<TicketComment> Comments { get; set; } = new();

public void LoadComments(int id)
{
    Comments = _ticketStore.GetComments(id);
}

public IActionResult OnPostAddComment(int id)
{
    Console.WriteLine($"AddComment called. Ticket ID: {id}");
    Console.WriteLine($"Author: {CommentAuthor}");
    Console.WriteLine($"Message: {CommentMessage}");

    if (string.IsNullOrWhiteSpace(CommentAuthor) ||
        string.IsNullOrWhiteSpace(CommentMessage))
    {
        ModelState.AddModelError(string.Empty, "Укажите имя и текст комментария.");
        return ReloadPage(id);
    }

    if (CommentAuthor.Length > 80 || CommentMessage.Length > 2000)
    {
        ModelState.AddModelError(string.Empty, "Имя должно быть не длиннее 80 символов, а комментарий — 2000.");
        return ReloadPage(id);
    }

    bool added = _ticketStore.AddComment(
        id, CommentAuthor, CommentMessage);

    if (!added)
    {
        return NotFound();
    }

    return RedirectToPage(new { id });
}

private IActionResult ReloadPage(int id)
{
    Ticket = _ticketStore.GetById(id);

    if (Ticket is null)
    {
        return NotFound();
    }

    LoadComments(id);
    return Page();
}

}
