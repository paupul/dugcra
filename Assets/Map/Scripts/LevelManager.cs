using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static List<string> levels = new List<string>();

    public GameObject itemPool;
    public GameObject worldPrefab;
    public GameObject fogPrefab;
    public GameObject playerPrefab;
    public static string levelName;
    public static bool isRandom;
    public static int levelIndex;
    public List<GameObject> spawnables;
    public bool loaded = false;

    private static GameObject spawnedWorld;
    private static World spawnedWorldComp;

    public static World SpawnedWorldComp
    {
        get
        {
            return spawnedWorldComp;
        }

        set
        {
            spawnedWorldComp = value;
        }
    }

    void Awake()
    {
        if (loaded)
        {
            return;
        }
        SaveAndLoadManager.gridSaveFolder = "Levels";
        levelIndex = levels.FindIndex(o => o.Equals(levelName));

        GameObject world = Instantiate(worldPrefab);
        GameObject fog = Instantiate(fogPrefab);
        GameObject player = Instantiate(playerPrefab);

        Player playerComp = player.GetComponent<Player>();
        playerComp.world = world.GetComponent<World>();
        playerComp.world.levelManager = this;
        if (isRandom)
        {
            playerComp.world.isRandom = isRandom;
        }
        else
        {
            playerComp.world.isRandom = isRandom;
            playerComp.world.worldName = levelName;
        }
        playerComp.fogWorld = fog.GetComponent<World>();
        playerComp.fogWorld.isFogGenerator = true;
        playerComp.fogWorld.levelManager = this;
        //playerComp.map = Camera.main.gameObject;

        Camera.main.GetComponent<SmoothCamera>().Lookat = player.transform;

        loaded = true;
    }

    public void LoadMapMakerWorld()
    {
        Destroy(spawnedWorld);
        Despawn();

        spawnedWorld = Instantiate(worldPrefab);
        SpawnedWorldComp = spawnedWorld.GetComponent<World>();
        SpawnedWorldComp.levelManager = this;
        SpawnedWorldComp.isRandom = false;
        SpawnedWorldComp.worldName = levelName;
    }

    public void SaveMapMakerWorld()
    {
        if (spawnedWorld == null)
        {
            return;
        }
        foreach (var item in SpawnedWorldComp.grids)
        {
            item.Value.save = true;
        }

    }

    public void Despawn()
    {
        foreach (Transform item in itemPool.transform)
        {
            Destroy(item.gameObject);
        }
    }

    public void Spawn(Grid grid)
    {
        GameObject item;
        for (int x = 0; x < Grid.gridSize; x++)
        {
            for (int y = 0; y < Grid.gridSize; y++)
            {
                if (!grid.tiles[x, y].itemSpawned)
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
                        case GridTile.ContainedObject.StartingPoint:
                            item = Instantiate(spawnables[5], new Vector3(x - grid.pos.x + 0.5f, y - grid.pos.y + 0.5f, -0.5f), Quaternion.identity);
                            item.transform.SetParent(itemPool.transform);
                            break;
                        default:
                            break;
                    }
                    grid.tiles[x, y].itemSpawned = true;
                }
            }
        }
    }
}
