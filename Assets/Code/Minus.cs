using System;
using UnityEngine;

public class Minus : Powerup
{
    GameManager manager;
    DefenceSpawner defence;
    public float Duration = 5f;
    public float SizeModifyer = 0.5f;

    protected override void Awake()
    {
        base.Awake();
        defence = FindObjectOfType<DefenceSpawner>();
        manager = FindObjectOfType<GameManager>();
    }

    protected override void OnPickup()
    {
        defence.WeaponSizeModifyer = SizeModifyer;
        defence.WeaponSizeModifyerTime = Time.time + Duration;
        defence.ScaleWeapon();
        manager.ShowMinusUI(Duration);
    }
}
