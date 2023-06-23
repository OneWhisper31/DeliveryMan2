using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeGrid : MonoBehaviour
{
    [SerializeField] Node prefab;
    [SerializeField] LayerMask wallMask;

    [SerializeField] Node[] _nodes;//en orden horizontal

    Node[,] _grid;

    public int width;
    public int height;

    private void Awake()
    {
        CreateGrid();
    }

    void CreateGrid()
    {
        _grid = new Node[width, height];
        _nodes = new Node[width * height];

        for (int x = 0; x < width; x++)
        {
            int i = 0;
            for (int y = 0; y < height; y++)
            {
                i = x  + y * width;
                if (i < _nodes.Length)
                {
                    _grid[x, y] = Instantiate(prefab, new Vector2(transform.position.x + y * 6, transform.position.y + x * 6), transform.rotation, transform);
                    _grid[x, y].Initialize(this, new Vector2Int(x, y));
                    _grid[x, y].name = "Node-" + i;
                    _nodes[i]= _grid[x, y];
                }
            }
        }
    }

    public List<Node> GetNeighborsBasedOnPosition(int x, int y)
    {

        List<Node> neighbors = new List<Node>();

        Vector3 mainPos = Vector3.zero;

        if (x < width || y < height)
        {
            mainPos = _grid[x , y].transform.position;
        }

        if (x + 1 < width)
        {
            if (_grid[x +1 , y] != null)
            {
                if (_grid[x + 1, y].locked == false)
                {
                    Vector3 endPos1 = _grid[x + 1, y].transform.position;

                    if (!Physics.Raycast(mainPos, endPos1 - mainPos, (endPos1 - mainPos).magnitude, wallMask))
                        neighbors.Add(_grid[x + 1, y]);
                }
            }
        }

        if (x - 1 >= 0)
        {
            if (_grid[x - 1 ,y] != null)
            {
                if (_grid[x - 1 , y].locked == false)
                {
                    Vector3 endPos2 = _grid[x - 1, y].transform.position;

                    if (!Physics.Raycast(mainPos, endPos2 - mainPos, (endPos2 - mainPos).magnitude, wallMask))
                        neighbors.Add(_grid[x - 1, y]);
                }
            }
        }

        if (y + 1 < height)
        {
            if (_grid[x,y+1] != null)
            {
                if (_grid[x , y + 1].locked == false)
                {
                    Vector3 endPos3 = _grid[x, y + 1].transform.position;

                    if (!Physics.Raycast(mainPos, endPos3 - mainPos, (endPos3 - mainPos).magnitude, wallMask))
                        neighbors.Add(_grid[x, y + 1]);
                }
            }
        }

        if (y - 1 >= 0)
        {
            if (_grid[x, y - 1] != null)
            {
                if (_grid[x, y - 1].locked == false)
                {
                    Vector3 endPos4 = _grid[x, y - 1].transform.position;

                    if (!Physics.Raycast(mainPos, endPos4 - mainPos, (endPos4 - mainPos).magnitude, wallMask))
                        neighbors.Add(_grid[x, y - 1]);
                }
            }
        }

        return neighbors;
    }
    public Node GetStartingNode(Vector3 pos)//devuelve el nodo mas cercano segun la pos
    {
        float closestMagnitude = Mathf.Infinity;
        Node closestNode = null;

        foreach (Node node in _nodes)
        {
            if (node == null||node.locked==true) continue;

            float magnitude = Vector3.Distance(pos, node.transform.position);
            if (magnitude < closestMagnitude)
            {
                closestMagnitude = magnitude;
                closestNode = node;

            }
        }

        return closestNode;
    }
}
