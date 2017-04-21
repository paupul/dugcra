using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class FillLevels : MonoBehaviour {

    public GameObject content;

    private void OnEnable()
    {
        if (!Directory.Exists("Levels/"))
        {
            Directory.CreateDirectory("Levels/");
        }
        var files = Directory.GetDirectories("Levels/");
    }

    private void OnDisable()
    {

    }
}
