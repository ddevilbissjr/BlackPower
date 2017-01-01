using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour {

    public UIClass currentUIEnabled = UIClass.hotbarUI;

    public RectTransform itemInfo;
    public RectTransform currentSlotMarker;

    bool currentSlotMarkerIsInitialized = false;

    public Inventory inventory;

    public HotbarUI hotbarUI;
    public InventoryUI inventoryUI;
    public MechanismUI mechanismUI;

    void Update () {
        if(currentUIEnabled == UIClass.hotbarUI) {
            itemInfo.gameObject.SetActive(false);
            itemInfo.GetComponent<CanvasGroup>().alpha = 0;
        }
    }

    public void SetSlotFor (UISlotType slotType, UITypes type, int slotNumber) {
        switch (slotType) {
            case UISlotType.hotbar:
                switch (type) {
                    case UITypes.hotbarUI_hotslots:
                        hotbarUI.hotSlots.Add(CreateSlot(hotbarUI, slotType, hotbarUI.hotbarGo.transform, slotNumber));
                        break;
                    case UITypes.inventoryUI_hotSlots:
                        inventoryUI.hotSlots.Add(CreateSlot(inventoryUI, slotType, inventoryUI.hotbarGo.transform, slotNumber));
                        break;
                    case UITypes.mechanismUI_hotSlots:
                        mechanismUI.hotSlots.Add(CreateSlot(mechanismUI, slotType, mechanismUI.hotbarGo.transform, slotNumber));
                        break;
                    default:
                        Debug.Log("None set for " + type.ToString());
                        break;
                }
                break;
            case UISlotType.inventory:
                switch (type) {
                    case UITypes.inventoryUI_invSlots:
                        inventoryUI.invSlots.Add(CreateSlot(inventoryUI, slotType, inventoryUI.inventoryGo.transform, slotNumber));
                        break;
                    case UITypes.mechanismUI_invSlots:
                        mechanismUI.invSlots.Add(CreateSlot(mechanismUI, slotType, mechanismUI.inventoryGo.transform, slotNumber));
                        break;
                    default:
                        Debug.Log("None set for " + type.ToString());
                        break;
                }
                break;
        }
    }

    public void SetIconFor (UITypes type, Item item, Slot slot) {
        ItemData createdGo = CreateSlotPlaceholder(item, slot);

        switch (type) {
            case UITypes.hotbarUI_hotslots:
                hotbarUI.hotSlots[hotbarUI.hotSlots.IndexOf(slot)].itemData = createdGo;
                break;
            case UITypes.inventoryUI_hotSlots:
                inventoryUI.hotSlots[inventoryUI.hotSlots.IndexOf(slot)].itemData = createdGo;
                break;
            case UITypes.inventoryUI_invSlots:
                inventoryUI.invSlots[inventoryUI.invSlots.IndexOf(slot)].itemData = createdGo;
                break;
            case UITypes.mechanismUI_hotSlots:
                mechanismUI.hotSlots[mechanismUI.hotSlots.IndexOf(slot)].itemData = createdGo;
                break;
            case UITypes.mechanismUI_invSlots:
                mechanismUI.invSlots[mechanismUI.invSlots.IndexOf(slot)].itemData = createdGo;
                break;
            default:
                Debug.Log("None set for " + type.ToString());
                break;
        }
    }

    public void SetIconTo (UISlotType type, Item item, int slot) {
        List<ItemData> toDelete = new List<ItemData>();

        switch (type) {
            case UISlotType.hotbar:
                if (hotbarUI.initialized) {
                    toDelete.Add(hotbarUI.hotSlots[slot].itemData);
                    SetIconFor(UITypes.hotbarUI_hotslots, item, hotbarUI.hotSlots[slot]);
                }

                if (inventoryUI.initialized) {
                    toDelete.Add(inventoryUI.hotSlots[slot].itemData);
                    SetIconFor(UITypes.inventoryUI_hotSlots, item, inventoryUI.hotSlots[slot]);
                }

                if (mechanismUI.initialized) {
                    toDelete.Add(mechanismUI.hotSlots[slot].itemData);
                    SetIconFor(UITypes.mechanismUI_hotSlots, item, mechanismUI.hotSlots[slot]);
                }
                break;
            case UISlotType.inventory:
                if (inventoryUI.initialized) {
                    toDelete.Add(inventoryUI.invSlots[slot].itemData);
                    SetIconFor(UITypes.inventoryUI_invSlots, item, inventoryUI.invSlots[slot]);
                }

                if (mechanismUI.initialized) {
                    toDelete.Add(mechanismUI.invSlots[slot].itemData);
                    SetIconFor(UITypes.mechanismUI_invSlots, item, mechanismUI.invSlots[slot]);
                }
                break;
        }

        foreach (ItemData go in toDelete) {
            Destroy(go.gameObject);
        }
    }

    public Slot CreateSlot (BaseUI inventory, UISlotType type, Transform soonToBeParent, int slotNumber) {
        GameObject clone = Instantiate(Resources.Load("UI/Slot"), soonToBeParent) as GameObject;
        Slot slot = clone.GetComponent<Slot>();

        slot.slotNumber = slotNumber;
        slot.slotType = type;
        slot.inventory = inventory;
        slot.UI = this;

        return slot;
    }

    public ItemData CreateSlotPlaceholder (Item item, Slot slot) {
        GameObject clone = Instantiate(Resources.Load("UI/SlotPlaceholder"), slot.transform) as GameObject;
        ItemData data = clone.GetComponent<ItemData>();

        data.slotNumber = slot.slotNumber;
        data.slotType = slot.slotType;
        data.inventory = slot.inventory;

        if (item != null) {
            data.Settings(item);
        }

        return data;
    }

    public void UISwitchTo (UIClass type) {
        UICloseAll();

        currentUIEnabled = type;

        switch(type) {
            case UIClass.hotbarUI:
                hotbarUI.baseGo.SetActive(true);
                break;
            case UIClass.inventoryUI:
                inventoryUI.baseGo.SetActive(true);
                break;
            case UIClass.mechanismUI:
                mechanismUI.baseGo.SetActive(true);
                break;
            default:
                Debug.Log("None set for " + type.ToString());
                break;
        }
    }

    public void UICloseAll () {
        hotbarUI.baseGo.SetActive(false);
        inventoryUI.baseGo.SetActive(false);
        mechanismUI.baseGo.SetActive(false);
    }

    public void CurrentSlotMarker (int slot) {
        if(!currentSlotMarkerIsInitialized) {
            currentSlotMarkerIsInitialized = true;

            currentSlotMarker.gameObject.SetActive(true);
            currentSlotMarker.SetAsLastSibling();
        }

        int newXPos = ((int) (slot - ((inventory.hotbarSize/2.0f) - 0.5f))) * 50;

        currentSlotMarker.anchoredPosition = new Vector2(newXPos, currentSlotMarker.anchoredPosition.y);
    }
}

