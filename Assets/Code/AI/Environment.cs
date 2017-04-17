using System;
using System.Collections.Generic;
using System.Linq;

[Serializable]
public struct StateActionPair 
{
    public State state;
    public byte action;

    public StateActionPair(State state, AIAction action)
    {
        this.state = state;
        this.action = action.Spawner;
    }
}

[Serializable]
public struct AIAction
{
    public byte Spawner;
}

/// <summary>
/// Since the time step is fixed and the spawnlocations.
/// The state is represented by the x locations for each for the enemies.
/// </summary>
[Serializable]
public class State : IEquatable<State>
{
    public byte[] EnemyYPos;

    public override bool Equals(object obj)
    {
        return Equals(obj as State);
    }

    public bool Equals(State other)
    {
        if (other == null) return false;
        return ArraysEqual(EnemyYPos, other.EnemyYPos);
    }

    static bool ArraysEqual(byte[] a1, byte[] a2)
    {
        if (a1.Length == a2.Length)
        {
            for (int i = 0; i < a1.Length; i++)
            {
                if (a1[i] != a2[i])
                {
                    return false;
                }
            }
            return true;
        }
        return false;
    }

    public override int GetHashCode()
    {
        var hc = EnemyYPos.Length;
        for (int i = 0; i < EnemyYPos.Length; ++i)
        {
            hc = unchecked(hc * 314159 + EnemyYPos[i]);
        }
        return hc;
    }
}