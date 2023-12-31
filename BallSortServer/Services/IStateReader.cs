﻿using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using BallSortServer.Models;

namespace BallSortServer.Services;

public interface IStateReader
{
    BallSortStateModel GetState(string userId);
}
