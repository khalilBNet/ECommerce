using API.Extensions;
using API.Middleware;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddApplicationServices(builder.Configuration);

var app = builder.Build();

// Configure the request pipeline

app.UseMiddleware<ExceptionMiddleware>();
//hedhy amelnaha wakteli zedna errorController bach ki yebda fama request mouch mawjouda par exemple 
// nom controller mafemech nraj3ou msg wadha7 khater kanet traja3 vide
app.UseStatusCodePagesWithReExecute("/errors/{0}");
app.UseHttpsRedirection();
app.UseRouting();
app.UseStaticFiles();
app.UseAuthorization();
app.UseSwagger();
app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "API v1"));

app.MapControllers();

using var scope = app.Services.CreateScope();
var services = scope.ServiceProvider;
var loggerFactory = services.GetRequiredService<ILoggerFactory>();
try
{
    var context = services.GetRequiredService<StoreContext>();
    // apply any pending migrations for the context to the database and create db if not exist
    // it is another way to create database and update it , our example we dropped the database but we have migrations pending and waiting
    await context.Database.MigrateAsync();
    await StoreContextSeed.SeedAsnyc(context, loggerFactory);
}
catch (Exception ex)
{
    var logger = loggerFactory.CreateLogger<Program>();
    logger.LogError(ex, "An error occured during migration khalilo");
}
app.Run();