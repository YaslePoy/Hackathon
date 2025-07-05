using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HackathonApi.Controllers;

[Route("api/chat")]
[ApiExplorerSettings]
public class ChatController : Controller
{
    private readonly IChatService _chatService;

    public ChatController(IChatService chatService)
    {
        _chatService = chatService;
    }

    [HttpGet("{key}")]
    public List<ChatLine> GetMessagesAsync(string key)
    {
        return _chatService.GetAllMessages(key).ToList();
    }
}
public interface IChatService
{
    IReadOnlyList<ChatLine> GetAllMessages(string key);
    void SaveMessage(ChatLine chatLine);
    ChatLine GetMessage(int chatLineId);
    Task DeleteMessage(int chatLineId);
}

public class ChatService(HckContext kino) : IChatService
{
    public IReadOnlyList<ChatLine> GetAllMessages(string key)
    {
        return kino.Chat.Where(i => i.Key == key).Include(i => i.User).ToList();
    }

    public void SaveMessage(ChatLine chatLine)
    {
        kino.Chat.Add(chatLine);
        kino.SaveChanges();
    }

    public ChatLine GetMessage(int chatLineId)
    {
        return kino.Chat.Include(i => i.User).FirstOrDefault(i => i.Id == chatLineId);
    }

    public async Task DeleteMessage(int chatLineId)
    {
        kino.Chat.Remove(kino.Chat.FirstOrDefault(i => i.Id == chatLineId));
        await kino.SaveChangesAsync();
    }
}