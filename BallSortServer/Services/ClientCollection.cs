using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using BallSortServer.Models;
using WebSocket.Core;

namespace BallSortServer.Services;

public class ClientCollection
{
    private readonly Dictionary<string, List<IPushClient>> _clientCollections = new();
    public void Add(string userId, IPushClient pushClient)
    {
        if (!_clientCollections.TryGetValue(userId, out List<IPushClient>? clientCollection))
        {
            clientCollection = new List<IPushClient>();
        }

        clientCollection.Add(pushClient);
        _clientCollections[userId] = clientCollection;
    }
    
    public void Remove(string userId, IPushClient pushClient)
    {
        if (!_clientCollections.TryGetValue(userId, out List<IPushClient>? clientCollection))
        {
            return;
        }

        clientCollection.Remove(pushClient);
    }

    public IEnumerable<IPushClient> GetClients(string userId)
    {
        if (!_clientCollections.TryGetValue(userId, out List<IPushClient>? clientCollection))
        {
            clientCollection = new List<IPushClient>();
        }

        return clientCollection;
    }
}
