using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using BallSortServer.Models;

namespace BallSortServer.Services;

public interface ISubscriptionsMgr
{
    void AddSubscriber(string id);
    void RemoveSubscriber(string id);
}
