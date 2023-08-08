namespace BallSortServer.Models;

public record BallSortStateModel
{
    public required int NofRows  { get; set; }
    public required int NofCols { get; set; }
    public required int PosX { get; set; }
    public required int PosY { get; set; }
}
