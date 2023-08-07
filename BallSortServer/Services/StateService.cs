using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using BallSortServer.Models;

namespace BallSortServer.Services;

public class StateService : IStateReader, IStateUpdater, ISubscriptionsMgr
{
    private readonly object _lock = new();
    private readonly ILogger<StateService> _logger;
    private BallSortStateModel currentState;

    public StateService(ILogger<StateService> logger)
    {
        _logger = logger;
        currentState = new();
    }

    // IStateReader implementation
    public BallSortStateModel GetState() => currentState;
    
    // IStateUpdater implementation
    public void UpdateState(BallSortStateModel newState)
    {
        lock(_lock) 
        {
            currentState = newState;
        }
    }

    // ISubscriptions implementation
    public void AddSubscriber(string id)
    {
        lock(_lock) 
        {
        }
    }

    public void RemoveSubscriber(string id)
    {
        lock(_lock) 
        {
        }
    }
}
