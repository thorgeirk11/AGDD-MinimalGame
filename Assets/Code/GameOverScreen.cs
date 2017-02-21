using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverScreen : MonoBehaviour
{
    public Text nowScore;
    public Text bestScore;

    // Use this for initialization
    void OnEnable()
    {
        var scoreSystem = GetComponentInParent<ScoreSystem>();
        var gameManager = GetComponentInParent<GameManager>();

        var curBestScore = PlayerPrefs.GetInt("bestScore");
        var score = Mathf.FloorToInt(scoreSystem.Score);
        if (curBestScore < score)
        {
            PlayerPrefs.SetInt("bestScore", score);
            curBestScore = score;
        }
        nowScore.text = score.ToString();
        bestScore.text = curBestScore.ToString();
    }
}
