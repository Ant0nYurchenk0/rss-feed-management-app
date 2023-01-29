
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RSSFeedsAPI.Entities;
using RSSFeedsAPI.Extensions;
using RSSFeedsAPI.Interfaces;

namespace RSSFeedsAPI.Controllers
{
  [Authorize]
  public class FeedsController : BaseApiController
  {
    private readonly IUserRepository _userRepository;

    public FeedsController(IUserRepository userRepository)
    {
      _userRepository = userRepository;
    }

    [HttpPost]
    public async Task<ActionResult> Add(string url)
    {
      try
      {
        await _userRepository.AddFeedAsync(User.GetUsername(), url);  
      }
      catch (NullReferenceException ex) 
      {
        return NotFound(ex.Message);
      }
      catch (ArgumentException ex)
      {
        return NotFound(ex.Message);
      }
      return Ok();
    }
    [HttpGet]
    public async Task<ActionResult<List<string>>> Get()
    {
      try
      {
        var feeds = await _userRepository.GetFeedsAsync(User.GetUsername());
        var urls = feeds.Select(f => f.Url);
        return Ok(urls);
      }
      catch (NullReferenceException ex)
      {
        return NotFound(ex.Message);
      }
    }
    [HttpPost("get-news")]
    public async Task<ActionResult<List<string>>> Get(DateTime date)
    {
      try
      {
        await _userRepository.UpdateNewsAsync(User.GetUsername());
        return Ok(await _userRepository.GetNewsFromDateAsync(User.GetUsername(), date));
      }
      catch (NullReferenceException ex)
      {
        return NotFound(ex.Message);
      }     
    }
  }
}
