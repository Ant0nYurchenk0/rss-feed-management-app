using RSSFeedsAPI.Interfaces;
using Microsoft.EntityFrameworkCore;
using RSSFeedsAPI.Entities;
using System.Xml;
using System.ServiceModel.Syndication;
using System.Threading;

namespace RSSFeedsAPI.Data
{
  public class UserRepository : IUserRepository
  {
    private readonly DataContext _context;

    public UserRepository(DataContext context)
    {
      _context = context;
    }

    public async Task AddFeedAsync(string username, string url)
    {
      var user = await _context.Users.Include(u=>u.Feeds).FirstOrDefaultAsync(u => u.UserName == username);
      if (user == null) throw new NullReferenceException("user not found");
      if(user.Feeds.Any(f=>f.Url == url)) throw new ArgumentException("feed already added");
      user.Feeds.Add(new Feed { Url = url });
      await _context.SaveChangesAsync();
    }

    public async Task<List<Feed>> GetFeedsAsync(string username)
    {
      var user = await _context.Users.Include(u => u.Feeds).FirstOrDefaultAsync(u => u.UserName == username);
      if (user == null) throw new NullReferenceException("user not found");
      var result = user.Feeds;
      return result;
    }

    public async Task<List<News>> GetNewsFromDateAsync(string username, DateTime date)
    {
      var user = await _context.Users.Include(u => u.Feeds).ThenInclude(f => f.News).FirstOrDefaultAsync(u => u.UserName == username);
      return user.Feeds.SelectMany(f => f.News)
        .Where(n => n.PubDate.CompareTo(date) >= 0
                  && n.IsRead == false)
        .ToList();
    }

    public async Task UpdateNewsAsync(string username)
    {
      var user = await _context.Users.Include(u => u.Feeds).ThenInclude(f=>f.News).FirstOrDefaultAsync(u => u.UserName == username);
      foreach (var feed in user.Feeds)
      {
        XmlReader reader = XmlReader.Create(feed.Url);
        SyndicationFeed syndFeed = SyndicationFeed.Load(reader);
        reader.Close();
        foreach (SyndicationItem item in syndFeed.Items)
        {
          var a = feed.News.ToList();
          var id = item.Id.ToString();
          if (feed.News.Any(n => n.Id == id))
            continue;
          var news = new News()
          {
            Id = id,
            Title = item.Title?.Text.ToString(),
            Categories = string.Join(' ', item.Categories?.Select(c => c.Name).ToList()),
            PubDate = item.PublishDate.Date,
            Link = item.Links[0].Uri.ToString(),
            Content = item.ElementExtensions.ReadElementExtensions<string>("encoded", "http://purl.org/rss/1.0/modules/content/")[0],
            IsRead = false,
          };
          feed.News.Add(news);
          await _context.SaveChangesAsync();
        }
      }
    }

  }
}
