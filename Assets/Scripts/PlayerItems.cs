using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerItems : MonoBehaviour
{

    private int spear;
    private int ladder;
    private static int pointsForItem;
    private static int pointsForEnemy;
    private static int pointsForChest;
    public GameObject game_over;
    public GameObject next_level;
    public ScoreManager scoreManager;
    private GameSounds gameSounds;
    public Text game_over_text;

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
            gameSounds.PlaySound(4);
            spear++;
            scoreManager.AddPoints(pointsForItem);
            print("Spear count:" + spear);
            other.gameObject.SetActive(false);

        }
        else if (other.tag == "Ladder")
        {
            gameSounds.PlaySound(4);
            ladder++;
            scoreManager.AddPoints(pointsForItem);
            print("Ladder count:" + ladder);
            other.gameObject.SetActive(false);
        }

        else if (other.tag == "Monster")
        {
            if (spear <= 0)
            {
                
                game_over_text.text = "You got butchered by the monster.";
                print("Game over...:");
                game_over.SetActive(true);
                gameSounds.PlaySound(2);
                Time.timeScale = 0;
                //   SceneManager.LoadScene(0);
            }
            else
            {
                spear--;
                gameSounds.PlaySound(5);
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
                game_over_text.text = "You died in agony inside a pit.";
                print("Game over...:");
                game_over.SetActive(true);
                gameSounds.PlaySound(2);
                //  SceneManager.LoadScene(0);
            }
            else
            {
                ladder--;
                gameSounds.PlaySound(5);
                print("Ladder count:" + ladder);
                scoreManager.AddPoints(pointsForEnemy);
                other.gameObject.SetActive(false);
            }
        }
        else if (other.tag == "Chest")
        {
            scoreManager.AddPoints(pointsForChest);
            scoreManager.SaveCurrentPoints();
            Time.timeScale = 0;
            gameSounds.PlaySound(3);
            next_level.SetActive(true);
        }
    }
}
