using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour {

    public GameObject worldPrefab;
    public GameObject fogPrefab;
    public GameObject playerPrefab;
    public string levelName;
    public bool isRandom;

	void Start () {
        if (isRandom)
        {
            GameObject world = Instantiate(worldPrefab);
            GameObject fog = Instantiate(fogPrefab);
            GameObject player = Instantiate(playerPrefab);
        }
	}	
}
