using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapCameraMovement : MonoBehaviour {
    public float speed;
	// Use this for initialization


    // Update is called once per frame
    void Update() {

        if (Input.GetMouseButton(0))
        {
            float mouseX = -Input.GetAxis("Mouse X");
            float mouseY = -Input.GetAxis("Mouse Y");
            float cameraX = transform.position.x+ mouseX;
            float cameraY = transform.position.y+ mouseY;
            if ((cameraX>=-15&&cameraX<=40) &&(cameraY>=-10&&cameraY<=45))
            {
                Vector3 endPosition = new Vector3(mouseX, mouseY);
                transform.position = Vector3.Lerp(transform.position, transform.position + endPosition, 0.5f);
            }
            //var step = speed * Time.deltaTime;
            
            
        }
	}
    public void reset_Position()
    {
        Vector3 reset = new Vector3(8, 8, -15);
        transform.position = reset;
    }
}
