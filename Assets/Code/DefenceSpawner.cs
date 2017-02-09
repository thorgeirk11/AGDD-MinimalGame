using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DefenceSpawner : UIBehaviour, IPointerDownHandler//, IPointerUpHandler
{
    private const int HeighOffset = 60;
    private float colDepth = 4f;
    private float zPosition;
    protected override void Start()
    {
        //Generate our empty objects
        var top = new GameObject().transform;
        top.name = "TopCollider";
        top.tag = "Defence";
        var defenceCollider = top.gameObject.AddComponent<BoxCollider2D>();
        top.parent = transform;
        var cameraPos = Camera.main.transform.position;
        var screenSize = Vector2.zero;
        screenSize.x = Vector2.Distance(Camera.main.ScreenToWorldPoint(new Vector2(0, 0)), Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, 0))) * 0.5f;
        screenSize.y = Vector2.Distance(Camera.main.ScreenToWorldPoint(new Vector2(0, 0)), Camera.main.ScreenToWorldPoint(new Vector2(0, Screen.height))) * 0.5f;
        top.localScale = new Vector3(screenSize.x * 2, colDepth, colDepth);
        top.position = new Vector3(cameraPos.x, cameraPos.y + screenSize.y + (top.localScale.y * 0.5f), zPosition);
    }

    public Weapon WeaponPrefab;
    public Weapon weaponActive;

    bool hasSetAncorPoint;

    public void OnPointerDown(PointerEventData touchPoint)
    {
        var adjustTouch = new Vector2(touchPoint.pressPosition.x, Screen.height - HeighOffset);
        var hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(adjustTouch), Vector2.zero);

        if (weaponActive != null && weaponActive.isDropping)
            hasSetAncorPoint = false;

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
