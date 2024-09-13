using ContactCenter.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ContactCenter.Web.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class TicketsController : SysAPIController
    {

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<Ticket>>> GetAll()
        {
            var tickets = await Db.Tickets.Include(x => x.TicketAudits).ToListAsync();
            return Ok(tickets);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Ticket>> GetById(Guid id)
        {
            var ticket = await Db.Tickets.FindAsync(id);
            if (ticket == null)
            {
                return NotFound();
            }
            return Ok(ticket);
        }

        [HttpPost]
        public async Task<ActionResult<Ticket>> Create(Ticket ticket)
        {
            Db.Tickets.Add(ticket);

            await Db.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = ticket.Id }, ticket);
        }


        [HttpPut("{id}")]
        public async Task<ActionResult> Update(Guid id, Ticket ticket)
        {
            if (id != ticket.Id)
            {
                return BadRequest("Ticket ID mismatch");
            }

            var existingTicket = await Db.Tickets.FindAsync(id);
            if (existingTicket == null)
            {
                return NotFound();
            }

            Db.Entry(existingTicket).CurrentValues.SetValues(ticket);

            var updated = await Db.SaveChangesAsync() > 0;
            if (!updated)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while updating the ticket.");
            }

            return NoContent();
        }


        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            var ticket = await Db.Tickets.FindAsync(id);
            if (ticket == null)
            {
                return NotFound();
            }

            Db.Tickets.Remove(ticket);

            var deleted = await Db.SaveChangesAsync() > 0;
            if (!deleted)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while deleting the ticket.");
            }

            return NoContent();
        }

    }
}
