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

    public int CurrentWave { get; private set; }

    public CanvasGroup CountDownGroup;
    public Text CountDownText;
    public Image Minus;
    public Image Plus;
    private float showTimeCountdown;

    internal void ShowMinusUI(float duration)
    {
        Minus.enabled = true;
        Plus.enabled = false;
        showTimeCountdown = Time.time + duration;
    }

    internal void ShowPlusUI(float duration)
    {
        Minus.enabled = false;
        Plus.enabled = true;
        showTimeCountdown = Time.time + duration;
    }

    void Update()
    {
        if (showTimeCountdown > Time.time)
        {
            CountDownGroup.alpha = 1;
            CountDownText.text = Mathf.CeilToInt(showTimeCountdown - Time.time).ToString();
        }
        else CountDownGroup.alpha = 0;
    }

    public static bool GameOver { get; private set; }

    Coroutine waveSpawner;

    public void PlayPressed()
    {
        scoreSystem.LifeCounter.Lifes = 3;
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
            yield return StartCoroutine(attack.StartRound(i, i + 3, duration));
            if (GameOver) yield break;
            while (FindObjectsOfType<Enemy>().Any(e => !e.HitByWeapon))
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
        StartCoroutine(Utils.Wait(1f, () =>
        {
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
        foreach (var body in FindObjectsOfType<Powerup>())
            Destroy(body.gameObject);
        foreach (var body in FindObjectsOfType<Weapon>())
            Destroy(body.gameObject);
    }
}
