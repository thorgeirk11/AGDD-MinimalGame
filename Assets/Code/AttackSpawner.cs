using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class AttackSpawner : UIBehaviour
{
    public Enemy enemyPrefab;
    public float StartSpeed = 20;
    public float MaxSpeedDelta = 20;
    public float SpeedIncrament = 4;

    public IEnumerator StartRound(int n, float duration)
    {
        var start = Time.time;
        var end = Time.time + duration;

        var enemies = new Enemy[n];
        var enemySize = Vector3.zero;
        var delta = (end - start) / n;
        for (int i = 0; i < n; i++)
        {
            var spawnPosition = new Vector3(Random.Range(enemySize.x, Screen.width - enemySize.x), 0);
            var worldPoint = Camera.main.ScreenToWorldPoint(spawnPosition);
            worldPoint.z = 0;
            enemies[i] = Instantiate(enemyPrefab, worldPoint, Quaternion.identity);

            var min = SpeedIncrament * i + StartSpeed;
            var max = min + MaxSpeedDelta;
            enemies[i].StartMoving(Random.Range(min, max));
            enemySize = enemies[i].GetComponent<Collider2D>().bounds.size;
            if (i + 1 < n)
                yield return new WaitForSeconds(delta);
        }
        //yield return new WaitWhile(() => enemies.Any(e => e != null && e.IsVisible));
    }
}
