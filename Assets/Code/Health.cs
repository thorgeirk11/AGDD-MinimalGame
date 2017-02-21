using System;
using UnityEngine;

public class Health : Powerup
{
    protected override void OnPickup()
    {
        ScoreSystem.Instance.LifeCounter.Lifes++;
    }
}
