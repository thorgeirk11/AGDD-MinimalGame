using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class AttackSpawner : UIBehaviour
{
    public Enemy[] enemyPrefabs;
    public Health healthPrefab;
    public Plus plusPrefab;
    public Minus minusPrefab;

    public float StartSpeed = 1;
    public float MaxSpeedDelta = 0.2f;
    public float SpeedIncrament = 0.005f;

    public IEnumerator StartRound(int wave, int n, float duration)
    {
        var start = Time.time;
        var end = Time.time + duration;

        var enemies = new Enemy[n];
        var enemySize = Vector3.zero;
        var delta = (end - start) / n;
        for (int i = 0; i < n; i++)
        {
            if (GameManager.GameOver) yield break;

            Vector3 worldPoint = GetSpwanLocation(enemySize);
            enemies[i] = Instantiate(enemyPrefabs[i % 4 == 3 ? 1 : 0], worldPoint, Quaternion.identity);

            var min = SpeedIncrament * wave + StartSpeed;
            var max = min + MaxSpeedDelta;
            enemies[i].StartMoving(Random.Range(min, max));
            enemySize = enemies[i].GetComponent<Collider2D>().bounds.size;
            if (i + 1 < n)
                yield return new WaitForSeconds(delta / 2);

            if (Random.value < 0.05f)
            {
                worldPoint = GetSpwanLocation(enemySize);
                Instantiate(plusPrefab, worldPoint, Quaternion.identity);
            }

            if (Random.value < 0.08f)
            {
                worldPoint = GetSpwanLocation(enemySize);
                Instantiate(minusPrefab, worldPoint, Quaternion.identity);
            }
            if (i + 1 < n)
                yield return new WaitForSeconds(delta / 2);
        }
        if (Random.value < 0.3f)
        {
            var spawnPosition = new Vector3(Random.Range(enemySize.x, Screen.width - enemySize.x), 0);
            var worldPoint = Camera.main.ScreenToWorldPoint(spawnPosition);
            worldPoint.z = 0;

            Instantiate(healthPrefab, worldPoint, Quaternion.identity);
        }
    }

    private static Vector3 GetSpwanLocation(Vector3 enemySize)
    {
        var spawnPosition = new Vector3(Random.Range(enemySize.x, Screen.width - enemySize.x), 0);
        var worldPoint = Camera.main.ScreenToWorldPoint(spawnPosition);
        worldPoint.z = 0;
        return worldPoint;
    }
}
