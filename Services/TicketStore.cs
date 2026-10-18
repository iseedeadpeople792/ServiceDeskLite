
using Microsoft.EntityFrameworkCore;
using ServiceDeskLite.Data;
using ServiceDeskLite.Models;

namespace ServiceDeskLite.Services;

public class TicketStore
{
    private readonly AppDbContext _db;

    public TicketStore(AppDbContext db)
    {
        _db = db;
    }

    public List<Ticket> GetAll()
    {
        return _db.Tickets
            .AsNoTracking()
            .OrderByDescending(ticket => ticket.CreatedAt)
            .ToList();
    }

    public Ticket? GetById(int id)
    {
        return _db.Tickets.FirstOrDefault(ticket => ticket.Id == id);
    }

    public Ticket Add(Ticket ticket)
    {
        ticket.Id = 0;
        ticket.CreatedAt = DateTime.UtcNow;
        ticket.Status = "Open";

        _db.Tickets.Add(ticket);
        _db.SaveChanges();

        return ticket;
    }

    public bool UpdateStatus(int id, string status)
    {
        string[] allowedStatuses =
        {
            "Open", "In Progress", "Resolved"
        };

        if (!allowedStatuses.Contains(status))
        {
            return false;
        }

        Ticket? ticket = GetById(id);

        if (ticket is null)
        {
            return false;
        }

        ticket.Status = status;
        _db.SaveChanges();

        return true;
    }

    
public List<TicketComment> GetComments(int ticketId)
{
    return _db.TicketComments
        .AsNoTracking()
        .Where(comment => comment.TicketId == ticketId)
        .OrderBy(comment => comment.CreatedAt)
        .ToList();
}

public bool AddComment(int ticketId, string author, string message)
{
    Ticket? ticket = _db.Tickets.Find(ticketId);

    if (ticket is null ||
        string.IsNullOrWhiteSpace(author) ||
        string.IsNullOrWhiteSpace(message))
    {
        return false;
    }

    var comment = new TicketComment
    {
        TicketId = ticketId,
        Author = author.Trim(),
        Message = message.Trim(),
        CreatedAt = DateTime.UtcNow
    };

    _db.TicketComments.Add(comment);
    _db.SaveChanges();

    return true;
}


public bool UpdateTicket(Ticket updatedTicket)
{
    Ticket? ticket = _db.Tickets.Find(updatedTicket.Id);

    if (ticket is null)
    {
        return false;
    }

    if (string.IsNullOrWhiteSpace(updatedTicket.Subject) ||
        string.IsNullOrWhiteSpace(updatedTicket.Description) ||
        string.IsNullOrWhiteSpace(updatedTicket.RequesterName))
    {
        return false;
    }

    if (updatedTicket.Subject.Length > 100 ||
        updatedTicket.Description.Length > 2000 ||
        updatedTicket.RequesterName.Length > 80)
    {
        return false;
    }

    string[] allowedCategories =
    {
        "Hardware", "Software", "Network", "Account"
    };

    string[] allowedPriorities =
    {
        "Low", "Medium", "High"
    };

    if (!allowedCategories.Contains(updatedTicket.Category) ||
        !allowedPriorities.Contains(updatedTicket.Priority))
    {
        return false;
    }

    ticket.Subject = updatedTicket.Subject.Trim();
    ticket.Description = updatedTicket.Description.Trim();
    ticket.RequesterName = updatedTicket.RequesterName.Trim();
    ticket.Category = updatedTicket.Category;
    ticket.Priority = updatedTicket.Priority;

    _db.SaveChanges();

    return true;
}

}
