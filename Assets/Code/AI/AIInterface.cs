using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class AIInterface : MonoBehaviour
{
    private const int REACHING_TOP_REWARD = 1000;
    private const float RANDOM_ACTION_CHANCE = 0.1f;

    public QDict Q;
    public bool IsTotalRandom;

    AttackSpawner spawner;
    List<Enemy> enemies = new List<Enemy>();

    void Awake()
    {
        IsTotalRandom = Random.value > .5f;
        spawner = GetComponent<AttackSpawner>();
        var storedQJson = PlayerPrefs.GetString("QStored" + (IsTotalRandom ? "Rand" : ""), "");
        var storedQ = JsonUtility.FromJson<Q_stored>(storedQJson);
        if (storedQ != null)
            Q = new QDict(storedQ.ToDic());
        else
            Q = new QDict();
    }

    public static string printStats()
    {
        var randQ = new QDict(
            JsonUtility.FromJson<Q_stored>(
                PlayerPrefs.GetString("QStoredRand")).ToDic()
            );
        var aiQ = new QDict(
            JsonUtility.FromJson<Q_stored>(
                PlayerPrefs.GetString("QStored")).ToDic()
            );

        var rand = GroupByEnemyCount(randQ);
        var ai = GroupByEnemyCount(aiQ);

        var sb = new System.Text.StringBuilder();
        sb.AppendLine("Rand:");
        foreach (var g in rand)
        {
            sb.AppendLine(g.Key + " " + g.Average());
        }
        sb.AppendLine("avg: " + rand.Average(i => i.Average()));
        sb.AppendLine();
        sb.AppendLine("AI:");
        foreach (var g in ai)
        {
            sb.AppendLine(g.Key + " " + g.Average());
        }
        sb.AppendLine("avg: " + ai.Average(i => i.Average()));
        return sb.ToString();
    }

    private static IEnumerable<IGrouping<int, float>> GroupByEnemyCount(QDict aiQ)
    {
        return from i in aiQ.Dict
               from x in i.Value
               where x < REACHING_TOP_REWARD
               group x by i.Key.state.EnemyYPos.Length
                 into g
               orderby g.Key
               select g;
    }

    public void SaveQ()
    {
        var storedQ = new Q_stored(Q.Dict);
        var json = JsonUtility.ToJson(storedQ, true);
        PlayerPrefs.SetString("QStored" + (IsTotalRandom ? "Rand" : ""), json);
    }

    public void LateReward(StateActionPair spawnInfo, Enemy enemy)
    {
        SaveQ();
        var y = enemy.transform.position.y;
        var reward = Mathf.Round(y * 10);
        Q.AddRecord(spawnInfo, reward);
    }
    public void LateRewardMax(StateActionPair spawnInfo)
    {
        Q.AddRecord(spawnInfo, REACHING_TOP_REWARD);
    }

    internal StateActionPair GetAction(Enemy[] enemies)
    {
        var state = GetGameState(enemies);
        var allPairs = Actions.Select(a => new StateActionPair(state, a));
        var allQ = from pair in allPairs
                   where Q.Contains(pair)
                   let q = Q.GetValue(pair)
                   orderby q descending
                   select new { pair, q };

        if (!IsTotalRandom &&
            allQ.Count() >= Actions.Length / 2 &&
            Random.value > RANDOM_ACTION_CHANCE)
        {
            var max = allQ.FirstOrDefault();
            return max.pair;
        }
        else
        {
            var rand = Random.Range(0, Actions.Length);
            return new StateActionPair(state, Actions[rand]);
        }
    }

    private State GetGameState(Enemy[] enemies)
    {
        return new State
        {
            EnemyYPos =
                (from e in enemies
                 where e != null && !e.HitByWeapon
                 let y = e.transform.position.y
                 orderby y
                 select (byte)Mathf.FloorToInt(y)).ToArray()
        };
    }

    public static readonly AIAction[] Actions =
    {
        new AIAction { Spawner = 0 },
        new AIAction { Spawner = 1 },
        new AIAction { Spawner = 2 },
        new AIAction { Spawner = 3 },
        new AIAction { Spawner = 4 },
        new AIAction { Spawner = 5 },
    };

}
