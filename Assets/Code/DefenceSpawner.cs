using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DefenceSpawner : UIBehaviour, IPointerDownHandler//, IPointerUpHandler
{
    private const int HeighOffset = 100;
    public Weapon WeaponPrefab;
    public Weapon WeaponActive;

    bool hasSetAncorPoint;

    public void OnPointerDown(PointerEventData touchPoint)
    {
        var adjustTouch = new Vector2(touchPoint.pressPosition.x, Screen.height - HeighOffset);
        var hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(adjustTouch), Vector2.zero);

        if (WeaponActive != null && WeaponActive.isDropping)
            hasSetAncorPoint = false;

        if (!hasSetAncorPoint)
        {
            if (WeaponActive != null)
                WeaponActive.DropWeapon();

            WeaponActive = Instantiate(WeaponPrefab);
            WeaponActive.SetOrigin(hit.point);
            hasSetAncorPoint = true;
        }
        else
        {
            WeaponActive.SetEnd(hit.point);
            hasSetAncorPoint = false;
        }
    }
}
