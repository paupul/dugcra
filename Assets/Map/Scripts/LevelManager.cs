using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{

    public GameObject worldPrefab;
    public GameObject fogPrefab;
    public GameObject playerPrefab;
    public string levelName;
    public bool isRandom;

    void Start()
    {

        GameObject world = Instantiate(worldPrefab);
        GameObject fog = Instantiate(fogPrefab);
        GameObject player = Instantiate(playerPrefab);

        Player playerComp = player.GetComponent<Player>();
        playerComp.world = world.GetComponent<World>();
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
        //playerComp.map = Camera.main.gameObject;

        Camera.main.GetComponent<SmoothCamera>().Lookat = player.transform;
    }
}
