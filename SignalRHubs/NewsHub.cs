using dotnet_signalr_starter.Providers;
using Microsoft.AspNetCore.SignalR;

namespace dotnet_signalr_starter.SignalRHubs;

public class NewsHub : Hub
{
    private readonly NewsStore _newsStore;

    public NewsHub(NewsStore newsStore)
    {
        _newsStore = newsStore;
    }

    public Task Send(NewsItem newsItem)
    {
        if(!_newsStore.GroupExists(newsItem.NewsGroup))
        {
            throw new Exception("cannot send a news item to a group which does not exist.");
        }

        _newsStore.CreateNewItem(newsItem);
        return Clients.Group(newsItem.NewsGroup).SendAsync(NewsHubActions.Send, newsItem);
    }

    public async Task JoinGroup(string groupName)
    {
        if (!_newsStore.GroupExists(groupName))
        {
            throw new Exception("cannot join a group which does not exist.");
        }

        await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
        await Clients.Group(groupName).SendAsync(NewsHubActions.JoinGroup, groupName);

        var history = _newsStore.GetAllNewsItems(groupName);
        await Clients.Client(Context.ConnectionId).SendAsync(NewsHubActions.History, history);
    }

    public async Task LeaveGroup(string groupName)
    {
        if (!_newsStore.GroupExists(groupName))
        {
            throw new Exception("cannot leave a group which does not exist.");
        }

        await Clients.Group(groupName).SendAsync(NewsHubActions.LeaveGroup, groupName);
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
    }
}
