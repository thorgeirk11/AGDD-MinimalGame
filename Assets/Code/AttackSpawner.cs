using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class AttackSpawner : UIBehaviour
{
    public Enemy[] enemyPrefabs;
    public float StartSpeed = 1;
    public float MaxSpeedDelta = 0.2f;
    public float SpeedIncrament = 0.02f;



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
            enemies[i] = Instantiate(enemyPrefabs[i % 4 == 0 ? 1 : 0], worldPoint, Quaternion.identity);

            var min = SpeedIncrament * i + StartSpeed;
            var max = min + MaxSpeedDelta;
            enemies[i].StartMoving(Random.Range(min, max));
            enemySize = enemies[i].GetComponent<Collider2D>().bounds.size;
            if (i + 1 < n)
                yield return new WaitForSeconds(delta);
        }
    }
}
