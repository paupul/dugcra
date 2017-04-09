using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour {

    public static int score;

    public static int highscore;

    public Text scoreText;

    void Start()
    {

        score = PlayerPrefs.GetInt("score");
        scoreText.text = "Score: " + score;

        PlayerPrefs.SetInt("score", 0);

        highscore = PlayerPrefs.GetInt("highscore", highscore);
    }
    void Update()
    {
        if (score > highscore)
        {
            highscore = score;
            PlayerPrefs.SetInt("highscore", highscore);
        }
    }

    public void AddPoints(int pointsToAdd)
    {
        score += pointsToAdd;
        scoreText.text = "Score: " + score;
    }

    public void Reset()
    {
        score = 0;
        scoreText.text = "Score: " + score;
    }

    public void SaveCurrentPoints()
    {
        PlayerPrefs.SetInt("score", score);
        scoreText.text = "Score: " + score;
    }
}
