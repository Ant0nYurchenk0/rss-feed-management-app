using System.ComponentModel.DataAnnotations;
using System.Reflection.PortableExecutable;

namespace RSSFeedsAPI.Entities
{
  public class News
  {
    [Key]
    public string Id { get; set; }
    public bool IsRead { get; set; }
    public string Title { get; set; }
    public string Link { get; set; }
    public string Content { get; set; }
    public DateTime PubDate { get; set; }
    public string Categories { get; set; }

  }
}
