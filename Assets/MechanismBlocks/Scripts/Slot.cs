﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Slot : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler {

    public UI UI;

    public ItemData itemData;
    public UISlotType slotType;

    public Inventory playerInventory;
    public BaseUI inventory;
    public int slotNumber;

    public bool mouseIsOverSlot;

    void Update () {
        if (mouseIsOverSlot && itemData.item != null) {
            RectTransform itemInfoParent = UI.itemInfo;
            Text itemInfo = itemInfoParent.GetComponentInChildren<Text>();
            float scaleFactor = UI.GetComponent<Canvas>().scaleFactor;

            itemInfoParent.position = transform.position + new Vector3(0, itemInfoParent.rect.height * scaleFactor, 0);

            itemInfo.text = itemData.itemName;
            itemInfo.color = GetTextColor();

            if (!itemInfoParent.gameObject.activeSelf) {
                itemInfoParent.gameObject.SetActive(true);
            }
        }
    }

    public void OnDrop (PointerEventData eventData) {
        playerInventory = UI.inventory;
        ItemData item = eventData.pointerDrag.GetComponent<ItemData>();
        
        if(itemData.item == null || itemData.item == item) {
            playerInventory.MoveInInventory(item.slotType, item.slotNumber, null, slotType, slotNumber, item.item);

            UI.SetIconTo(item.slotType, null, item.slotNumber);
            UI.SetIconTo(slotType, item.item, slotNumber);

            item.slotType = slotType;
            item.slotNumber = slotNumber;
            item.inventory = inventory;
        } else {
            if (itemData.itemName == item.itemName && itemData.item.getHowMany() < itemData.item.getStackSize() && itemData.item.getStackable()) {
                int difference = Mathf.Abs(item.item.getHowMany() - (itemData.item.getStackSize() - itemData.item.getHowMany()));

                itemData.item.setHowMany(HowMany.totalValue, difference);
                item.item.setHowMany(HowMany.totalValue, item.item.getStackSize());
            }

            playerInventory.MoveInInventory(item.slotType, item.slotNumber, itemData.item, slotType, slotNumber, item.item);

            UI.SetIconTo(item.slotType, itemData.item, item.slotNumber);
            UI.SetIconTo(slotType, item.item, slotNumber);

            item.slotType = slotType;
            item.slotNumber = slotNumber;
            item.inventory = inventory;

            itemData.slotType = item.slotType;
            itemData.slotNumber = item.slotNumber;
            itemData.inventory = item.inventory;
        }
    }

    public void OnPointerEnter (PointerEventData eventData) {
        if(UI.itemInfo.GetComponent<CanvasGroup>().alpha == 0 && itemData.item != null) {
            UI.itemInfo.GetComponent<CanvasGroup>().alpha = 1;
        }

        mouseIsOverSlot = true;
    }

    public void OnPointerExit (PointerEventData eventData) {
        RectTransform itemInfoParent = UI.itemInfo;

        if (itemInfoParent.gameObject.activeSelf) {
            itemInfoParent.gameObject.SetActive(false);
        }

        mouseIsOverSlot = false;
    }

    public Color GetTextColor () {
        Color color = Color.black;

        if (itemData.item.GetType().IsSubclassOf(typeof(Mechanism))) {
            color = Color.red;
        }

        return color;
    }
}
