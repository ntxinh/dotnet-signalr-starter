using Microsoft.AspNetCore.Mvc;
using dotnet_signalr_starter.Data;
using dotnet_signalr_starter.Providers;

namespace dotnet_signalr_starter.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class NewsController : ControllerBase
{
    private readonly NewsContext _newsContext;
    public NewsController(NewsContext newsContext)
    {
        _newsContext = newsContext;
    }

    [HttpGet]
    public IEnumerable<NewsItemEntity> Get()
    {
        return _newsContext.NewsItemEntities.ToList();
    }
}
