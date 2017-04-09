using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class GridGenerator
{
    List<List<Node>> maze = new List<List<Node>>();


    public Grid GridGen(Grid grid, out bool isPointGenerated, out WorldPos startingGrid, out WorldPos startingPoint, bool isFogGenerator = false)
    {
        isPointGenerated = false;
        startingPoint = new WorldPos(0, 0);
        startingGrid = new WorldPos(0, 0);

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
                    int xx = x;
                    int yy = y;
                    if (x < 0)
                    {
                        xx *= -1;
                        xx--;
                    }
                    if (y < 0)
                    {
                        yy *= -1;
                        yy--;
                    }
                    if (maze[xx % 16][yy % 16].isWall)
                    {
                        grid = GridTileGen(grid, x, y, GridTile.TileTypes.Wall);
                    }
                    else grid = GridTileGen(grid, x, y, GridTile.TileTypes.Ground);
                    if (maze[xx % 16][yy % 16].isStart)
                    {
                        startingPoint = new WorldPos(x - grid.pos.x, y - grid.pos.y);
                        startingGrid = new WorldPos(x, y);
                        isPointGenerated = true;
                    }
                }
            }
        }
        return grid;
    }

    /// <summary>
    /// Sujungimu versija v2
    /// </summary>
    /// <param name="grid">Grid</param>
    /// <returns>Grid</returns>
    public Grid GridConnectionGen(Grid grid)
    {
        Grid g;
        System.Random r = new System.Random();
        List<int> positions = new List<int>();
        if (g = grid.world.GetGrid(grid.pos.x, grid.pos.y - Grid.gridSize))
        {
            for (int x = grid.pos.x + 1; x < grid.pos.x + Grid.gridSize - 1; x++)
            {
                if (grid.GetTile(x - grid.pos.x, 1).type == GridTile.TileTypes.Ground
                    && g.GetTile(x - g.pos.x, 14).type == GridTile.TileTypes.Ground)
                {
                    positions.Add(x);
                }
            }

            for (int i = positions.Count; i > 0; i = positions.Count / 2)
            {
                int rand = r.Next(0, positions.Count);
                grid = GridTileGen(grid, positions[rand], 0 + grid.pos.y, GridTile.TileTypes.Ground);
                g = GridTileGen(g, positions[rand], 15 + g.pos.y, GridTile.TileTypes.Ground);
                g.update = true;
                positions.RemoveAt(rand);
            }
        }
        positions.Clear();
        if (g = grid.world.GetGrid(grid.pos.x - Grid.gridSize, grid.pos.y))
        {
            for (int y = grid.pos.y + 1; y < grid.pos.y + Grid.gridSize - 1; y++)
            {
                if (grid.GetTile(1, y - grid.pos.y).type == GridTile.TileTypes.Ground
                       && g.GetTile(14, y - g.pos.y).type == GridTile.TileTypes.Ground)
                {
                    positions.Add(y);
                }
            }
            for (int i = positions.Count; i > 0; i = positions.Count / 2)
            {
                int rand = r.Next(0, positions.Count);
                grid = GridTileGen(grid, 0 + grid.pos.x, positions[rand], GridTile.TileTypes.Ground);
                g = GridTileGen(g, 15 + g.pos.x, positions[rand], GridTile.TileTypes.Ground);
                g.update = true;
                positions.RemoveAt(rand);
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

    public Grid GridItemGen(Grid grid)
    {
        List<GridTile.ContainedObject> chosenObjects = new List<GridTile.ContainedObject>();
        GridToMaze(grid);
        for (int x = 0; x < Grid.gridSize; x++)
        {
            for (int y = 0; y < Grid.gridSize; y++)
            {
                if (maze[x][y].isWall == false && maze[x][y].up != null && maze[x][y].down != null && maze[x][y].right != null && maze[x][y].left != null)
                {
                    if (maze[x][y].up.isWall == true && maze[x][y].down.isWall == true && maze[x][y].right.isWall == true && maze[x][y].left.isWall == false)
                    {
                        switch (chosenObjects.LastOrDefault())
                        {
                            case GridTile.ContainedObject.Ladder:
                                chosenObjects.Add(GridTile.ContainedObject.Spear);
                                grid.tiles[x, y].containedObject = GridTile.ContainedObject.Spear;
                                grid.tiles[x - 1, y].containedObject = GridTile.ContainedObject.Pit;
                                break;
                            case GridTile.ContainedObject.Spear:
                                chosenObjects.Add(GridTile.ContainedObject.Chest);
                                grid.tiles[x, y].containedObject = GridTile.ContainedObject.Chest;
                                grid.tiles[x - 1, y].containedObject = GridTile.ContainedObject.Enemy;
                                break;
                            default:
                                chosenObjects.Add(GridTile.ContainedObject.Ladder);
                                grid.tiles[x, y].containedObject = GridTile.ContainedObject.Ladder;
                                break;
                        }
                    }
                    else if (maze[x][y].up.isWall == true && maze[x][y].down.isWall == true && maze[x][y].right.isWall == false && maze[x][y].left.isWall == true)
                    {
                        switch (chosenObjects.LastOrDefault())
                        {
                            case GridTile.ContainedObject.Ladder:
                                chosenObjects.Add(GridTile.ContainedObject.Spear);
                                grid.tiles[x, y].containedObject = GridTile.ContainedObject.Spear;
                                grid.tiles[x + 1, y].containedObject = GridTile.ContainedObject.Pit;
                                break;
                            case GridTile.ContainedObject.Spear:
                                chosenObjects.Add(GridTile.ContainedObject.Chest);
                                grid.tiles[x, y].containedObject = GridTile.ContainedObject.Chest;
                                grid.tiles[x + 1, y].containedObject = GridTile.ContainedObject.Enemy;
                                break;
                            default:
                                chosenObjects.Add(GridTile.ContainedObject.Ladder);
                                grid.tiles[x, y].containedObject = GridTile.ContainedObject.Ladder;
                                break;
                        }
                    }
                    else if (maze[x][y].up.isWall == true && maze[x][y].down.isWall == false && maze[x][y].right.isWall == true && maze[x][y].left.isWall == true)
                    {
                        switch (chosenObjects.LastOrDefault())
                        {
                            case GridTile.ContainedObject.Ladder:
                                chosenObjects.Add(GridTile.ContainedObject.Spear);
                                grid.tiles[x, y].containedObject = GridTile.ContainedObject.Spear;
                                grid.tiles[x, y + 1].containedObject = GridTile.ContainedObject.Pit;
                                break;
                            case GridTile.ContainedObject.Spear:
                                chosenObjects.Add(GridTile.ContainedObject.Chest);
                                grid.tiles[x, y].containedObject = GridTile.ContainedObject.Chest;
                                grid.tiles[x, y + 1].containedObject = GridTile.ContainedObject.Enemy;
                                break;
                            default:
                                chosenObjects.Add(GridTile.ContainedObject.Ladder);
                                grid.tiles[x, y].containedObject = GridTile.ContainedObject.Ladder;
                                break;
                        }
                    }
                    else if (maze[x][y].up.isWall == false && maze[x][y].down.isWall == true && maze[x][y].right.isWall == true && maze[x][y].left.isWall == true)
                    {
                        switch (chosenObjects.LastOrDefault())
                        {
                            case GridTile.ContainedObject.Ladder:
                                chosenObjects.Add(GridTile.ContainedObject.Spear);
                                grid.tiles[x, y].containedObject = GridTile.ContainedObject.Spear;
                                grid.tiles[x, y - 1].containedObject = GridTile.ContainedObject.Pit;
                                break;
                            case GridTile.ContainedObject.Spear:
                                chosenObjects.Add(GridTile.ContainedObject.Chest);
                                grid.tiles[x, y].containedObject = GridTile.ContainedObject.Chest;
                                grid.tiles[x, y - 1].containedObject = GridTile.ContainedObject.Enemy;
                                break;
                            default:
                                chosenObjects.Add(GridTile.ContainedObject.Ladder);
                                grid.tiles[x, y].containedObject = GridTile.ContainedObject.Ladder;
                                break;
                        }
                    }
                }
            }
        }
        return grid;
    }

    /// <summary>
    /// Konvertuoja i labirinto formata
    /// </summary>
    /// <param name="grid"></param>
    private void GridToMaze(Grid grid)
    {
        #region Maze setup
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

        for (int x = 0; x < Grid.gridSize; x++)
        {
            for (int y = 0; y < Grid.gridSize; y++)
            {
                if (grid.GetTile(x, y).type == GridTile.TileTypes.Wall)
                {
                    maze[x][y].isWall = true;
                }
                else if (grid.GetTile(x, y).type == GridTile.TileTypes.Ground)
                {
                    maze[x][y].isWall = false;
                }
            }
        }
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

        Node current = maze[r.Next(3, Grid.gridSize - 3)][r.Next(3, Grid.gridSize - 3)];
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

        int startingBranches = r.Next(3, count + 1);

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
