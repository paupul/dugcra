using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using UnityEngine;
using UnityEngine.UI;

public class FillLevels : MonoBehaviour
{
    List<string> directories;
    public GameObject content;
    public GameObject levelPrefab;

    private void Awake()
    {
        LevelManager.levels.Clear();
        if (!Directory.Exists("Levels/"))
        {
            Directory.CreateDirectory("Levels/");
        }
        directories = new List<string>(Directory.GetDirectories("Levels/"));
        directories.Sort(new NaturalStringComparer());
        foreach (var item in directories)
        {
            string levelName = item.Substring(7);
            LevelManager.levels.Add(levelName);
        }
    }

    private void OnEnable()
    {        
        foreach (var item in directories)
        {
            string levelName = item.Substring(7);
            GameObject level = Instantiate(levelPrefab, content.transform);
            level.transform.GetChild(0).GetComponent<Text>().text = levelName.Replace('_', ' ');
            level.GetComponent<Button>().onClick.AddListener(delegate { Change_scene.ChangeToLevel(1, levelName, false); });
        }
    }

    private void OnDisable()
    {
        foreach (Transform item in content.transform)
        {
            Destroy(item.gameObject);
        }
    }
}

[SuppressUnmanagedCodeSecurity]
internal static class SafeNativeMethods
{
    [DllImport("shlwapi.dll", CharSet = CharSet.Unicode)]
    public static extern int StrCmpLogicalW(string psz1, string psz2);
}

public sealed class NaturalStringComparer : IComparer<string>
{
    public int Compare(string a, string b)
    {
        return SafeNativeMethods.StrCmpLogicalW(a, b);
    }
}

public sealed class NaturalFileInfoNameComparer : IComparer<FileInfo>
{
    public int Compare(FileInfo a, FileInfo b)
    {
        return SafeNativeMethods.StrCmpLogicalW(a.Name, b.Name);
    }
}
