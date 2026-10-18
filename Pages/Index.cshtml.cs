
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ServiceDeskLite.Models;
using ServiceDeskLite.Services;

namespace ServiceDeskLite.Pages;

public class IndexModel : PageModel
{
    private readonly TicketStore _ticketStore;

    public IndexModel(TicketStore ticketStore)
    {
        _ticketStore = ticketStore;
    }

    public List<Ticket> Tickets { get; set; } = new();
    public List<Ticket> AllTickets { get; set; } = new();

    [BindProperty(SupportsGet = true)]
    public string SearchTerm { get; set; } = string.Empty;

    [BindProperty(SupportsGet = true)]
    public string StatusFilter { get; set; } = string.Empty;

    [BindProperty(SupportsGet = true)]
    public string CategoryFilter { get; set; } = string.Empty;

    [BindProperty(SupportsGet = true)]
    public string PriorityFilter { get; set; } = string.Empty;

    public int TotalTickets => AllTickets.Count;

    public int OpenTickets =>
        AllTickets.Count(ticket => ticket.Status == "Open");

    public int InProgressTickets =>
        AllTickets.Count(ticket => ticket.Status == "In Progress");

    public int ResolvedTickets =>
        AllTickets.Count(ticket => ticket.Status == "Resolved");

    public void OnGet()
    {
        AllTickets = _ticketStore.GetAll();

        IEnumerable<Ticket> filteredTickets = AllTickets;

        if (!string.IsNullOrWhiteSpace(SearchTerm))
        {
            string search = SearchTerm.Trim();

            filteredTickets = filteredTickets.Where(ticket =>
                ticket.Subject.Contains(
                    search, StringComparison.OrdinalIgnoreCase) ||
                ticket.RequesterName.Contains(
                    search, StringComparison.OrdinalIgnoreCase));
        }

        if (!string.IsNullOrWhiteSpace(StatusFilter))
        {
            filteredTickets = filteredTickets.Where(ticket =>
                ticket.Status == StatusFilter);
        }

        if (!string.IsNullOrWhiteSpace(CategoryFilter))
        {
            filteredTickets = filteredTickets.Where(ticket =>
                ticket.Category == CategoryFilter);
        }

        if (!string.IsNullOrWhiteSpace(PriorityFilter))
        {
            filteredTickets = filteredTickets.Where(ticket =>
                ticket.Priority == PriorityFilter);
        }

        Tickets = filteredTickets.ToList();
    }
}
