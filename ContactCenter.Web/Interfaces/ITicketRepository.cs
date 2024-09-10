using ContactCenter.Data;

namespace ContactCenter.Web.Interfaces
{
    public interface ITicketRepository
    {
        Task<IReadOnlyList<Ticket>> GetAllAsync();
        Task<Ticket> GetByIdAsync(Guid id);
        Task<Ticket> AddAsync(Ticket ticket);
        Task<bool> UpdateAsync(Ticket ticket);
        Task<bool> DeleteAsync(Guid id);
    }
}
