using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class AttackSpawner : UIBehaviour
{
    public Enemy enemyPrefab;

    public IEnumerator StartRound(int n, float duration)
    {
        var start = Time.time;
        var end = Time.time + duration;

        var delta = (end - start) / n;
        for (int i = 0; i < n; i++)
        {
            var spawnPosition = new Vector3(Random.Range(0, Screen.width), 0);
            var worldPoint = Camera.main.ScreenToWorldPoint(spawnPosition);
            worldPoint.z = 0;
            Instantiate(enemyPrefab, worldPoint, Quaternion.identity);
            yield return new WaitForSeconds(delta);
        }

    }
}
