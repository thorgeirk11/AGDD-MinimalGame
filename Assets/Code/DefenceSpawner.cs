using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DefenceSpawner : UIBehaviour, IPointerDownHandler//, IPointerUpHandler
{
    public Weapon WeaponPrefab;
    Weapon weaponActive;

    bool hasSetAncorPoint;

    float nextTimeAllowed;

    public void OnPointerDown(PointerEventData touchPoint)
    {
        var adjustTouch = new Vector2(touchPoint.pressPosition.x, Screen.height-10);
        var hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(adjustTouch), Vector2.zero);

        if (!hasSetAncorPoint)
        {
            if (weaponActive != null)
                weaponActive.DropWeapon();

            weaponActive = Instantiate(WeaponPrefab);
            weaponActive.SetOrigin(hit.point);
            hasSetAncorPoint = true;
        }
        else
        {
            weaponActive.SetEnd(hit.point);
            hasSetAncorPoint = false;
        }
    }
}
