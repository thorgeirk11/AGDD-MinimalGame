using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DefenceSpawner : UIBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler
{
    public Weapon WeaponPrefab;
    public Weapon WeaponActive;

    public bool IsDragging = false;

    public void OnBeginDrag(PointerEventData touchPoint)
    {
        var adjustTouch = new Vector2(touchPoint.pressPosition.x, Screen.height * 0.93f);
        var hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(adjustTouch), Vector2.zero);

        if (WeaponActive != null)
            WeaponActive.DropWeapon();

        WeaponActive = Instantiate(WeaponPrefab);
        WeaponActive.SetOrigin(hit.point);
        IsDragging = true;
    }

    public void OnDrag(PointerEventData touchPoint)
    {
        var adjustTouch = new Vector2(touchPoint.position.x, Screen.height * 0.93f);
        var hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(adjustTouch), Vector2.zero);


        var wPos = WeaponActive.Origin;
        var end = wPos;
        var magnitute = Mathf.Max(Vector2.Distance(hit.point, wPos), 1.2f);
        if (wPos.x > hit.point.x)
        {
            end = wPos + Vector2.left * magnitute;
        }
        else
        {
            end = wPos + Vector2.right * magnitute;
        }

        WeaponActive.SetEnd(end);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        WeaponActive.ReleaseWeapon();
        IsDragging = false;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (IsDragging) return;

        if (WeaponActive != null)
            WeaponActive.DropWeapon();
        WeaponActive = null;

    }
}
