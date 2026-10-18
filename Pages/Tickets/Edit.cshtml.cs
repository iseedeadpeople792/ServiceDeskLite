
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ServiceDeskLite.Models;
using ServiceDeskLite.Services;

namespace ServiceDeskLite.Pages.Tickets;

public class EditModel : PageModel
{
    private readonly TicketStore _ticketStore;

    public EditModel(TicketStore ticketStore)
    {
        _ticketStore = ticketStore;
    }

    [BindProperty]
    public Ticket Ticket { get; set; } = new();

    public IActionResult OnGet(int id)
    {
        Ticket? ticket = _ticketStore.GetById(id);

        if (ticket is null)
        {
            return NotFound();
        }

        Ticket = ticket;
        return Page();
    }

    public IActionResult OnPost()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        bool updated = _ticketStore.UpdateTicket(Ticket);

        if (!updated)
        {
            ModelState.AddModelError(
                string.Empty,
                "Не удалось обновить заявку. Проверьте введённые данные.");

            return Page();
        }

        TempData["SuccessMessage"] = "Заявка успешно обновлена.";

        return RedirectToPage("/Tickets/Details", new { id = Ticket.Id });
    }
}
