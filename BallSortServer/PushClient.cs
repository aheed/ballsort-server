using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using BallSortServer.Models;
using System.Text;
using System.Net.WebSockets;
using System.Text.Json;

namespace WebSocket.Core;

public class PushClient : IPushClient
{
    private readonly ILogger<PushClient> _logger;
    private readonly System.Net.WebSockets.WebSocket _webSocket;

    public PushClient(System.Net.WebSockets.WebSocket webSocket, ILogger<PushClient> logger)
    {
        _webSocket = webSocket;
        _logger = logger;
    }

    public async Task UpdateState(BallSortStateModel newState)
    {
        _logger.LogInformation("PushClient.UpdateState");
        await _webSocket.SendAsync(Encoding.Default.GetBytes(JsonSerializer.Serialize(newState)), WebSocketMessageType.Text, true, CancellationToken.None);
    }    
}
