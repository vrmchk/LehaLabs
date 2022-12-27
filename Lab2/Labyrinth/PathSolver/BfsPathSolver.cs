using Labyrinth.Common;
using Labyrinth.Model;

namespace Labyrinth.PathSolver;
internal class BfsPathSolver : IPathSolver
{
    private int _iteration = 0;
    
    public SearchResult Solve(State state, bool printSteps = false)
    {
        return Bfs(state, printSteps);
    }

    private SearchResult Bfs(State state, bool printSteps)
    {
        var visited = new HashSet<State>();
        var queue = new HashSet<State>();
        queue.Add(state);
        while (queue.Count != 0)
        {
            var current = queue.First();
            queue.Remove(current);
            if (visited.Contains(current))
                continue;

            visited.Add(current);

            _iteration++;
            if (printSteps)
                current.PrintState();

            if (current.Distance == 1)
                return new SearchResult(current, _iteration, queue.Count + current.Generation);

            foreach (var child in current.Children)
            {
                if (!visited.Contains(child))
                    queue.Add(child);
            }
        }
        
        return new SearchResult(null, int.MaxValue ,int.MaxValue);
    }
}
