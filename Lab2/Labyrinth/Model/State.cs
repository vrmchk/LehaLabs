using Labyrinth.Enums;

namespace Labyrinth.Model;

internal class State : IEquatable<State>
{
    private List<State>? _children;

    public State(Maze maze, State? parent)
    {
        Maze = maze ?? throw new ArgumentNullException(nameof(maze));
        Parent = parent;
        Generation = 1 + parent?.Generation ?? 0;
    }

    public Maze Maze { get; set; }

    public State? Parent { get; set; }

    public int Distance => (int)Math.Ceiling(Cell.DistanceBetween(Maze.Selected, Maze.Destination));

    public int Generation { get; set; }

    public int Evaluation => Distance + Generation;

    public List<State> Children
    {
        get
        {
            if (_children != null)
                return _children;
            
            _children = new List<State>();
            foreach (var direction in Enum.GetValues<Direction>())
            {
                if (TryMove(direction, out var currentChild) && currentChild!.Equals(Parent) == false)
                {
                    _children.Add(currentChild);
                }
            }

            return _children;
        }
    }

    public List<State> GetPath()
    {
        var result = new List<State>();
        var curr = this;
        do
        {
            result.Add(curr);
            curr = curr.Parent;
        } while (curr != null);

        return result;
    }

    private bool TryMove(Direction dir, out State? newState)
    {
        (int horizontal, int vertical) = (0, 0);
        switch (dir)
        {
            case Direction.Up:
                vertical--;
                break;
            case Direction.Down:
                vertical++;
                break;
            case Direction.Left:
                horizontal--;
                break;
            case Direction.Right:
                horizontal++;
                break;
            case Direction.UpLeftDiagonal:
                vertical--;
                horizontal--;
                break;
            case Direction.UpRightDiagonal:
                vertical--;
                horizontal++;
                break;
            case Direction.DownLeftDiagonal:
                vertical++;
                horizontal--;
                break;
            case Direction.DownRightDiagonal:
                vertical++;
                horizontal++;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(dir), dir, null);
        }

        var selected = Maze.Selected;
        var below = Maze[selected.Coordinate.X + vertical, selected.Coordinate.Y + horizontal];

        newState = null;

        if (below == null)
            return false;
        if (below.Type is not CellType.Empty)
            return false;

        var moved = (Maze)Maze.Clone();
        moved.MoveSelection(dir);

        newState = new State(moved, this);
        return true;
    }

    public bool Equals(State? other)
    {
        return other is not null
               && Maze.Equals(other.Maze)
               && Parent == other.Parent;
    }

    public override string ToString()
    {
        return $"Generation: {Generation}, Distance: {Distance}, F: {Evaluation}";
    }

    public override bool Equals(object? obj)
    {
        return Equals(obj as State);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Maze, Parent, Distance, Generation, Evaluation);
    }
}