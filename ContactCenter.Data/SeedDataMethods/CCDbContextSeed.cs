using ContactCenter.Lib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ContactCenter.Data.SeedDataMethods
{
    public class CCDbContextSeed
    {
        public static async Task SeedAsync(CCDbContext context)
        {
            //if (!context.IdentificationTypes.Any())
            //{
            //    var identificationTypesData = File.ReadAllText("../ContactCenter.Data/SeedData/identificationTypes.json");
            //    var identificationTypes = JsonSerializer.Deserialize<List<IdentificationType>>(identificationTypesData);
            //    context.IdentificationTypes.AddRange(identificationTypes);
            //}

            //if (!context.PreferredContactMethods.Any())
            //{
            //    var contactMethodsData = File.ReadAllText("../ContactCenter.Data/SeedData/contactMethods.json");
            //    var contactMethods = JsonSerializer.Deserialize<List<PreferredContactMethod>>(contactMethodsData);
            //    context.PreferredContactMethods.AddRange(contactMethods);
            //}

            if (!context.Countries.Any())
            {
                var countriesData = File.ReadAllText("../ContactCenter.Data/SeedData/countries.json");
                var countries = JsonSerializer.Deserialize<List<Country>>(countriesData);
                context.Countries.AddRange(countries);
            }

            if (!context.Councillors.Any())
            {
                var councillorsData = File.ReadAllText("../ContactCenter.Data/SeedData/councillors.json");
                var councillors = JsonSerializer.Deserialize<List<Councillor>>(councillorsData);
                context.Councillors.AddRange(councillors);
            }

            if (!context.Faqs.Any())
            {
                var faqsData = File.ReadAllText("../ContactCenter.Data/SeedData/faqs.json");
                var faqs = JsonSerializer.Deserialize<List<Faq>>(faqsData);
                context.Faqs.AddRange(faqs);
            }

            if (context.ChangeTracker.HasChanges()) await context.SaveChangesAsync();
        }
    }
}
