using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class Player : MonoBehaviour
{

    public float turnDelay;
    //public float speed;
    private Rigidbody2D rb2D;
    private bool move;
    public LayerMask blockingLayer;
    public LayerMask layer;
    public AudioClip tempAudio;
    private new AudioSource audio;

    protected void Start()
    {
        move = true;
        rb2D = GetComponent<Rigidbody2D>();

        audio = GetComponent<AudioSource>();
        audio.clip = tempAudio;
    }

    void Update()
    {
        int horizontal = 0;
        int vertical = 0;
        horizontal = (int)Input.GetAxisRaw("Horizontal");
        vertical = (int)Input.GetAxisRaw("Vertical");
        if (horizontal != 0)
        {
            vertical = 0;
        }
        if ((horizontal != 0 || vertical != 0)&&move)
        {
            move = false;
            if(AttemptMove(horizontal, vertical))
            {
                print("Moved");
            }
        }

    }

    public bool AttemptMove(int xDir, int yDir)
    {
        Vector2 end = rb2D.position + new Vector2(xDir, yDir);

        RaycastHit2D[] hit;
        RaycastHit2D[] notHit;
        hit = Physics2D.LinecastAll(rb2D.position, end, blockingLayer);
        notHit = Physics2D.LinecastAll(rb2D.position, end, layer);

        if (hit.Length==0)
        {
            for (int i = 0; i < notHit.Length; i++)
            {
                GameObject.Find(notHit[i].transform.name).GetComponent<SpriteRenderer>().enabled = false;
                print(notHit[i].transform.name);
            }
            StartCoroutine(Move(end));
            return true;
        }
        else
        {
            for(int i = 0; i < hit.Length; i++)
            {
                GameObject.Find(hit[i].transform.name).GetComponent<SpriteRenderer>().enabled = false;
                print(hit[i].transform.name);
            }
            move = true;
        }
        return false;

    }
    IEnumerator Move(Vector2 end)
    {
        yield return new WaitForSeconds(turnDelay);
        rb2D.MovePosition(end);
        audio.Play();
        move = true;

        //float sqrRemainingDistance = (rb2D.position - end).sqrMagnitude;

        //while (sqrRemainingDistance > float.Epsilon)
        //{
        //    Vector3 newPostion = Vector3.MoveTowards(rb2D.position, end, speed * Time.deltaTime);

        //    rb2D.MovePosition(newPostion);

        //    sqrRemainingDistance = (rb2D.position - end).sqrMagnitude;
        //    yield return null;
        //}
    }

}
