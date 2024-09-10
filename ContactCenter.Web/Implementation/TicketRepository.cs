using ContactCenter.Data;
using ContactCenter.Web.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ContactCenter.Web.Implementation
{
    public class TicketRepository : ITicketRepository
    {
        private readonly EDRSMContext _edrsmContext;

        public TicketRepository(
            EDRSMContext edrsmContext
            )
        {
            _edrsmContext = edrsmContext;
        }

        public async Task<IReadOnlyList<Ticket>> GetAllAsync()
        {
            return await _edrsmContext.Tickets.Include(x=>x.TicketAudits).ToListAsync();
        }

        public async Task<Ticket> GetByIdAsync(Guid id)
        {
            return await _edrsmContext.Tickets.FindAsync(id);
        }

        public async Task<Ticket> AddAsync(Ticket ticket)
        {
            _edrsmContext.Tickets.Add(ticket);
            await _edrsmContext.SaveChangesAsync();
            return ticket;
        }

        public async Task<bool> UpdateAsync(Ticket ticket)
        {
            _edrsmContext.Tickets.Update(ticket);
            return await _edrsmContext.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var ticket = await _edrsmContext.Tickets.FindAsync(id);
            if (ticket == null)
            {
                return false;
            }

            _edrsmContext.Tickets.Remove(ticket);
            return await _edrsmContext.SaveChangesAsync() > 0;
        }
    }

}
