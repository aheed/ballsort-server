using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using BallSortServer.Models;

namespace WebSocket.Core;

public interface IPushClient
{
    Task UpdateState(BallSortStateModel newState);
}
