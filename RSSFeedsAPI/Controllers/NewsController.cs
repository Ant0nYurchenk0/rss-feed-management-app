using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RSSFeedsAPI.Interfaces;

namespace RSSFeedsAPI.Controllers
{
  [Authorize]
  public class NewsController : BaseApiController
  {
    private readonly INewsRepository _repository;

    public NewsController(INewsRepository repository)
    {
      _repository = repository;
    }

    [HttpPatch]
    public async Task<ActionResult> ReadNews(string id)
    {
      try
      {
        var news = await _repository.ReadNews(id);
        return Ok(news);
      }
      catch (NullReferenceException ex)
      {
        return NotFound(ex.Message);
      }
      catch(ArgumentException ex)
      {
        return BadRequest(ex.Message);
      }
    }
  }
}
