using Microsoft.AspNetCore.Mvc;
using dotnet_signalr_starter.Data;
using dotnet_signalr_starter.Providers;

namespace dotnet_signalr_starter.Controllers;

[ApiController]
[Route("api/[controller]")]
public class NewsController : ControllerBase
{
    // private readonly NewsContext _newsContext;
    private readonly NewsStore _newsStore;
    public NewsController(NewsContext newsContext, NewsStore newsStore)
    {
        // _newsContext = newsContext;
        _newsStore = newsStore;
    }

    // [HttpGet]
    // public IEnumerable<NewsItemEntity> Get()
    // {
    //     return _newsContext.NewsItemEntities.ToList();
    // }

    [HttpPost]
    public IActionResult AddGroup([FromQuery] string group)
    {
        if (string.IsNullOrEmpty(group))
        {
            return BadRequest();
        }

        _newsStore.AddGroup(group);

        return Created("AddGroup", group);
    }

    [HttpGet]
    public List<string> GetAllGroups()
    {
        return _newsStore.GetAllGroups();
    }
}
