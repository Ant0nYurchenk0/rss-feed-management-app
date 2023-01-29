using RSSFeedsAPI.Entities;
using RSSFeedsAPI.Interfaces;

namespace RSSFeedsAPI.Data
{
  public class NewsRepository : INewsRepository
  {
    private readonly DataContext _context;

    public NewsRepository(DataContext context)
    {
      _context = context;
    }
    public async Task<News> ReadNews(string id)
    {
      var news = _context.News.FirstOrDefault(n=>n.Id == id);
      if (news == null) throw new NullReferenceException("news not found");
      if (news.IsRead == true) throw new ArgumentException("news already read");
      news.IsRead = true;
      await _context.SaveChangesAsync();
      return news;
    }
  }
}
