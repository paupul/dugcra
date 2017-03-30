using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class GridGenerator
{
    List<List<Node>> maze = new List<List<Node>>();


    public Grid GridGen(Grid grid, out bool isPointGenerated, out WorldPos startingPoint, bool isFogGenerator = false)
    {
        isPointGenerated = false;
        startingPoint = new WorldPos(0, 0);

        if (isFogGenerator)
        {
            for (int x = grid.pos.x; x < grid.pos.x + Grid.gridSize; x++)
            {
                for (int y = grid.pos.y; y < grid.pos.y + Grid.gridSize; y++)
                {
                    grid = GridFogGen(grid, x, y);
                }
            }
        }
        else
        {
            MazeGen2();
            for (int x = grid.pos.x; x < grid.pos.x + Grid.gridSize; x++)
            {
                for (int y = grid.pos.y; y < grid.pos.y + Grid.gridSize; y++)
                {
                    if (maze[x][y].isWall)
                    {
                        grid = GridTileGen(grid, x, y, GridTile.TileTypes.Wall);
                    }
                    else grid = GridTileGen(grid, x, y, GridTile.TileTypes.Ground);
                    if (maze[x][y].isStart)
                    {
                        startingPoint = new WorldPos(x, y);
                        isPointGenerated = true;
                    }
                }
            }
        }
        return grid;
    }

    public Grid GridTileGen(Grid grid, int x, int y, GridTile.TileTypes type)
    {
        grid.SetTile(x - grid.pos.x, y - grid.pos.y, new GridTile(type));
        return grid;
    }

    private Grid GridTileGen(Grid grid, int x, int y)
    {
        if ((
            (x == grid.pos.x || x == grid.pos.x + Grid.gridSize - 1) && (y >= grid.pos.y && y <= grid.pos.y + Grid.gridSize)
            ) || (
            (y == grid.pos.y || y == grid.pos.y + Grid.gridSize - 1) && x >= grid.pos.x && x <= grid.pos.x + Grid.gridSize
            ))
        {
            grid.SetTile(x - grid.pos.x, y - grid.pos.y, new GridTile(GridTile.TileTypes.Wall));
        }
        else
        {
            grid.SetTile(x - grid.pos.x, y - grid.pos.y, new GridTile(GridTile.TileTypes.Ground));
        }
        return grid;
    }

    private Grid GridFogGen(Grid grid, int x, int y)
    {
        grid.SetTile(x - grid.pos.x, y - grid.pos.y, new GridTile(GridTile.TileTypes.Fog));
        return grid;
    }

    private void MazeGen2()
    {
        #region
        maze.Clear();
        for (int x = 0; x < Grid.gridSize; x++)
        {
            List<Node> xNode = new List<Node>();
            for (int y = 0; y < Grid.gridSize; y++)
            {
                Node yNode = new Node();
                yNode.isWall = true;
                yNode.pos = new Vector2(x, y);
                if (y > 0)
                {
                    yNode.up = xNode[y - 1];
                    xNode[y - 1].down = yNode;
                }
                if (x > 0)
                {
                    yNode.left = maze[x - 1][y];
                    maze[x - 1][y].right = yNode;
                }
                xNode.Add(yNode);
            }
            maze.Add(xNode);
        }
        #endregion

        System.Random r = new System.Random();

        Node current = maze[(int)(r.NextDouble() * Grid.gridSize * 10) % Grid.gridSize][(int)(r.NextDouble() * Grid.gridSize * 10) % Grid.gridSize];
        current.isStart = true;
        current.isWall = false;

        List<Node> validNodes = new List<Node>();
        int count = 0;

        for (int i = 0; i < 4; i++)
        {
            if (current[i] != null)
            {
                count++;
            }
        }

        int startingBranches = r.Next(1, count + 1);

        #region
        if (startingBranches == count)
        {
            for (int i = 0; i < count; i++)
            {
                if (current[i] != null)
                {
                    current[i].isWall = false;
                    validNodes.Add(current[i]);
                }
            }
        }
        else if (startingBranches == 2)
        {
            int rand = r.Next(0, count);
            int rand2 = r.Next(0, count);
            while (rand2 == rand)
                rand2 = r.Next(0, count);
            for (int i = 0; i < count; i++)
            {
                if (current[i] != null && (current[i] != current[rand] || current[i] != current[rand2]))
                {
                    current[i].isWall = false;
                    validNodes.Add(current[i]);
                }
            }
        }
        else
        {
            int rand = r.Next(0, count);
            for (int i = 0; i < count; i++)
            {
                if (current[i] != null && current[i] != current[rand] && startingBranches == 3)
                {
                    current[i].isWall = false;
                    validNodes.Add(current[i]);
                }
                else if (current[i] != null && current[i] == current[rand] && startingBranches == 1)
                {
                    current[i].isWall = false;
                    validNodes.Add(current[i]);
                }
            }
        }
        

        while (validNodes.Count > 0)
        {
            int rand_node = 0;

            List<Node> readyNeighbours = new List<Node>();

            if (validNodes[rand_node][0] != null && validNodes[rand_node][0].isWall 
                && validNodes[rand_node][0].left != null && validNodes[rand_node][0].right != null
                && validNodes[rand_node][0].left.isWall && validNodes[rand_node][0].right.isWall
                && validNodes[rand_node][0].up != null && validNodes[rand_node][0].up.isWall)
            {
                readyNeighbours.Add(validNodes[rand_node][0]);
            }
            if (validNodes[rand_node][1] != null && validNodes[rand_node][1].isWall 
                && validNodes[rand_node][1].up != null && validNodes[rand_node][1].down != null
                && validNodes[rand_node][1].up.isWall && validNodes[rand_node][1].down.isWall
                && validNodes[rand_node][1].right != null && validNodes[rand_node][1].right.isWall)
            {
                readyNeighbours.Add(validNodes[rand_node][1]);
            }
            if (validNodes[rand_node][2] != null && validNodes[rand_node][2].isWall
                && validNodes[rand_node][2].left != null && validNodes[rand_node][2].right != null
                && validNodes[rand_node][2].left.isWall && validNodes[rand_node][2].right.isWall
                && validNodes[rand_node][2].down != null && validNodes[rand_node][2].down.isWall)
            {
                readyNeighbours.Add(validNodes[rand_node][2]);
            }
            if (validNodes[rand_node][3] != null && validNodes[rand_node][3].isWall 
                && validNodes[rand_node][3].up != null && validNodes[rand_node][3].down != null
                && validNodes[rand_node][3].up.isWall && validNodes[rand_node][3].down.isWall 
                && validNodes[rand_node][3].left != null && validNodes[rand_node][3].left.isWall)
            {
                readyNeighbours.Add(validNodes[rand_node][3]);
            }


            if (readyNeighbours.Count == 1)
            {
                readyNeighbours[0].isWall = false;
                validNodes.Add(readyNeighbours[0]);
            }
            else if (readyNeighbours.Count == 2)
            {
                int rand = r.Next(0, 2);
                int rand2 = r.Next(0, 2);
                readyNeighbours[rand].isWall = false;
                validNodes.Add(readyNeighbours[rand]);
                if (rand != rand2)
                { 
                    readyNeighbours[rand2].isWall = false;
                    validNodes.Add(readyNeighbours[rand2]);
                }
            }
            else if (readyNeighbours.Count == 3)
            {
                int rand = r.Next(0, 3);
                int rand2 = r.Next(0, 3);
                int rand3 = r.Next(0, 3);
                readyNeighbours[rand].isWall = false;
                validNodes.Add(readyNeighbours[rand]);
                if (rand != rand2)
                {
                    readyNeighbours[rand2].isWall = false;
                    validNodes.Add(readyNeighbours[rand2]);
                }
                if (rand != rand3)
                {
                    readyNeighbours[rand3].isWall = false;
                    validNodes.Add(readyNeighbours[rand3]);
                }
            }

            validNodes.RemoveAt(0);
        }
        #endregion
    }

    /// <summary>
    /// Jono random algoritmas, nepabaigtas
    /// </summary>
    private void MazeGen_EllersAlgorithm()
    {
        #region
        maze.Clear();
        for (int x = 0; x < Grid.gridSize; x++)
        {
            List<Node> xNode = new List<Node>();
            for (int y = 0; y < Grid.gridSize; y++)
            {
                Node yNode = new Node();
                yNode.isWall = true;
                yNode.pos = new Vector2(x, y);
                if (y > 0)
                {
                    yNode.up = xNode[y - 1];
                    xNode[y - 1].down = yNode;
                }
                if (x > 0)
                {
                    yNode.left = maze[x - 1][y];
                    maze[x - 1][y].right = yNode;
                }
                xNode.Add(yNode);
            }
            maze.Add(xNode);
        }
        #endregion

        System.Random r = new System.Random();

        List<List<Node>> ellersMaze = new List<List<Node>>();

    }

    #region Node
    public class Node
    {
        public Vector2 pos;

        public Node up, right, down, left;

        public int wall = 0;

        public bool isWall, isVisited, isStart = false;

        public Node(bool isWall = false)
        {
            this.isWall = isWall;
        }

        public Node this[int index]
        {
            get
            {
                switch (index)
                {
                    case 0:
                        return up;
                    case 1:
                        return right;
                    case 2:
                        return down;
                    case 3:
                        return left;
                    default:
                        return null;
                }
            }
            set
            {
                switch (index)
                {
                    case 0:
                        up = value;
                        break;
                    case 1:
                        right = value;
                        break;
                    case 2:
                        down = value;
                        break;
                    case 3:
                        left = value;
                        break;
                    default:
                        break;
                }
            }
        }

        public override string ToString()
        {
            return isWall.ToString();
        }
    }
    #endregion
}
