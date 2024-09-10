using ContactCenter.Data.Identity;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ContactCenter.Data.SeedDataMethods
{
    public class EdrsmAppUserSeed
    {
        public static async Task SeedUsersAsync(UserManager<ContactUser> userManager)
        {

            if (!userManager.Users.Any())
            {
                var user = new ContactUser
                {
                    Name = "Bob",
                    Surname = "Smith",
                    Email = "bob@test.com",
                    UserName = "bob@test.com",
                    DisplayName = "Bob",
                    MunicipalityAccountNumber = "AGG112",
                    PhoneNumber = "072 930 4444",
                    PreferredContactMethodId = 1,
                    IdentificationTypeId = 1,
                    IdentificationNumber = "950101 1111 222",
                    CountryOfOriginId = 1,
                    AgreedToTerms = true,
                    IsAdmin = true,
                    Ward = 1
                };

                await userManager.CreateAsync(user, "Pa$$w0rd");
            }
        }
    }
}
