using System;
using UnityEngine;

public class Plus : Powerup
{
    GameManager manager;
    DefenceSpawner defence;
    public float Duration = 10f;
    public float SizeModifyer = 1.5f;

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
        manager.ShowPlusUI(Duration);
    }
}
