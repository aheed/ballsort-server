using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using BallSortServer.Models;
using WebSocket.Core;

namespace BallSortServer.Services;

public interface ISubscriptionsMgr
{
    Task AddSubscriber(string id, IPushClient pushClient);
    void RemoveSubscriber(string id, IPushClient pushClient);
}
