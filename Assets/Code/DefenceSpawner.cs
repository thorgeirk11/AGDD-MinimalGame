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
    

    public void OnPointerDown(PointerEventData touchPoint)
    {
        if (weaponActive == null || weaponActive.CanBeDropped)
        {
            var hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(touchPoint.pressPosition), Vector2.zero);
            if (!hasSetAncorPoint)
            {
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
}
