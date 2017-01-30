using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public DefenceSpawner defence;
    public AttackSpawner attack;

    // Use this for initialization
    IEnumerator Start () {
        for (int i = 1; i < 5; i*=2)
        {
            yield return StartCoroutine(attack.StartRound(i, i + 10));
            yield return new WaitWhile(() => FindObjectsOfType<Enemy>().Any());
        }
    }
    
}
