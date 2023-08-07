using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using BallSortServer.Models;

namespace BallSortServer.Controllers;

public class ApiController : Controller
{
    private readonly ILogger<ApiController> _logger;

    public ApiController(ILogger<ApiController> logger)
    {
        _logger = logger;
    }

    public string Get() {
        return "Welcome To BallSort Web API";
    }

    [HttpPost]
    public IActionResult Update([FromBody] BallSortStateModel newState)
    {    
        _logger.LogInformation("Got an update request: {cols}", newState.NofCols);
        //return Json("Got it!");
        return Json(newState);
    }
}
