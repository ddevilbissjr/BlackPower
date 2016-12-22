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
}
