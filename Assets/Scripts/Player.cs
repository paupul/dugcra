using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float turnDelay;
    private Rigidbody2D rb2D;
    private bool idle;
    public LayerMask fog;
    public LayerMask wall;
    public World fogWorld;

    protected void Start()
    {
        idle = true;
        rb2D = GetComponent<Rigidbody2D>();
    }

    int horizontal = 0;
    int vertical = 0;

    void Update()
    {
        horizontal = 0;
        vertical = 0;
        horizontal = (int)Input.GetAxisRaw("Horizontal");
        vertical = (int)Input.GetAxisRaw("Vertical");

        if (horizontal != 0)
        {
            vertical = 0;
        }
        if ((horizontal != 0 || vertical != 0) && idle)
        {
            idle = false;
            AttemptMove(horizontal, vertical);
        }
    }

    private void OnGUI()
    {
        GUI.Box(new Rect(new Vector2(0, 0), new Vector2(50, 20)), horizontal + " " + vertical);
    }

    public void AttemptMove(int xDir, int yDir)
    {
        Vector2 end = rb2D.position + new Vector2(xDir, yDir);
        RaycastHit2D fogDetect;
        RaycastHit2D walldetect;
        fogDetect = Physics2D.Linecast(rb2D.position, end);
        walldetect = Physics2D.Linecast(rb2D.position, end);        
        WorldPos pos = EditTerrain.GetBlockPos(fogDetect);
        //Debug.Log(pos.x + " " + pos.y);
        if (fogDetect)
        {
            fogWorld.SetTile(pos.x, pos.y, new GridTile(GridTile.TileTypes.Empty));
        }
        //if (fogDetect)
        //{
        //    GameObject.Find(fogDetect.transform.name).GetComponent<SpriteRenderer>().enabled = false;
        //    print(fogDetect.transform.name);
        //    GameObject.Find(fogDetect.transform.name).GetComponent<BoxCollider2D>().enabled = false;
        //    Destroy(GameObject.Find(fogDetect.transform.name).GetComponent<GameObject>());
        //}
        if (!walldetect)
        {
            rb2D.MovePosition(end);
        }
        StartCoroutine(Delay());
    }
    IEnumerator Delay()
    {
        yield return new WaitForSeconds(turnDelay);
        idle = true;
    }

}
