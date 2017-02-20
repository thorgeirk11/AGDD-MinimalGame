using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverScreen : MonoBehaviour
{
    public Text nowWaves;
    public Text nowScore;
    public Text bestWaves;
    public Text bestScore;

    // Use this for initialization
    void OnEnable()
    {
        var scoreSystem = GetComponentInParent<ScoreSystem>();
        var gameManager = GetComponentInParent<GameManager>();

        var curBestWave = PlayerPrefs.GetFloat("bestWave");
        var curBestScore = PlayerPrefs.GetFloat("bestScore");
        var score = Mathf.FloorToInt(scoreSystem.Score);
        if (curBestScore < score)
        {
            PlayerPrefs.SetInt("bestScore", score);
            curBestScore = score;
        }

        var wave = gameManager.CurrentWave;
        if (curBestWave < wave)
        {
            PlayerPrefs.SetInt("bestWave", wave);
            curBestWave = wave;
        }

        nowWaves.text = wave.ToString();
        nowScore.text = score.ToString();
        bestWaves.text = curBestWave.ToString();
        bestScore.text = curBestScore.ToString();
    }
}
