using RSSFeedsAPI.Entities;

namespace RSSFeedsAPI.Interfaces
{
  public interface ITokenService
  {
    string CreateToken(AppUser user);

  }
}
