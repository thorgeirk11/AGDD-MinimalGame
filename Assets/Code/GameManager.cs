using Assets.Code;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using System;

public class GameManager : MonoBehaviour
{
    public DefenceSpawner defence;
    public AttackSpawner attack;
    public ScoreSystem scoreSystem;

    public RectTransform MainMenu;
    public RectTransform GameUI;
    public RectTransform GameOverView;
    public Animator infoBox;
    private Text infoText;

    public int CurrentWave { get; private set; }
    public static bool GameOver { get; private set; }

    Coroutine waveSpawner;

    // Use this for initialization
    void Start()
    {
        infoText = infoBox.GetComponentInChildren<Text>();
    }
    public void PlayPressed()
    {
        scoreSystem.LifeCounterText.Lifes = 2;
        scoreSystem.Score = 0;
        GameUI.gameObject.SetActive(true);
        defence.gameObject.SetActive(true);
        attack.gameObject.SetActive(true);
        GameOverView.gameObject.SetActive(false);
        MainMenu.gameObject.SetActive(false);
        waveSpawner = StartCoroutine(StartGame());
    }

    IEnumerator StartGame()
    {
        for (int i = 1; true; i++)
        {
            CurrentWave = i;
            var duration = i * 1.5f + 5;
            yield return StartCoroutine(attack.StartRound(i*2, duration));
            while (FindObjectsOfType<Enemy>().Any())
            {
                yield return new WaitForSeconds(1f);
            }
        }
    }

    internal void LostGame()
    {
        GameOver = true;
        StopCoroutine(waveSpawner);

        foreach (var body in FindObjectsOfType<Rigidbody2D>())
        {
            body.constraints = RigidbodyConstraints2D.FreezeAll;
        }
        StartCoroutine(Utils.Wait(1f, () => {
            GameUI.gameObject.SetActive(false);
            GameOverView.gameObject.SetActive(true);
        }));
    }

    public void Retry()
    {
        PlayPressed();
        GameOver = false;
        foreach (var body in FindObjectsOfType<Enemy>())
            Destroy(body.gameObject);
        foreach (var body in FindObjectsOfType<Weapon>())
            Destroy(body.gameObject);
    }
}
