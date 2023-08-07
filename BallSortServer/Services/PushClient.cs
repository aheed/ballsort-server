using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using BallSortServer.Models;

namespace BallSortServer.Services;

public class PushClient : IPushClient
{
    public void UpdateState(BallSortStateModel newState)
    {
        Console.WriteLine("PushClient.UpdateState");
    }    
}
