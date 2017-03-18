using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerItems : MonoBehaviour {

    private int spear;
    private int ladder;
    private int points;


    void Start () {
        spear = 0;
        ladder = 0;
        points = 0;
    }
	
	void Update () {
		
	}

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Spear")
        {
            spear++;
            points += 5;
            print("Spear count:" + spear);

            other.gameObject.SetActive(false);
        }
        else if (other.tag == "Ladder")
        {
            ladder++;
            points += 5;
            print("Ladder count:" + ladder);

            other.gameObject.SetActive(false);
        }

        else if (other.tag == "Monster")
        {
            if (spear <= 0)
            {
                print("Game over...:");
                SceneManager.LoadScene(0);
            }
            else
            {
				spear--;
                points += 10;
                print("Spear count:" + spear);
                other.gameObject.SetActive(false);
            }
        }
        else if (other.tag == "Pit")
        {
            if (ladder <= 0)
            {
                print("Game over...:");
                SceneManager.LoadScene(0);
            }
            else
            {
                ladder--;
                points += 10;
                print("Ladder count:" + ladder);
                other.gameObject.SetActive(false);
            }
        }
        else if (other.tag == "Chest")
        {
            points += 50;
            print("Points count:" + points);
            SceneManager.LoadScene(1);
        }
    }
}
