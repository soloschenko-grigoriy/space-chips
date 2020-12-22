using System;
using System.Collections.Generic;

public class AStarSearch {
    public Dictionary<HexCell, HexCell> cameFrom = new Dictionary<HexCell, HexCell>();
    public Dictionary<HexCell, int> costSoFar = new Dictionary<HexCell, int>();

    static public int Heuristic(HexCoordinates a, HexCoordinates b) => (Math.Abs(a.X - b.X) + Math.Abs(a.Y - b.Y) + Math.Abs(a.Z - b.Z));

    public AStarSearch(HexCell start, HexCell goal) {
        var frontier = new PriorityQueue<HexCell>();
        frontier.Enqueue(start, 0);

        cameFrom[start] = start;
        costSoFar[start] = 0;

        while (frontier.Count > 0) {
            var current = frontier.Dequeue();

            if (current.Equals(goal)) {
                break;
            }

            foreach (var next in current.Neighbors) {
                if (!next) {
                    continue;
                }

                int newCost = costSoFar[current] + current.Cost(next);
                if (!costSoFar.ContainsKey(next) || newCost < costSoFar[next]) {
                    costSoFar[next] = newCost;
                    int priority = newCost + Heuristic(next.coordinates, goal.coordinates);
                    frontier.Enqueue(next, priority);
                    cameFrom[next] = current;
                }
            }
        }
    }
}
