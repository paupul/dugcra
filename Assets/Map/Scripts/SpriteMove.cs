using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteMove : MonoBehaviour {

    Rigidbody2D rigid;
    Vector3 mousePos;
    Vector3 mouseDir;

	// Use this for initialization
	void Start () {
        rigid = GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;
        mouseDir = mousePos - transform.position;
        mouseDir = mouseDir.normalized;

        if (Input.GetMouseButton(0))
        {
            rigid.AddForce((mouseDir) * 100);
        }
    }
}
