using System.Net.WebSockets;
using System.Text;
using BallSortServer.Services;
using Microsoft.AspNetCore.Mvc;

namespace WebSocket.Core;

public class WebSocketMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ISubscriptionsMgr _subscriptionsMgr;
    private readonly ILogger<WebSocketMiddleware> _logger;

    public WebSocketMiddleware(RequestDelegate next, ISubscriptionsMgr subscriptionsMgr, ILogger<WebSocketMiddleware> logger)
    {
        _next = next;
        _subscriptionsMgr = subscriptionsMgr;
        _logger = logger;
    }

    private async Task HandleWebSocket(HttpContext context)
    {
        Console.WriteLine("Handle web socket");

        // Get the underlying socket
        using var socket = await context.WebSockets.AcceptWebSocketAsync();
                
        // 1. Extract useful information from HttpContext
        string requestRoute = context.Request.Path.ToString();
        var token = context.Request.Query["token"];

        // Initialize containers for reading
        bool connectionAlive = true;
        var pushClient = new PushClient(socket, _logger);
        _subscriptionsMgr.AddSubscriber("default", pushClient); //temp

        List<byte> webSocketPayload = new List<byte>(1024 * 4); // 4 KB initial capacity
        byte[] tempMessage = new byte[1024 * 4]; // Message reader

        // 2. Connection loop
        while (connectionAlive)
        {
            // Empty the container
            webSocketPayload.Clear();

            // Message handler
            WebSocketReceiveResult? webSocketResponse;

            // Read message in a loop until fully read
            do
            {
                // Wait until client sends message
                webSocketResponse = await socket.ReceiveAsync(tempMessage, CancellationToken.None);

                // Save bytes
                webSocketPayload.AddRange(new ArraySegment<byte>(tempMessage, 0, webSocketResponse.Count));
            }
            while (webSocketResponse.EndOfMessage == false);
            
            // Process the message
            if (webSocketResponse.MessageType == WebSocketMessageType.Text)
            {
                // 3. Convert textual message from bytes to string
                string message = System.Text.Encoding.UTF8.GetString(webSocketPayload.ToArray());

                Console.WriteLine("Client says {0}", message);
                var echoMsg = $"thanks for {message}!";
                var sendTask = socket.SendAsync(Encoding.Default.GetBytes(echoMsg), WebSocketMessageType.Text, true, CancellationToken.None);
            }
            else if (webSocketResponse.MessageType == WebSocketMessageType.Close)
            {
                // 4. Close the connection
                connectionAlive = false;
                _subscriptionsMgr.RemoveSubscriber("default", pushClient); //todo: replace "default"
            }
        }

        Console.WriteLine("Client disconnected");
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (context.WebSockets.IsWebSocketRequest)
        {
            // Here we will handle the web socket request
            Console.WriteLine("Got a web socket request");
            await HandleWebSocket(context);
        }
        else
        {
            // Handle other requests normally
            Console.WriteLine("Got a non-web socket request");
            await _next(context);
        }
    }
}

public static class WebSocketMiddlewareExtensions
{
    public static IApplicationBuilder UseWebSocketMiddleware(
        this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<WebSocketMiddleware>();
    }
}
