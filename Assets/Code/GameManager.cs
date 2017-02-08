using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public DefenceSpawner defence;
    public AttackSpawner attack;

    public RectTransform MainMenu;
    public Animator infoBox;
    private Text infoText;

    // Use this for initialization
    void Start()
    {
        infoText = infoBox.GetComponentInChildren<Text>();
    }
    public void PlayPressed()
    {
        defence.gameObject.SetActive(true);
        attack.gameObject.SetActive(true);
        MainMenu.gameObject.SetActive(false);
        StartCoroutine(StartGame());
    }

    IEnumerator StartGame()
    {
        for (int i = 1; true; i++)
        {
            var duration = i * 2 + 10;
            var end = Time.time + duration;
            StartCoroutine(attack.StartRound(i, duration));

            while (FindObjectsOfType<Enemy>().Any() || Time.time > end)
            {
                yield return new WaitForSeconds(1f);
            }

            var canvasgroup = infoBox.GetComponent<CanvasGroup>();
            infoBox.SetTrigger("Show");
            infoText.text = "Round " + (i+1);
            yield return new WaitForSeconds(1f);
            yield return new WaitWhile(() => canvasgroup.alpha > 0);
        }
    }

}
