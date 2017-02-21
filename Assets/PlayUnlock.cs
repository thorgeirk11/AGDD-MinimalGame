using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlayUnlock : MonoBehaviour, IPointerDownHandler, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    public Image Release;
    public Text Header;
    public GameManager gameManager;
    public DefenceSpawner spawner;

    public void OnDrag(PointerEventData eventData)
    {
        spawner.OnDrag(eventData);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        spawner.OnBeginDrag(eventData);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        spawner.OnEndDrag(eventData);
        gameManager.PlayPressed();
        SoundManager.Instance.PlayButtonsSound();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Header.enabled = false;
    }
}
