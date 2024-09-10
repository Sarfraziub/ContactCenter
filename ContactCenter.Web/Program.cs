using ContactCenter.Data;
using ContactCenter.Web;
using ContactCenter.Lib;
using Microsoft.AspNetCore.Identity;
using wCyber.Helpers.Identity.Auth;
using Microsoft.EntityFrameworkCore;
using EDRSM.API.Extentions;
using ContactCenter.Data.Identity;
using ContactCenter.Data.SeedDataMethods;
using EDRSM.API.Middleware;
using ContactCenter.Web.Implementation;
using ContactCenter.Web.Interfaces;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddApplicationServices(builder.Configuration);
builder.Services.AddIdentityServices(builder.Configuration);
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<EDRSMContext>(options => options.UseNpgsql(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddScoped<ITicketRepository, TicketRepository>(); // Assuming 'TicketRepository' is your concrete implementation

//Configure auth
builder.ConfigureAuth();
// Add services to the container.
builder.Services.AddRazorPages();

var app = builder.Build();
// Configure the HTTP request pipeline.
app.UseMiddleware<ExceptionMiddleware>();
app.UseStatusCodePagesWithReExecute("/errors/{0}");

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseCors("CorsPolicy");

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();  // Ensure API controllers are mapped
    endpoints.MapDefaultControllerRoute();  // For MVC routing
});


app.MapRazorPages();
//app.MapControllers();
using var scope = app.Services.CreateScope();
var services = scope.ServiceProvider;
var edrsmContext = services.GetRequiredService<EDRSMContext>();
var identityContext = services.GetRequiredService<EdrsmIdentityDbContext>();
var userManager = services.GetRequiredService<UserManager<ContactUser>>();
var logger = services.GetRequiredService<ILogger<Program>>();
try
{
    //await edrsmContext.Database.MigrateAsync();
    //await identityContext.Database.MigrateAsync();
    await EdrsmContextSeed.SeedAsync(edrsmContext);
    await EdrsmAppUserSeed.SeedUsersAsync(userManager);
}
catch (Exception ex)
{
    logger.LogError(ex, "An error occured during migration");
}
app.Run();
