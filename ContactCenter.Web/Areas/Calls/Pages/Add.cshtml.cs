using ContactCenter.Data;
using ContactCenter.Lib;
using ContactCenter.Web.Pages;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace ContactCenter.Web.Areas.Calls.Pages
{
    public class AddModel : SysPageModel
    {

        [BindProperty]
        public long StartTime { get; set; }
        [BindProperty]
        public Contact Contact { get; set; }

        [BindProperty]
        public Call Call { get; set; }

        [BindProperty]
        public Ticket Ticket { get; set; }

        [BindProperty]
        public string Number { get; set; }
        [BindProperty]
        public string Comments { get; set; }

        public SelectList CallCategories { get; set; }
        public SelectList TicketCategories { get; set; }
        public SelectList Locations { get; set; }

        public List<Ticket> Tickets { get; private set; }
        public List<Call> Calls { get; private set; }

        public async Task OnGetAsync(string Id)
        {
            if (Id != null) Id = long.Parse(Id.Replace(" ", "")).ToString();
            Number = Id;
            CallCategories = new SelectList(await Db.CallCategories.OrderBy(c => c.Name).ToListAsync(), nameof(CallCategory.Id), nameof(CallCategory.Name));
            TicketCategories = new SelectList(await Db.TicketCategories.OrderBy(c => c.Name).ToListAsync(), nameof(CallCategory.Id), nameof(CallCategory.Name));
            Locations = new SelectList(await Db.Locations.OrderBy(c => c.Name).ToListAsync(), nameof(CallCategory.Id), nameof(CallCategory.Name));
            Title = "Start a new call..";
            Contact = await Db.Contacts
                .Include(c => c.Tickets)
                .Include(c => c.Calls).ThenInclude(c => c.Agent)
                .FirstOrDefaultAsync(c => c.Id == Id);
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var contact = await Db.Contacts.FirstOrDefaultAsync(c => c.Id == Number);
            if (contact == null)
            {
                Contact.CreationDate = DateTime.Now;
                Contact.CreatorId = CurrentUserId;
                Contact.Id = Number;
                Db.Contacts.Add(Contact);
            }
            else
            {
                contact.Name = Contact.Name;
                if (!string.IsNullOrWhiteSpace(Contact.Address)) contact.Address = Contact.Address;
                if (!string.IsNullOrWhiteSpace(Contact.Email)) contact.Email = Contact.Email;
            }
            Call.LocationId = Contact.LocationId;
            Call.Id = Guid.NewGuid();
            Call.StartTime = new DateTime(StartTime);
            Call.EndTime = DateTime.Now;
            Call.AgentId = CurrentUserId;
            Call.ContactId = Number;
            Call.Direction = CallDirection.INBOUND;
            Call.Extension = CurrentAgent.Extension;
            if (!string.IsNullOrWhiteSpace(Comments)) Call.AddNotes(new Lib.Note
            {
                Comments = Comments,
                Date = DateTime.Now,
                UserId = CurrentUserId,
                Username = CurrentUser.Name,
            });
            Db.Calls.Add(Call);
           await CreateTicket();
            await Db.SaveChangesAsync();
            return RedirectToPage("./Index");
        }

        async Task CreateTicket()
        {
            if (Ticket?.TypeId > 0)
            {
                Ticket.CreationDate = DateTime.Now;
                Ticket.Status = ContactCenter.Lib.TicketStatus.PENDING;
                Ticket.Id = Guid.NewGuid();
                Ticket.LocationId = Call.LocationId;
                Ticket.CreatorId = CurrentUserId.ToString();
                Ticket.AssigneeId = CurrentUserId;
                Ticket.AssignmentDate = DateTime.Now;
                Ticket.ContactId = Number;
                var thisMonth = DateTime.Now.Date.AddDays(1 - DateTime.Now.Day);
                var count = (await Db.Tickets.CountAsync(c => c.CreationDate > thisMonth) + 1) % 1000;
                switch (Ticket.Type)
                {
                    case Lib.TicketType.INCIDENT:
                        Ticket.Number = $"IN{DateTime.Now.Year.ToString()[^1]}{DateTime.Now:MM}{count:0000}";
                        break;
                    case Lib.TicketType.PROBLEM:
                        Ticket.Number = $"PR{DateTime.Now.Year.ToString()[^1]}{DateTime.Now:MM}{count:0000}";
                        break;
                    case Lib.TicketType.SERVICE_REQUEST:
                        Ticket.Number = $"SR{DateTime.Now.Year.ToString()[^1]}{DateTime.Now:MM}{count:0000}";
                        break;
                    case Lib.TicketType.CHANGE_REQUEST:
                        Ticket.Number = $"CR{DateTime.Now.Year.ToString()[^1]}{DateTime.Now:MM}{count:0000}";
                        break;
                    default:
                        break;
                }
                Db.Tickets.Add(Ticket);
            }
        }
    }
}
