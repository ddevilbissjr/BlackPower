using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour {

    public List<Item> hotbar = new List<Item>();
    int hotbarSize = 9;
    public List<Item> inventory = new List<Item>();
    int inventorySize = 27;

    public UI UI;

    public bool testBool = false;

    public Item currentItem;
    public int currentSlot = 0;

    void Awake () {
        InstantiateUI();

        InitializeUI(UIClass.hotbarUI);
        InitializeUI(UIClass.inventoryUI);

        foreach (Transform child in transform.FindChild("ToBePlacedIntoInventories")) {
            SetIntoLists(child.GetComponent<Item>());
        }

        SetCurrentItem(currentSlot);
    } 

    void Start () {
		
	}
	
	void Update () {

        if (Input.GetKeyDown(KeyCode.F)) {
            if (UI.mechanismUI.initialized) {
                UI.UISwitchTo(UIClass.mechanismUI);
            } else {
                InitializeUI(UIClass.mechanismUI);
                UI.UISwitchTo(UIClass.mechanismUI);
            }
        }

        foreach (Item bar in hotbar) {
            if (bar != null) {
                bar.transform.parent = transform.FindChild("Hotbar");
            }
        }

        foreach (Item inv in inventory) {
            if (inv != null) {
                inv.transform.parent = transform.FindChild("Inventory");
            }
        }
    }

    void InstantiateUI () {
        UI = (Instantiate(Resources.Load("UI/UI")) as GameObject).GetComponent<UI>();

        UI.inventory = this;

        if (hotbar.Count < hotbarSize) {
            for (int i = hotbar.Count - 1; i < hotbarSize - 1; i++) {
                hotbar.Add(null);
            }
        }

        if (inventory.Count < inventorySize) {
            for (int i = inventory.Count - 1; i < inventorySize - 1; i++) {
                inventory.Add(null);
            }
        }
    }

    void InitializeUI (UIClass type) {
        for(int i = 0; i < hotbar.Count; i++) {
            Item bar = hotbar[i];

            switch (type) {
                case UIClass.hotbarUI:
                    UI.SetSlotFor(UISlotType.hotbar, UITypes.hotbarUI_hotslots, i);
                    break;
                case UIClass.inventoryUI:
                    UI.SetSlotFor(UISlotType.hotbar, UITypes.inventoryUI_hotSlots, i);
                    break;
                case UIClass.mechanismUI:
                    UI.SetSlotFor(UISlotType.hotbar, UITypes.mechanismUI_hotSlots, i);
                    break;
            }
        }

        for(int i = 0; i < inventory.Count; i++) {
            Item inv = inventory[i];

            switch (type) {
                case UIClass.inventoryUI:
                    UI.SetSlotFor(UISlotType.inventory, UITypes.inventoryUI_invSlots, i);
                    break;
                case UIClass.mechanismUI:
                    UI.SetSlotFor(UISlotType.inventory, UITypes.mechanismUI_invSlots, i);
                    break;
            }
        }
        
        switch (type) {
            case UIClass.hotbarUI:
                UI.hotbarUI.initialized = true;
                break;
            case UIClass.inventoryUI:
                UI.inventoryUI.initialized = true;
                break;
            case UIClass.mechanismUI:
                UI.mechanismUI.initialized = true;
                break;
            default:
                Debug.Log("None set for " + type.ToString());
                break;
        }
    }

    public bool OpenUI (UISlotType type) {
        bool openOrClosed = false;

        switch (type) {
            case UISlotType.inventory:
                if (UI.currentUIEnabled != UIClass.hotbarUI) {
                    openOrClosed = false;
                    UI.UISwitchTo(UIClass.hotbarUI);
                } else {
                    openOrClosed = true;
                    UI.UISwitchTo(UIClass.inventoryUI);
                }
                break;
        }

        return openOrClosed;
    }
    
    public Item GetCurrentItem () {
        return hotbar[currentSlot];
    }

    public void SetCurrentItem (int id) {
        currentSlot = id;
        currentItem = hotbar[id];
        DisableAllItemsBut(id);
    }

    public int GetCurrentItemNum () {
        return currentSlot;
    }

    public void DisableAllItemsBut (int id) {
        foreach (Item item in hotbar) {
            if(item != null) {
                item.gameObject.SetActive(false);
                item.GetComponent<Collider>().enabled = false;
            }
        }
        if(hotbar[id] != null) {
            hotbar[id].gameObject.SetActive(true);
        }
    }

    public void PlaceBlock (PlayerMode mode, Vector3 coords) {
        Debug.Log("Placing item in slot " + currentSlot + ".");

        if (currentItem != null) { //Check if its not null.
            GameObject clone = null;
            if (mode == PlayerMode.survival) {
                clone = CreateBlock(currentItem.gameObject, coords);

                if (currentItem.getHowMany() > 1) { //Check if its the last one in inventory. If not, keep copy in inventory.
                    currentItem.setHowMany(HowMany.increment, -1);
                } else { //If so, use last one.
                    RemoveForPlacing(UISlotType.hotbar);
                }

            } else if (mode == PlayerMode.creative) {
                clone = CreateBlock(currentItem.gameObject, coords);
            }
            Debug.Log("Placed " + clone.GetComponent<Item>().getItemName() + " at coordinates " + clone.transform.position);

        } else {
            Debug.Log("Placed nothing.");
        }
    }

    public GameObject CreateBlock (GameObject go, Vector3 coordinates) {
        GameObject clone = Instantiate(currentItem.gameObject, coordinates, Quaternion.identity);

        clone.SetActive(true);
        clone.GetComponent<Item>().setInInventory(false);
        clone.GetComponent<Item>().setHowMany(HowMany.totalValue, 1);
        clone.GetComponent<MeshRenderer>().enabled = true;
        clone.GetComponent<Collider>().enabled = true;

        return clone;
    }

    public void RemoveForPlacing (UISlotType type) {
        UI.SetIconTo(type, null, GetCurrentItemNum());

        RemoveFromInventory(type, GetCurrentItemNum(), currentItem.gameObject);

        currentItem = hotbar[GetCurrentItemNum()];
    }

    public void RemoveFromInventory (UISlotType type, int slotNumber, GameObject toDestroy) {

        switch (type) {
            case UISlotType.hotbar:
                hotbar[slotNumber] = null;
                break;
            case UISlotType.inventory:
                inventory[slotNumber] = null;
                break;
        }

        Destroy(toDestroy);
    }

    public void MoveInInventory (UISlotType previousType, int previousNumber, Item previousItem, UISlotType nextType, int nextNumber, Item nextItem) {

        switch (previousType) {
            case UISlotType.hotbar:
                hotbar[previousNumber] = previousItem;
                break;
            case UISlotType.inventory:
                inventory[previousNumber] = previousItem;
                break;
        }

        switch (nextType) {
            case UISlotType.hotbar:
                hotbar[nextNumber] = nextItem;
                break;
            case UISlotType.inventory:
                inventory[nextNumber] = nextItem;
                break;
        }

        currentItem = hotbar[GetCurrentItemNum()];

    }

    public void SetIntoInventoriesShortcut (UISlotType type, Item item, int slot) {
        switch(type) {
            case UISlotType.hotbar:
                hotbar[slot] = item;

                if(UI.hotbarUI.initialized) {
                    UI.SetIconTo(type, item, slot);
                }

                break;
            case UISlotType.inventory:
                inventory[slot] = item;

                if(UI.inventoryUI.initialized) {
                    UI.SetIconTo(type, item, slot);
                }

                break;
        }
    }

    void SetIntoLists (Item item) {
        List<Item> hotRemove = new List<Item>();
        List<Item> invRemove = new List<Item>();

        item.GetComponent<MeshRenderer>().enabled = false;

        int curHotNum = -1;
        for(int stuff = 0; stuff < hotbar.Count; stuff++) {
            if(hotbar[stuff] == null) {
                curHotNum = stuff;
                break;
            }
        }

        if (curHotNum == -1) {
            curHotNum = hotbarSize;
        }

        int curInvNum = -1;
        for (int stuff = 0; stuff < inventory.Count; stuff++) {
            if (inventory[stuff] == null) {
                curInvNum = stuff;
                break;
            }
        }

        if (curInvNum == -1) {
            curInvNum = inventorySize;
        }

        //Debug.Log(curHotNum + ", " + curInvNum);

        Item hotDuplicate = hotbar.Find(myItem => myItem != null && myItem.getItemName() == item.getItemName() && myItem.getHowMany() < myItem.getStackSize());
        Item invDuplicate = inventory.Find(myItem => myItem != null && myItem.getItemName() == item.getItemName() && myItem.getHowMany() < myItem.getStackSize());

        if (hotDuplicate == null && invDuplicate == null && item.getStackable()) {
            if (curHotNum < hotbarSize) {
                SetIntoInventoriesShortcut(UISlotType.hotbar, item, curHotNum);
            } else {
                if (curInvNum < inventorySize) {
                    SetIntoInventoriesShortcut(UISlotType.inventory, item, curInvNum);
                } else {
                    // Throw away item.
                }
            }
        } else if (hotDuplicate != null) {
            if (hotDuplicate.getHowMany() < hotDuplicate.getStackSize()) {
                int combined = item.getHowMany() + hotDuplicate.getHowMany();

                if (combined <= hotDuplicate.getStackSize()) {
                    hotDuplicate.setHowMany(HowMany.totalValue, combined);
                    hotRemove.Add(item);
                } else {
                    int difference = Mathf.Abs(item.getHowMany() - (hotDuplicate.getStackSize() - hotDuplicate.getHowMany()));

                    hotDuplicate.setHowMany(HowMany.totalValue, hotDuplicate.getStackSize());
                    if (difference > 0) {
                        item.setHowMany(HowMany.totalValue, difference);
                        if (curHotNum < hotbarSize) {
                            SetIntoInventoriesShortcut(UISlotType.hotbar, item, curHotNum);
                        } else {
                            SetIntoInventoriesShortcut(UISlotType.inventory, item, curInvNum);
                        }
                    } else {
                        if (curHotNum < hotbarSize) {
                            SetIntoInventoriesShortcut(UISlotType.hotbar, item, curHotNum);
                        } else {
                            SetIntoInventoriesShortcut(UISlotType.inventory, item, curInvNum);
                        }
                    }
                }

            } else {
                if (curHotNum < hotbarSize) {
                    SetIntoInventoriesShortcut(UISlotType.hotbar, item, curHotNum);
                } else {
                    SetIntoInventoriesShortcut(UISlotType.inventory, item, curInvNum);
                }
            }

        } else if (invDuplicate != null) {
            if (invDuplicate.getHowMany() < invDuplicate.getStackSize()) {
                int combined = item.getHowMany() + invDuplicate.getHowMany();

                if (combined <= invDuplicate.getStackSize()) {
                    invDuplicate.setHowMany(HowMany.totalValue, combined);
                    invRemove.Add(item);
                } else {
                    int difference = Mathf.Abs(item.getHowMany() - (invDuplicate.getStackSize() - invDuplicate.getHowMany()));

                    invDuplicate.setHowMany(HowMany.totalValue, invDuplicate.getStackSize());
                    if (difference > 0) {
                        item.setHowMany(HowMany.totalValue, difference);
                        SetIntoInventoriesShortcut(UISlotType.inventory, item, curInvNum);
                    } else {
                        SetIntoInventoriesShortcut(UISlotType.inventory, item, curInvNum);
                    }
                }

            } else {
                SetIntoInventoriesShortcut(UISlotType.inventory, item, curInvNum);
            }

        } else if (!item.getStackable()) {
            if (curHotNum < hotbarSize) {
                SetIntoInventoriesShortcut(UISlotType.hotbar, item, curHotNum);
            } else {
                SetIntoInventoriesShortcut(UISlotType.inventory, item, curInvNum);
            }
        }

        foreach (Item idk in hotRemove) {
            Destroy(idk.gameObject);
        }

        foreach (Item idk in invRemove) {
            Destroy(idk.gameObject);
        }
    }
}
