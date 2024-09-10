using ContactCenter.Data;
using ContactCenter.Data.Identity;
using ContactCenter.Data.SeedDataMethods;
using EDRSM.API.Errors;
using EDRSM.API.Extentions;
using EDRSM.API.Implementation;
using EDRSM.API.Interfaces;
using EDRSM.API.Middleware;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddApplicationServices(builder.Configuration);
builder.Services.AddIdentityServices(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseMiddleware<ExceptionMiddleware>();
app.UseStatusCodePagesWithReExecute("/errors/{0}");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseStaticFiles();
app.UseCors("CorsPolicy");

//app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

using var scope = app.Services.CreateScope();
var services = scope.ServiceProvider;
var edrsmContext = services.GetRequiredService<EDRSMContext>();
var identityContext = services.GetRequiredService<EdrsmIdentityDbContext>();
var userManager = services.GetRequiredService<UserManager<EdrsmAppUser>>();
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
