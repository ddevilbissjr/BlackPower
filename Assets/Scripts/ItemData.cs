using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.EventSystems;

public class ItemData : MonoBehaviour, IPointerDownHandler, IDragHandler, IEndDragHandler {
    public Item item;

    public UISlotType slotType;
    public BlackPowerItems type;
    public string itemName;
    public int howMany;
    public int slotNumber;
    public Sprite sprite;
    public Image itemImage;
    public Text howManyNum;
    public CanvasGroup canvasGroup;
    public BaseUI inventory;
    
    private Vector2 offset;

    public void Settings (Item newItem) {
        item = newItem;

        type = item.type;
        itemName = item.getItemName();
        howMany = item.getHowMany();
        sprite = Resources.Load<Sprite>("Sprites/" + itemName);

        itemImage.overrideSprite = sprite;
    }

    void Update () {
        if(item != null) {
            howMany = item.getHowMany();
            if (howMany > 1) {
                howManyNum.text = howMany.ToString();
            } else {
                howManyNum.text = null;
            }
        }
    }

    public void OnPointerDown (PointerEventData eventData) {
        if (item != null) {
            offset = eventData.position - new Vector2(transform.position.x, transform.position.y);
            transform.SetParent(transform.parent.parent.parent);
            transform.position = eventData.position - offset;
            canvasGroup.blocksRaycasts = false;
        }
    }

    public void OnDrag (PointerEventData eventData) {
        if (item != null) {
            transform.position = eventData.position - offset;
        }
    }

    public void OnEndDrag (PointerEventData eventData) {
        Debug.Log(slotNumber);
        switch (slotType) {
            case UISlotType.hotbar:
                transform.SetParent(inventory.hotSlots[slotNumber].transform);
                transform.position = inventory.hotSlots[slotNumber].transform.position;
                break;
            case UISlotType.inventory:
                transform.SetParent(inventory.invSlots[slotNumber].transform);
                transform.position = inventory.invSlots[slotNumber].transform.position;
                break;
        }
        canvasGroup.blocksRaycasts = true;
    }
}
