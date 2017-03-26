using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerItems : MonoBehaviour
{

    private int spear;
    private int ladder;
    private static int pointsForItem;
    private static int pointsForEnemy;
    private static int pointsForChest;
    public GameObject game_over;
    public ScoreManager scoreManager;
    private GameSounds gameSounds;

    void Start()
    {
        spear = 0;
        ladder = 0;
        pointsForItem = 5;
        pointsForEnemy = 10;
        pointsForChest = 50;
        gameSounds = GetComponent<GameSounds>();
    }

    void Update()
    {
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Spear")
        {
            spear++;
            scoreManager.AddPoints(pointsForItem);
            print("Spear count:" + spear);
            other.gameObject.SetActive(false);

        }
        else if (other.tag == "Ladder")
        {
            ladder++;
            scoreManager.AddPoints(pointsForItem);
            print("Ladder count:" + ladder);
            other.gameObject.SetActive(false);
        }

        else if (other.tag == "Monster")
        {
            if (spear <= 0)
            {
                Time.timeScale = 0;
                print("Game over...:");
                game_over.SetActive(true);
                gameSounds.PlaySound(2);
                //   SceneManager.LoadScene(0);
            }
            else
            {
                spear--;
                print("Spear count:" + spear);
                scoreManager.AddPoints(pointsForEnemy);
                other.gameObject.SetActive(false);
            }
        }
        else if (other.tag == "Pit")
        {
            if (ladder <= 0)
            {
                Time.timeScale = 0;
                print("Game over...:");
                game_over.SetActive(true);

                //  SceneManager.LoadScene(0);
            }
            else
            {
                ladder--;
                print("Ladder count:" + ladder);
                scoreManager.AddPoints(pointsForEnemy);
                other.gameObject.SetActive(false);
                gameSounds.PlaySound(2);
            }
        }
        else if (other.tag == "Chest")
        {
            scoreManager.AddPoints(pointsForChest);
            scoreManager.SaveCurrentPoints();
            SceneManager.LoadScene(1);
        }
    }
}
