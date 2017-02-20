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
            defence.WeaponActive.DropWeapon();

            var canvasgroup = infoBox.GetComponent<CanvasGroup>();
            infoBox.SetTrigger("Show");
            infoText.text = "Round " + (i + 1);
            yield return new WaitForSeconds(1f);
            yield return new WaitWhile(() => canvasgroup.alpha > 0);
        }
    }

    internal void GameOver()
    {
        StopCoroutine(waveSpawner);

        foreach (var body in FindObjectsOfType<Rigidbody2D>())
        {
            body.constraints = RigidbodyConstraints2D.FreezeAll;
        }
        StartCoroutine(Utils.Wait(1f, () => GameOverView.gameObject.SetActive(true)));
    }

    public void Retry()
    {
        PlayPressed();
    }
}
