﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class World : MonoBehaviour
{

    public Dictionary<WorldPos, Grid> grids = new Dictionary<WorldPos, Grid>();
    public GameObject gridPrefab;
    public bool isFogGenerator = false;
    public bool isRandom = false;

    public WorldPos startingPoint;
    public WorldPos startingGrid;
    public bool isPointGenerated = false;
    public LevelManager levelManager;

    public string worldName = "World";

    public void CreateGrid(int x, int y)
    {
        WorldPos worldPos = new WorldPos(x, y);

        GameObject newGridObject = Instantiate(gridPrefab, new Vector3(x, y), Quaternion.Euler(Vector3.zero)) as GameObject;
        if (isFogGenerator)
        {
            newGridObject.transform.position = new Vector3(x, y, -1);
            newGridObject.layer = 8;
        }
        else
        {
            newGridObject.layer = 9;
        }
        newGridObject.transform.SetParent(gameObject.transform);
        Grid newGrid = newGridObject.GetComponent<Grid>();

        GridGenerator gen = new GridGenerator();

        newGrid.pos = worldPos;
        newGrid.world = this;
        if (isFogGenerator || isRandom || !SaveAndLoadManager.LoadGrid(newGrid))
        {
            newGrid = gen.GridGen(newGrid, out isPointGenerated, out startingGrid, out startingPoint, isFogGenerator);
            newGrid = gen.GridConnectionGen(newGrid);
            newGrid = gen.GridItemGen(newGrid);
            if (!isFogGenerator)
            {
                newGrid.SetTile(startingPoint.x, startingPoint.y, new GridTile(GridTile.TileTypes.Ground, GridTile.ContainedObject.StartingPoint));
            }
        }

        //newGrid = gen.GridTileGen(newGrid, startingPoint.x, startingPoint.y, GridTile.TileTypes.Empty);

        if (!isFogGenerator)
        {
            bool flag = false;
            for (int xx = 0; xx < Grid.gridSize; xx++)
            {
                for (int yy = 0; yy < Grid.gridSize; yy++)
                {
                    if (newGrid.tiles[xx, yy].containedObject == GridTile.ContainedObject.StartingPoint)
                    {
                        startingPoint = new WorldPos(xx, yy);
                        startingGrid = new WorldPos(xx, yy);
                        flag = true;
                        isPointGenerated = true;
                        continue;                        
                    }
                }
                if (flag)
                {
                    continue;
                }
            }
            levelManager.Spawn(newGrid);
        }

        grids.Add(worldPos, newGrid);
    }

    public Grid GetGrid(int x, int y)
    {
        WorldPos pos = new WorldPos();
        float multiple = Grid.gridSize;
        pos.x = Mathf.FloorToInt(x / multiple) * Grid.gridSize;
        pos.y = Mathf.FloorToInt(y / multiple) * Grid.gridSize;

        Grid containerGrid = null;

        grids.TryGetValue(pos, out containerGrid);

        return containerGrid;
    }

    public Grid GetGrid(Vector2 position)
    {
        WorldPos pos = new WorldPos();
        float multiple = Grid.gridSize;
        pos.x = Mathf.FloorToInt(position.x / multiple) * Grid.gridSize;
        pos.y = Mathf.FloorToInt(position.y / multiple) * Grid.gridSize;

        Grid containerGrid = null;

        grids.TryGetValue(pos, out containerGrid);

        return containerGrid;
    }

    public void SetTile(int x, int y, GridTile tile)
    {
        Grid grid = GetGrid(x, y);

        if (grid != null)
        {
            grid.SetTile(x - grid.pos.x, y - grid.pos.y, tile);
            grid.update = true;
        }
    }

    public GridTile GetTile(int x, int y)
    {
        Grid containerGrid = GetGrid(x, y);

        if (containerGrid != null)
        {
            GridTile block = containerGrid.GetTile(
                x - containerGrid.pos.x,
                y - containerGrid.pos.y);

            return block;
        }
        else
        {
            return new GridTile(GridTile.TileTypes.Empty);
        }

    }

    internal void DestroyGrid(int x, int y)
    {
        Grid grid = null;
        if (grids.TryGetValue(new WorldPos(x, y), out grid))
        {
            Destroy(grid.gameObject);
            grids.Remove(new WorldPos(x, y));
        }
    }
}
