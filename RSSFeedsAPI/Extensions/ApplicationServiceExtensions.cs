using Microsoft.EntityFrameworkCore;
using RSSFeedsAPI.Data;
using RSSFeedsAPI.Interfaces;
using RSSFeedsAPI.Services;

namespace RSSFeedsAPI.Extensions
{
  public static class ApplicationServiceExtensions
  {
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
    {
      services.AddDbContext<DataContext>(opt =>
      {
        opt.UseSqlite(config.GetConnectionString("DefaultConnection"));
      });
      services.AddCors();
      services.AddScoped<ITokenService, TokenService>();
      services.AddScoped<IUserRepository, UserRepository>();
      services.AddScoped<INewsRepository, NewsRepository>();
      services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
      services.AddSwaggerGen();
      return services;
    }
  }
}
