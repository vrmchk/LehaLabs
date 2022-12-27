using Labyrinth.Enums;

namespace Labyrinth.Model;

internal class Cell : ICloneable
{
    public Cell(CellType type, (int X, int Y) coordinate)
    {
        Type = type;
        Coordinate = coordinate;
    }

    public (int X, int Y) Coordinate { get; private set; }

    public CellType Type { get; set; }

    public static double DistanceBetween(Cell source, Cell destination)
    {
        double distance = Math.Pow(source.Coordinate.X - destination.Coordinate.X, 2)
                                + Math.Pow(source.Coordinate.Y - destination.Coordinate.Y, 2);
                          

        return Math.Sqrt(distance);
    }

    public object Clone()
    {
        return new Cell(Type, Coordinate);
    }
}