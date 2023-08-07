﻿using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using BallSortServer.Models;

namespace BallSortServer.Services;

public interface IStateUpdater
{
    void UpdateState(BallSortStateModel newState);
}
