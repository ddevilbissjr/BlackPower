using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour {

    public UIClass currentUIEnabled = UIClass.hotbarUI;

    public HotbarUI hotbarUI;
    public InventoryUI inventoryUI;
    public MechanismUI mechanismUI;

	public void SetSlotFor (UITypes type) {
        switch (type) {
            case UITypes.hotbarUI_hotslots:
                hotbarUI.hotSlots.Add(CreateSlot(hotbarUI.hotbarGo.transform));
                break;
            case UITypes.inventoryUI_hotSlots:
                inventoryUI.hotSlots.Add(CreateSlot(inventoryUI.hotbarGo.transform));
                break;
            case UITypes.inventoryUI_invSlots:
                inventoryUI.invSlots.Add(CreateSlot(inventoryUI.inventoryGo.transform));
                break;
            case UITypes.mechanismUI_hotSlots:
                mechanismUI.hotSlots.Add(CreateSlot(mechanismUI.hotbarGo.transform));
                break;
            case UITypes.mechanismUI_invSlots:
                mechanismUI.invSlots.Add(CreateSlot(mechanismUI.inventoryGo.transform));
                break;
            default:
                Debug.Log("None set for " + type.ToString());
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

    public Slot CreateSlot (Transform soonToBeParent) {
        GameObject clone = Instantiate(Resources.Load("UI/Slot"), soonToBeParent) as GameObject;
        return clone.GetComponent<Slot>();
    }

    public ItemData CreateSlotPlaceholder (Item item, Slot slot) {
        GameObject clone = Instantiate(Resources.Load("UI/SlotPlaceholder"), slot.transform) as GameObject;
        ItemData data = clone.GetComponent<ItemData>();

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
public class HotbarUI {
    public GameObject baseGo;
    public GameObject hotbarGo;
    public List<Slot> hotSlots = new List<Slot>();
    public bool initialized = false;
}

[System.Serializable]
public class InventoryUI {
    public GameObject baseGo;
    public GameObject hotbarGo;
    public List<Slot> hotSlots = new List<Slot>();
    public GameObject inventoryGo;
    public List<Slot> invSlots = new List<Slot>();
    public bool initialized = false;
}

[System.Serializable]
public class MechanismUI {
    public Text blockName;
    public Text blockDescription;
    public GameObject baseGo;
    public GameObject hotbarGo;
    public List<Slot> hotSlots = new List<Slot>();
    public GameObject inventoryGo;
    public List<Slot> invSlots = new List<Slot>();
    public bool initialized = false;
}
