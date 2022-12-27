using Labyrinth.Common;
using Labyrinth.Model;

namespace Labyrinth.PathSolver;

internal class AStarPathSolver : IPathSolver
{
    private int _iteration = 0;

    public SearchResult Solve(State state, bool printSteps = false)
    {
        _iteration = 0;
        return AStar(state, printSteps);
    }

    private SearchResult AStar(State state, bool printSteps)
    {
        var open = new HashSet<State>();
        var closed = new HashSet<State>();
        open.Add(state);
        while (open.Count != 0)
        {
            var current = open.MinBy(s => s.Evaluation)!;
            open.Remove(current);
            closed.Add(current);

            _iteration++;
            if (printSteps)
                current.PrintState();

            if (current.Distance == 1)
                return new SearchResult(current, _iteration, open.Count + closed.Count);

            foreach (var child in current.Children)
            {
                if (!closed.Contains(child))
                    open.Add(child);
            }
        }

        return new SearchResult(null, int.MaxValue, int.MaxValue);
    }
}