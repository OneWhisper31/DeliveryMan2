using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding
{

    public List<Node> ThetaStar(Node startingNode, Node goalNode,bool skipStartingNode=false)
    {
        if (startingNode == null || goalNode == null) return new List<Node>();

        List<Node> path = AStar(startingNode, goalNode);

        if (skipStartingNode)//desde los npcs y leader se hace un ternario diciendo que si esta muy cerca del nodo inicial, que se borre el nodo inicial
            path.RemoveAt(0);

        int current = 0;
        while (current + 2 < path.Count)
        {
            if (Physics2DExtension.InLineOfSight(path[current].transform.position, path[current + 2].transform.position,UICounter.intance.wallMask))
                path.RemoveAt(current + 1);
            else
                current++;
        }

        return path;
    }

    public List<Node> AStar(Node startingNode, Node goalNode)
    {
        if (startingNode == null || goalNode == null) return new List<Node>();

        PriorityQueueSimplify<Node> frontier = new PriorityQueueSimplify<Node>();
        frontier.Enqueue(startingNode, 0);

        Dictionary<Node, Node> cameFrom = new Dictionary<Node, Node>();
        cameFrom.Add(startingNode, null);

        Dictionary<Node, int> costSoFar = new Dictionary<Node, int>();
        costSoFar.Add(startingNode, 0);

        while (frontier.Count > 0)
        {
            Node current = frontier.Dequeue();
            if (current.locked == false)
            {

                if (current == goalNode)
                {
                    List<Node> path = new List<Node>();
                    while (current != startingNode)
                    {
                        path.Add(current);
                        current = cameFrom[current];
                    }
                    path.Add(startingNode);
                    path.Reverse();

                    return path;
                }

                foreach (Node next in current.GetNeighbors())
                {


                    int newCost = costSoFar[current] + next.cost;
                    float priority = newCost + Vector3.Distance(next.transform.position, goalNode.transform.position);
                    if (!costSoFar.ContainsKey(next))
                    {
                        frontier.Enqueue(next, priority);
                        cameFrom.Add(next, current);
                        costSoFar.Add(next, newCost);
                    }
                    else if (newCost < costSoFar[next])
                    {
                        frontier.Enqueue(next, priority);
                        cameFrom[next] = current;
                        costSoFar[next] = newCost;
                    }
                }
            }
        }
        return new List<Node>();
    }
}

