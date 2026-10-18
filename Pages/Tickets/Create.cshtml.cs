
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ServiceDeskLite.Models;
using ServiceDeskLite.Services;

namespace ServiceDeskLite.Pages.Tickets;

public class CreateModel : PageModel
{
    private readonly TicketStore _ticketStore;

    public CreateModel(TicketStore ticketStore)
    {
        _ticketStore = ticketStore;
    }

    [BindProperty]
    public Ticket Ticket { get; set; } = new();

    public void OnGet()
    {
    }

    public IActionResult OnPost()
    {
        string[] allowedCategories =
        {
            "Hardware", "Software", "Network", "Account"
        };

        string[] allowedPriorities =
        {
            "Low", "Medium", "High"
        };

        if (!allowedCategories.Contains(Ticket.Category))
        {
            ModelState.AddModelError(
                "Ticket.Category", "Select a valid category.");
        }

        if (!allowedPriorities.Contains(Ticket.Priority))
        {
            ModelState.AddModelError(
                "Ticket.Priority", "Select a valid priority.");
        }

        if (!ModelState.IsValid)
        {
            return Page();
        }

        _ticketStore.Add(Ticket);

        TempData["SuccessMessage"] =
            "Your support ticket has been created.";

        return RedirectToPage("/Tickets/Create");
    }
}
