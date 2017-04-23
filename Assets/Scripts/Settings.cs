using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Settings : MonoBehaviour {
    public Toggle fullScreen;
    public Dropdown resolutionsDropDown;
    public Resolution[] resolutions;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    void OnEnable()
    {
        resolutions = Screen.resolutions;
    }
}
