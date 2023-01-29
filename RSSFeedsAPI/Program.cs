using Microsoft.EntityFrameworkCore;
using RSSFeedsAPI.Data;
using RSSFeedsAPI.Extensions;

internal class Program
{
  private static async Task Main(string[] args)
  {
    var builder = WebApplication.CreateBuilder(args);


    builder.Services.AddControllers();
    builder.Services.AddApplicationServices(builder.Configuration);
    builder.Services.AddIdentityServices(builder.Configuration);

    var app = builder.Build();

    if (app.Environment.IsDevelopment())
    {
      app.UseSwagger();
      app.UseSwaggerUI();
    }

    app.UseAuthentication();
    app.UseAuthorization();

    app.MapControllers();

    using var scope = app.Services.CreateScope();
    var services = scope.ServiceProvider;
    try
    {
      var context = services.GetRequiredService<DataContext>();
      await context.Database.MigrateAsync();
    }
    catch (Exception ex)
    {
      var logger = services.GetService<ILogger<Program>>();
      logger.LogError(ex, "An error occured during migration");
    }

    app.Run();
  }
}