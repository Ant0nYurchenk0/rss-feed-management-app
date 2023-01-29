namespace RSSFeedsAPI.Entities
{
  public class AppUser
  {
    public int Id { get; set; }

    public string UserName { get; set; }

    public byte[] PasswordHash { get; set; }

    public byte[] PasswordSalt { get; set; }
    public List<Feed> Feeds { get; set; } = new();

  }
}
