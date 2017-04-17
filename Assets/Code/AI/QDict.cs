using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class QDict
{
    public Dictionary<StateActionPair, List<float>> Dict;

    public QDict()
    {
        Dict = new Dictionary<StateActionPair, List<float>>();
    }
    public QDict(Dictionary<StateActionPair, List<float>> dict)
    {
        Dict = dict;
    }

    public void AddRecord(StateActionPair pair, float val)
    {
        var vals = new List<float>();
        if (Dict.TryGetValue(pair, out vals))
            vals.Add(val);
        else
            Dict[pair] = new List<float> { val };
    }
    public float GetValue(StateActionPair pair)
    {
        List<float> vals;
        if (Dict.TryGetValue(pair, out vals))
            return vals.Average();
        else
            return 0;
    }
    public bool Contains(StateActionPair pair)
    {
        return Dict.ContainsKey(pair);
    }

    public string Print()
    {
        var sb = new StringBuilder();
        var dict = Dict
                    .OrderBy(i => i.Key.state.EnemyYPos.Length)
                    .ThenBy(i => i.Key.action);
        foreach (var item in dict)
        {
            sb.Append("A: ");
            sb.Append(item.Key.action);
            sb.Append(" Q: " + item.Value.Average());
            sb.Append(" (" + item.Value.Count + ") ");
            //sb.Append(" [" + string.Join(",", item.Value.Select(i => i.ToString()).ToArray()) + "]");
            sb.Append("State: [");
            sb.Append(string.Join(",", item.Key.state.EnemyYPos.Select(i => i.ToString()).ToArray()));
            sb.Append("]");
            sb.AppendLine();
        }
        return sb.ToString();
    }
}
