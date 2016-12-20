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

        SetCurrentItem(currentSlot);
    } 

    void Start () {
		
	}
	
	void Update () {
        if(Input.GetKeyDown(KeyCode.E)) {
            if(UI.currentUIEnabled != UIClass.hotbarUI) {
                UI.UISwitchTo(UIClass.hotbarUI);
            } else {
                UI.UISwitchTo(UIClass.inventoryUI);
            }
        }

        if (Input.GetKeyDown(KeyCode.F)) {
            if (UI.mechanismUI.initialized) {
                UI.UISwitchTo(UIClass.mechanismUI);
            } else {
                InitializeUI(UIClass.mechanismUI);
                UI.UISwitchTo(UIClass.mechanismUI);
            }
        }

        if(testBool) {
            testBool = false;
            foreach (Transform child in transform.FindChild("ToBePlacedIntoInventories")) {
                SetIntoLists(child.GetComponent<Item>());
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
    }

    void InstantiateUI () {
        UI = (Instantiate(Resources.Load("UI/UI")) as GameObject).GetComponent<UI>();

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
        foreach (Item bar in hotbar) {
            switch (type) {
                case UIClass.hotbarUI:
                    UI.SetSlotFor(UITypes.hotbarUI_hotslots);
                    break;
                case UIClass.inventoryUI:
                    UI.SetSlotFor(UITypes.inventoryUI_hotSlots);
                    break;
                case UIClass.mechanismUI:
                    UI.SetSlotFor(UITypes.mechanismUI_hotSlots);
                    break;
            }
        }

        foreach (Item inv in inventory) {
            switch (type) {
                case UIClass.inventoryUI:
                    UI.SetSlotFor(UITypes.inventoryUI_invSlots);
                    break;
                case UIClass.mechanismUI:
                    UI.SetSlotFor(UITypes.mechanismUI_invSlots);
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

        hotbar[GetCurrentItemNum()] = null;
        Destroy(currentItem.gameObject);
        currentItem = hotbar[GetCurrentItemNum()];
    }

    public void SetIntoInventoriesShortcut (UISlotType type, Item item, int slot) {
        switch(type) {
            case UISlotType.hotbar:
                hotbar[slot] = item;

                if(UI.hotbarUI.initialized) {
                    Debug.Log("1");
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
