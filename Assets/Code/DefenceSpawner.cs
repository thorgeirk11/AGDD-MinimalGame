using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DefenceSpawner : UIBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler
{
    private const float SpawnOffset = 0.90f;
    public Weapon WeaponPrefab;
    public Weapon WeaponActive;

    public bool IsDragging = false;

    public float WeaponSizeModifyer { get; internal set; }
    public float WeaponSizeModifyerTime { get; internal set; }

    internal void ScaleWeapon()
    {
        if (Time.time > WeaponSizeModifyerTime)
            WeaponSizeModifyer = 1;
        WeaponActive.ScaleEnd(WeaponSizeModifyer, WeaponSizeModifyerTime);
    }

    public void OnBeginDrag(PointerEventData touchPoint)
    {
        if (GameManager.GameOver) return;

        var adjustTouch = new Vector2(touchPoint.pressPosition.x, Screen.height * SpawnOffset);
        var hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(adjustTouch), Vector2.zero);

        if (WeaponActive != null)
            WeaponActive.DropWeapon();

        WeaponActive = Instantiate(WeaponPrefab);
        WeaponActive.SetOrigin(hit.point);
        IsDragging = true;
    }

    public void OnDrag(PointerEventData touchPoint)
    {
        if (GameManager.GameOver) return;

        var adjustTouch = new Vector2(touchPoint.position.x, Screen.height * SpawnOffset);
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
        ScaleWeapon();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (GameManager.GameOver) return;

        WeaponActive.ReleaseWeapon();
        IsDragging = false;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (GameManager.GameOver) return;
        if (IsDragging) return;

        if (WeaponActive != null)
            WeaponActive.DropWeapon();
        WeaponActive = null;

    }
}
