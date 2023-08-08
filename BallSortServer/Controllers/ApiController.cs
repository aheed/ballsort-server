using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using BallSortServer.Models;
using BallSortServer.Services;

namespace BallSortServer.Controllers;

public class ApiController : Controller
{
    private readonly ILogger<ApiController> _logger;
    private readonly IStateUpdater _stateUpdater;

    public ApiController(ILogger<ApiController> logger, IStateUpdater stateUpdater)
    {
        _logger = logger;
        _stateUpdater = stateUpdater;
    }

    public string Get() {
        return "Welcome To BallSort Web API";
    }

    [HttpPost]
    public IActionResult Update([FromBody] BallSortStateUpdateMsg? updateMsg)
    {   
        _logger.LogInformation("Got an update request");
        if(updateMsg == null)
        {
            return BadRequest("Invalid format\n");
        }

        _logger.LogInformation("Got an update request: {cols}", updateMsg.State.NofCols);
        _stateUpdater.UpdateState(updateMsg.State, updateMsg.UserId);

        return Json(updateMsg); //temp
    }
}
