using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

[Serializable]
public class Q_stored
{
    public Q_stored()
    { }
    public Q_stored(Dictionary<StateActionPair, List<float>> Q)
    {
        foreach (var i in Q)
        {
            states.Add(i.Key);
            values.Add(new ListWrapper { l = i.Value });
        }
    }
    
    public List<StateActionPair> states = new List<StateActionPair>();
    public List<ListWrapper> values = new List<ListWrapper>();

    [Serializable]
    public class ListWrapper
    {
        public List<float> l;
    }

    public Dictionary<StateActionPair, List<float>> ToDic()
    {
        var dic = new Dictionary<StateActionPair, List<float>>();
        if (states == null) return dic;
        for (int i = 0; i < states.Count; i++)
        {
            dic.Add(states[i], values[i].l);
        }
        return dic;
    }
}