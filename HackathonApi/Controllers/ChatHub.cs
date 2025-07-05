using HackathonApi.Controllers;
using Microsoft.AspNetCore.SignalR;

public class ChatHub : Hub
{
    public static ServiceProvider ServiceProvider;

    public async Task PostMessage(ChatLine chatLine)
    {
        var chat = ServiceProvider.GetService(typeof(IChatService)) as IChatService;
        
        chat.SaveMessage(chatLine);

        var line = chat.GetMessage(chatLine.Id);
        
        await Clients.Group(chatLine.Key).SendAsync("NewLine", line);
    }

    public async Task Register(string key)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, key);
    }

    public async Task DeleteLine(int id)
    {
        var chat = ServiceProvider.GetService(typeof(IChatService)) as IChatService;
        
        var line = chat.GetMessage(id);
        await chat.DeleteMessage(id);
        
        
        await Clients.Group(line.Key).SendAsync("Update");

    }
}