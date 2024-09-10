using Microsoft.AspNetCore.Mvc;
using ContactCenter.Web.Models;
using ContactCenter.Data;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;

namespace ContactCenter.Web.Controllers
{
    public class AuthController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AuthController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Signup()
        {
            var model = new SignupViewModel
            {
                IdentificationTypes = new List<SelectListItem>
                {
                    new SelectListItem { Value = "0", Text = "ID" },
                    new SelectListItem { Value = "1", Text = "Passport" }
                },
                Countries = _context.Countries.Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.Name
                }).ToList()
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Signup(SignupViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new EdrsmUser
                {
                    Name = model.Name,
                    Surname = model.Surname,
                    Username = model.Username,
                    PasswordHash = model.PasswordHash, // Password will be hashed in API
                    Email = model.Email,
                    MunicipalityAccountNumber = model.MunicipalityAccountNumber,
                    CellphoneNumber = model.CellphoneNumber,
                    IdentificationNumber = model.IdentificationNumber,
                    IdentificationTypeId = model.IdentificationTypeId, // 0 for ID, 1 for Passport
                    CountryOfOriginId = model.CountryOfOriginId,
                    PreferredContactMethodId = model.PreferredContactMethodId,
                    AgreedToTerms = model.AgreedToTerms,
                };

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("https://yourapiurl.com/");
                    var response = await client.PostAsJsonAsync("api/auth/signup", user);
                    if (response.IsSuccessStatusCode)
                    {
                        return RedirectToAction("Index", "Home");
                    }
                    ModelState.AddModelError(string.Empty, "An error occurred while signing up.");
                }
            }
            model.IdentificationTypes = new List<SelectListItem>
            {
                new SelectListItem { Value = "0", Text = "ID" },
                new SelectListItem { Value = "1", Text = "Passport" }
            };
            model.Countries = _context.Countries.Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.Name
            }).ToList();
            return View(model);
        }
    }
}
