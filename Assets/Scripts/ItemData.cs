using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.EventSystems;

public class ItemData : MonoBehaviour, IPointerDownHandler, IDragHandler, IEndDragHandler {
    public Item item;

    public BlackPowerItems type;
    public string itemName;
    public int howMany;
    public Sprite sprite;
    public Image itemImage;
    public Text howManyNum;

    private Transform currentParent;
    private Vector2 offset;

    public void Settings (Item newItem) {
        item = newItem;

        type = item.type;
        itemName = item.getItemName();
        howMany = item.getHowMany();
        sprite = Resources.Load<Sprite>("Sprites/" + itemName);

        currentParent = transform.parent;

        ApplySettings();
    }

    void Update () {
        if(item != null) {
            howMany = item.getHowMany();
            if (howMany > 0) {
                howManyNum.text = howMany.ToString();
            } else {
                howManyNum.text = null;
            }
        }
    }

    void ApplySettings () {
        itemImage.overrideSprite = sprite;
    }

    public void OnPointerDown (PointerEventData eventData) {
        if (item != null) {
            offset = eventData.position - new Vector2(transform.position.x, transform.position.y);
            transform.SetParent(transform.parent.parent);
            transform.position = eventData.position - offset;
        }
    }

    public void OnDrag (PointerEventData eventData) {
        if (item != null) {
            transform.position = eventData.position - offset;
        }
    }

    public void OnEndDrag (PointerEventData eventData) {
        
    }
}
