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
        for (int i = 1; i < 5; i++)
        {
            yield return StartCoroutine(attack.StartRound(i, i * 2 + 10));
            yield return new WaitWhile(() => FindObjectsOfType<Enemy>().Any());

            infoBox.SetTrigger("Show");
            infoText.text = "Round " + i;
            yield return new WaitForSeconds(1f);
        }
    }

}
