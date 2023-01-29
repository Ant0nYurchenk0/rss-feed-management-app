using RSSFeedsAPI.Entities;

namespace RSSFeedsAPI.Interfaces
{
  public interface INewsRepository
  {
    public Task<News> ReadNews(string id);
  }
}
