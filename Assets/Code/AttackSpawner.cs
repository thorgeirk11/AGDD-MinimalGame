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

    const int EnemySize = 1;
    int partitionSize;
    int partitionCount;
    AIInterface AI;

    void Awake()
    {
        AI = GetComponent<AIInterface>();
        partitionCount = AIInterface.Actions.Length;
        var width = Screen.width - EnemySize * 2;
        partitionSize = width / partitionCount;
    }


    public IEnumerator StartRound(int wave, int n, float duration)
    {
        var start = Time.time;
        var end = Time.time + duration;

        var enemies = new Enemy[n];
        var delta = (end - start) / n;
        for (int i = 0; i < n; i++)
        {
            if (GameManager.GameOver) yield break;
            Spwan(wave, enemies, i);
            if (i + 1 < n)
                yield return new WaitForSeconds(delta / 2);

            if (Random.value < 0.05f) Instantiate(plusPrefab, GetSpwanLocation(), Quaternion.identity);
            if (Random.value < 0.08f) Instantiate(minusPrefab, GetSpwanLocation(), Quaternion.identity);
            if (i + 1 < n)
                yield return new WaitForSeconds(delta / 2);
        }
        if (Random.value < 0.3f)
        {
            Instantiate(healthPrefab, GetSpwanLocation(), Quaternion.identity);
        }
    }

    private void Spwan(int wave, Enemy[] enemies, int i)
    {
        var stateActionPair = AI.GetAction(enemies);

        var spawner = stateActionPair.action;

        var worldPoint = GetSpwanLocation(spawner);
        enemies[i] = Instantiate(enemyPrefabs[i % 4 == 3 ? 1 : 0], worldPoint, Quaternion.identity);
        enemies[i].SpawnInfo = stateActionPair;
        enemies[i].AI = AI;

        var min = SpeedIncrament * wave + StartSpeed;
        var max = min + MaxSpeedDelta;
        enemies[i].StartMoving(Random.Range(min, max));
    }

    private Vector3 GetSpwanLocation()
    {
        var spawnPosition = new Vector3(Random.Range(EnemySize, Screen.width - EnemySize), 0);
        var worldPoint = Camera.main.ScreenToWorldPoint(spawnPosition);
        worldPoint.z = 0;
        return worldPoint;
    }
    private Vector3 GetSpwanLocation(int spawner)
    {
        var spawnPosition = new Vector3(Random.Range(spawner * partitionSize, spawner * partitionSize + partitionSize), 0);
        var worldPoint = Camera.main.ScreenToWorldPoint(spawnPosition);
        worldPoint.z = 0;
        return worldPoint;
    }
}
