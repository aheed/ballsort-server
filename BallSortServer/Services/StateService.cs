using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using BallSortServer.Models;
using WebSocket.Core;

namespace BallSortServer.Services;

public class StateService : IStateReader, IStateUpdater, ISubscriptionsMgr
{
    private readonly object _lock = new();
    private readonly ILogger<StateService> _logger;
    private BallSortStateModel _currentState = new(); // todo: make it an instance per user
    private readonly ClientCollection _pushClients = new();
    private readonly Dictionary<string, BallSortStateModel> _states = new();

    public StateService(ILogger<StateService> logger)
    {
        _logger = logger;
    }

    // IStateReader implementation
    public BallSortStateModel GetState() => _currentState;
    
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
            _states.TryGetValue(id, out currentState);
        }

        if (currentState != null) 
        {
            await pushClient.UpdateState(_currentState);
        }
    }

    public void RemoveSubscriber(string id, IPushClient pushClient)
    {
        lock(_lock) 
        {
            _pushClients.Remove(id, pushClient);
        }
    }
}
