using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Change_scene : MonoBehaviour {

	public void ChangeToScene (string sceneToChangeTo) {
        Application.LoadLevel(sceneToChangeTo);
	}
}
