using RSSFeedsAPI.DTOs;
using RSSFeedsAPI.Entities;

namespace RSSFeedsAPI.Interfaces
{
  public interface IUserRepository
  {
    public Task AddFeedAsync(string username, string url);
    public Task<List<Feed>> GetFeedsAsync(string username);
    public Task<List<News>> GetNewsFromDateAsync(string username, DateTime date);
    public Task UpdateNewsAsync(string username);
  }
}
