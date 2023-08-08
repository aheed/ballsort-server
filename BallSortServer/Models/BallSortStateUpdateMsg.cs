namespace BallSortServer.Models;

public record BallSortStateUpdateMsg
{
    public required string UserId { get; init; }
    public required BallSortStateModel State { get; init; }
}