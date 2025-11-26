using System.Security.Claims;
using GreenConnectPlatform.Business.Models.Chat;
using GreenConnectPlatform.Business.Services.Chat;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GreenConnectPlatform.Api.Controllers;

[ApiController]
[Route("api/v1/chat")]
[Tags("18. Chat")]
[Authorize]
public class ChatController(IChatService chatService) : ControllerBase
{
    [HttpGet("rooms")]
    public async Task<IActionResult> GetMyRooms([FromQuery] int pageNumber = 10, [FromQuery] int pageSize = 10)
    {
        var userId = GetCurrentUserId();
        return Ok(await chatService.GetMyChatRoomAsync(userId, pageNumber, pageSize));
    }

    [HttpGet("rooms/{id:Guid}")]
    public async Task<IActionResult> GetMessages([FromRoute] Guid id, [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10)
    {
        return Ok(await chatService.GetChatHistoryAsync(pageNumber, pageSize, id));
    }
    
    [HttpPost("sendMessage")]
    public async Task<IActionResult> SendMessage([FromBody] SendMessageModel request)
    { 
        var result = await chatService.SendMessageAsync(request);
        return Ok(result);
    }
    
    
    private Guid GetCurrentUserId()
    {
        var idStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
        return Guid.TryParse(idStr, out var id) ? id : Guid.Empty;
    }
}