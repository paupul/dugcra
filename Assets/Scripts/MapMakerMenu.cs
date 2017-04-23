using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

public class MapMakerMenu : MonoBehaviour
{
    List<string> directories;
    public GameObject customLevelContent;
    public GameObject levelPrefab;
    public string saveLocation = "CustomLevels/";
    public LevelManager levelManager;

    public InputField newLevelName;

    public Toggle levelListToggle;


    private void Awake()
    {
        SaveAndLoadManager.gridSaveFolder = "CustomLevels";
        LoadLevels();
        levelManager = GetComponent<LevelManager>();
        levelManager.loaded = true;
    }

    public void LoadLevels()
    {
        LevelManager.levels.Clear();
        if (!Directory.Exists(saveLocation))
        {
            Directory.CreateDirectory(saveLocation);
        }
        directories = new List<string>(Directory.GetDirectories(saveLocation));
        directories.Sort(new NaturalStringComparer());
        foreach (var item in directories)
        {
            GameObject level = Instantiate(levelPrefab, customLevelContent.transform);
            level.transform.GetChild(0).GetComponent<Text>().text = item;
            string levelName = item.Substring(saveLocation.Length);
            LevelManager.levels.Add(levelName);
            level.GetComponent<Button>().onClick.AddListener(delegate { StartMapMaker(levelName); });
        }
    }

    public void CreateLevel()
    {
        Regex r = new Regex(@"^[a-zA-Z0-9]*$");
        if (!r.IsMatch(newLevelName.text))
        {
            return;
        }
        string newLevel = saveLocation + newLevelName.text + "/";
        if (!Directory.Exists(newLevel))
        {
            Directory.CreateDirectory(newLevel);
            directories.Add(newLevel);

            GameObject level = Instantiate(levelPrefab, customLevelContent.transform);
            level.transform.GetChild(0).GetComponent<Text>().text = newLevel;

            string levelName = newLevel.Substring(saveLocation.Length);
            LevelManager.levels.Add(levelName);
            level.GetComponent<Button>().onClick.AddListener(delegate { StartMapMaker(levelName); });            
        }
    }

    public void StartMapMaker(string levelName)
    {
        levelListToggle.isOn = false;
        LevelManager.levelName = levelName;
        levelManager.LoadMapMakerWorld();
    }

    public void SaveLevel()
    {
        levelManager.SaveMapMakerWorld();
    }
}
