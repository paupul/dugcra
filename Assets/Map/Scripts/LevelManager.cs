using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{    
    public GameObject itemPool;
    public GameObject worldPrefab;
    public GameObject fogPrefab;
    public GameObject playerPrefab;
    public string levelName;
    public bool isRandom;
    public List<GameObject> spawnables;

    void Awake()
    {
        GameObject world = Instantiate(worldPrefab);
        GameObject fog = Instantiate(fogPrefab);
        GameObject player = Instantiate(playerPrefab);

        Player playerComp = player.GetComponent<Player>();
        playerComp.world = world.GetComponent<World>();
        playerComp.world.levelManager = this;
        if (isRandom)
        {
            playerComp.world.isRandom = true;
        }
        else
        {
            playerComp.world.isRandom = false;
            playerComp.world.worldName = levelName;
        }
        playerComp.fogWorld = fog.GetComponent<World>();
        playerComp.fogWorld.isFogGenerator = true;
        playerComp.fogWorld.levelManager = this;
        //playerComp.map = Camera.main.gameObject;

        Camera.main.GetComponent<SmoothCamera>().Lookat = player.transform;
    }

    public void Spawn(Grid grid)
    {
        GameObject item;
        for (int x = 0; x < Grid.gridSize; x++)
        {
            for (int y = 0; y < Grid.gridSize; y++)
            {
                switch (grid.tiles[x, y].containedObject)
                {
                    case GridTile.ContainedObject.Ladder:
                        item = Instantiate(spawnables[0], new Vector3(x - grid.pos.x + 0.5f, y - grid.pos.y + 0.5f, -0.5f), Quaternion.identity);
                        item.transform.SetParent(itemPool.transform);
                        break;
                    case GridTile.ContainedObject.Pit:
                        item = Instantiate(spawnables[1], new Vector3(x - grid.pos.x + 0.5f, y - grid.pos.y + 0.5f, -0.5f), Quaternion.identity);
                        item.transform.SetParent(itemPool.transform);
                        break;
                    case GridTile.ContainedObject.Spear:
                        item = Instantiate(spawnables[2], new Vector3(x - grid.pos.x + 0.5f, y - grid.pos.y + 0.5f, -0.5f), Quaternion.identity);
                        item.transform.SetParent(itemPool.transform);
                        break;
                    case GridTile.ContainedObject.Enemy:
                        item = Instantiate(spawnables[3], new Vector3(x - grid.pos.x + 0.5f, y - grid.pos.y + 0.5f, -0.5f), Quaternion.identity);
                        item.transform.SetParent(itemPool.transform);
                        break;
                    case GridTile.ContainedObject.Chest:
                        item = Instantiate(spawnables[4], new Vector3(x - grid.pos.x + 0.5f, y - grid.pos.y + 0.5f, -0.5f), Quaternion.identity);
                        item.transform.SetParent(itemPool.transform);
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
