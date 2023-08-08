using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using BallSortServer.Models;
using WebSocket.Core;

namespace BallSortServer.Services;

public class StateService : IStateReader, IStateUpdater, ISubscriptionsMgr
{
    private readonly object _lock = new();
    private readonly ILogger<StateService> _logger;
    //private BallSortStateModel _currentState = new(); // todo: make it an instance per user
    private readonly ClientCollection _pushClients = new();
    private readonly Dictionary<string, BallSortStateModel> _states = new();

    public StateService(ILogger<StateService> logger)
    {
        _logger = logger;
    }

    private static BallSortStateModel GetDefaultState() => new(3, 5, 0, 0);

    // IStateReader implementation
    public BallSortStateModel GetState(string userId)
    {
        if (_states.TryGetValue(userId, out BallSortStateModel? currentState))
        {
            return currentState;
        }

        return GetDefaultState();
    }
    
    // IStateUpdater implementation
    public async Task UpdateState(BallSortStateModel newState, string userId)
    {
        IEnumerable<IPushClient> pushClients;

        lock(_lock)
        {
            //_currentState = newState;
            _states[userId] = newState;
            pushClients = _pushClients.GetClients(userId);
        }

        var pushTasks = pushClients.Select(client => client.UpdateState(newState));
        await Task.WhenAll(pushTasks);
    }

    // ISubscriptions implementation
    public async Task AddSubscriber(string id, IPushClient pushClient)
    {
        BallSortStateModel? currentState;

        lock(_lock) 
        {
            _pushClients.Add(id, pushClient);
            currentState = GetState(id);
        }

        await pushClient.UpdateState(currentState);
    }

    public void RemoveSubscriber(string id, IPushClient pushClient)
    {
        lock(_lock) 
        {
            _pushClients.Remove(id, pushClient);
        }
    }
}
