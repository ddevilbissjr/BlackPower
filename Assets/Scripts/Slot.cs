using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Slot : MonoBehaviour, IDropHandler {

    public UI UI;

    public ItemData itemData;
    public UISlotType slotType;

    public Inventory playerInventory;
    public BaseUI inventory;
    public int slotNumber;

    public void OnDrop (PointerEventData eventData) {
        playerInventory = UI.inventory;
        ItemData item = eventData.pointerDrag.GetComponent<ItemData>();
        
        if(itemData.item == null) {
            playerInventory.MoveInInventory(item.slotType, item.slotNumber, null, slotType, slotNumber, item.item);

            UI.SetIconTo(item.slotType, null, item.slotNumber);
            UI.SetIconTo(slotType, item.item, slotNumber);

            item.slotType = slotType;
            item.slotNumber = slotNumber;
            item.inventory = inventory;
        } else {
            playerInventory.MoveInInventory(item.slotType, item.slotNumber, itemData.item, slotType, slotNumber, item.item);

            UI.SetIconTo(item.slotType, itemData.item, item.slotNumber);
            UI.SetIconTo(slotType, item.item, slotNumber);

            Debug.Log(itemData.slotNumber + ", " + item.slotNumber);

            item.slotType = slotType;
            item.slotNumber = slotNumber;
            item.inventory = inventory;

            Debug.Log(itemData.slotNumber + ", " + item.slotNumber);

            itemData.slotType = item.slotType;
            itemData.slotNumber = item.slotNumber;
            itemData.inventory = item.inventory;

            Debug.Log(itemData.slotNumber + ", " + item.slotNumber);
        }
    }
}
