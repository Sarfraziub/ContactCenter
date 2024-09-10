using ContactCenter.Data;
using ContactCenter.Lib.DataSync;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ContactCenter.Web.API
{
    public class ContactsController : SysAPIController
    {

        [HttpGet("{id}")]
        public async Task<ActionResult<List<IContact>>> Get(string Id)
        {
            var query = Db.Contacts.AsQueryable();
            if (Id.Contains('@'))
            {
                Id = Id.ToLower();
                query = query.Where(c => c.DetailsJson != null && c.DetailsJson.ToLower().Contains(Id));
            }
            else
            {
                Id = long.Parse(Id.Replace(" ", "")).ToString();
                query = query.Where(c => c.Id.EndsWith(Id));
            }
            var data = await query.ToListAsync();
            return Ok(data.Select(contact => new IContact
            {
                Id = contact.Id,
                Address = contact.Address,
                Email = contact.Email,
                FirstName = contact.FirstName,
                LastName = contact.LastName,
                Company = contact.Company,
            }).ToList());
        }
    }
}
