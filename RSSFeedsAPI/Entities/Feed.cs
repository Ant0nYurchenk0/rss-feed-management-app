using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RSSFeedsAPI.Entities
{
  public class Feed
  {
    [Key]
    public int Id { get; set; }
    public string Url { get; set; }
    public List<News> News { get; set; } = new();
  }
}

