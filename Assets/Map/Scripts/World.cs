using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class World : MonoBehaviour {

    public Dictionary<WorldPos, Grid> grids = new Dictionary<WorldPos, Grid>();
    public GameObject gridPrefab;

    public string worldName = "World";

    public void CreateGrid(int x, int y)
    {
        WorldPos worldPos = new WorldPos(x, y);

        GameObject newGridObject = Instantiate(gridPrefab, new Vector3(x, y), Quaternion.Euler(Vector3.zero)) as GameObject;

        Grid newGrid = newGridObject.GetComponent<Grid>();

        GridGenerator gen = new GridGenerator();

        newGrid.pos = worldPos;
        newGrid.world = this;
        if (!SaveAndLoadManager.LoadGrid(newGrid))
        {
            newGrid = gen.GridGen(newGrid);
        }

        grids.Add(worldPos, newGrid);        
    }

    public Grid GetGrid(int x, int y)
    {
        WorldPos pos = new WorldPos();
        float multiple = Grid.gridSize;
        pos.x = Mathf.FloorToInt(x / multiple) * Grid.gridSize;
        pos.y = Mathf.FloorToInt(y / multiple) * Grid.gridSize;

        Grid containerGrid= null;

        grids.TryGetValue(pos, out containerGrid);

        return containerGrid;
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