public enum UISlotType {
    hotbar,
    inventory
}

public enum UIClass {
    hotbarUI,
    inventoryUI,
    mechanismUI
}

public enum UITypes {
    hotbarUI_hotslots,
    inventoryUI_hotSlots,
    inventoryUI_invSlots,
    mechanismUI_invSlots,
    mechanismUI_hotSlots
}

[System.Serializable]
public class BaseUI {
    public List<Slot> hotSlots = new List<Slot>();
    public List<Slot> invSlots = new List<Slot>();
    public GameObject baseGo;
}

[System.Serializable]
public class HotbarUI : BaseUI {
    public GameObject hotbarGo;

    public bool initialized = false;
}

[System.Serializable]
public class InventoryUI : BaseUI {
    public GameObject hotbarGo;
    public GameObject inventoryGo;
    
    public bool initialized = false;
}

[System.Serializable]
public class MechanismUI : BaseUI {
    public GameObject hotbarGo;
    public GameObject inventoryGo;
    public Text blockName;
    public Text blockDescription;

    public Component[] customUI;
    public int currentCustomUI = 0;

    public Component GetAndSetCustomUI (int numberInList) {
        TurnOffAllCustomUIBut((MonoBehaviour) customUI[numberInList]);

        currentCustomUI = numberInList;
        return customUI[numberInList];
    }

    public void TurnOffAllCustomUIBut (MonoBehaviour c) {
        foreach(MonoBehaviour cp in customUI) {
            cp.gameObject.SetActive(false);
            cp.enabled = false;
        }

        c.gameObject.SetActive(true);
        c.enabled = true;
    }

    public bool initialized = false;
}
