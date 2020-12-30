using System;
using System.Collections.Generic;
using UnityEngine;

public class AStarSearch {
    public Dictionary<HexCell, HexCell> CameFrom { get; private set; }
    public Dictionary<HexCell, int> CostSoFar { get; private set; }
    public HexCell Start { get; private set; }
    public HexCell End { get; private set; }

    static int Heuristic(HexCoordinates a, HexCoordinates b) => HexCoordinates.DistanceBetween(a, b);

    public AStarSearch(HexCell start, HexCell end) {
        Start = start;
        End = end;

        CameFrom = new Dictionary<HexCell, HexCell>();
        CostSoFar = new Dictionary<HexCell, int>();
    }

    public List<HexCell> Reconstruct(bool includeStart = false, bool reverse = true) {
        var current = End;
        var path = new List<HexCell>();
        while (current != Start) {
            path.Add(current);
            current = CameFrom[current];
        }

        if (includeStart) {
            path.Add(Start);
        }

        if (reverse) {
            path.Reverse();
        }


        return path;
    }

    public AStarSearch Search() {
        var frontier = new PriorityQueue<HexCell>();
        frontier.Enqueue(Start, 0);

        CameFrom[Start] = Start;
        CostSoFar[Start] = 0;

        while (frontier.Count > 0) {
            var current = frontier.Dequeue();

            if (current.Coordinates.Equals(End.Coordinates)) {
                break;
            }

            foreach (var next in current.Neighbors) {
                if (!next || next.OccupiedBy != null) {
                    continue;
                }

                int newCost = CostSoFar[current] + current.Cost(next);
                if (!CostSoFar.ContainsKey(next) || newCost < CostSoFar[next]) {
                    CostSoFar[next] = newCost;
                    int priority = newCost + Heuristic(next.Coordinates, End.Coordinates);
                    frontier.Enqueue(next, priority);
                    CameFrom[next] = current;
                }
            }
        }

        return this;
    }
}
