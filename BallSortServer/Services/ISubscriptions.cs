using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using BallSortServer.Models;

namespace BallSortServer.Services;

public interface ISubscriptions
{
    void AddSubscriber(string id);
    void RemoveSubscriber(string id);
}
