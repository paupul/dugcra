using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour {

    Camera cam;

	// Use this for initialization
	void Start () {
        cam = GetComponent<Camera>();
	}
	
	// Update is called once per frame
	void Update () {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        horizontal = horizontal * 5 * Time.deltaTime;
        vertical = vertical * 5 * Time.deltaTime;

        Vector3 position = new Vector3(transform.position.x + horizontal, transform.position.y + vertical, transform.position.z);
        
        transform.position = position;

        if (Input.GetMouseButtonDown(1))
        {
            cam.orthographicSize = 5;
        }
        if (Input.GetMouseButtonUp(1))
        {
            cam.orthographicSize = 8;
        }
    }
}
