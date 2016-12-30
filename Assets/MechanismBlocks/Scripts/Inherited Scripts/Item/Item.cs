using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Item : MonoBehaviour {

    public ItemTypes type;
    public string itemName;
    public int howMany = 1;
    public int stackSize = 64;
    public bool inInventory = true;
    public bool stackable = true;

    public string getItemName () {
        return itemName;
    }

    public void setItemName (string newName) {
        itemName = newName;
    }

    public int getHowMany () {
        return howMany;
    }

    public void setHowMany (HowMany type, int value) {
        switch (type) {
            case HowMany.totalValue:
                howMany = value;
                break;
            case HowMany.increment:
                howMany += value;
                break;
        }
    }

    public bool getInInventory () {
        return inInventory;
    }

    public void setInInventory (bool b) {
        inInventory = b;
    }

    public int getStackSize () {
        return stackSize;
    }

    public void setStackSize (int size) {
        stackSize = size;
    }

    public bool getStackable () {
        return stackable;
    }

    public void setStackable (bool b) {
        stackable = b;
    }
}

public enum HowMany {
    totalValue,
    increment
}

public enum ItemTypes {
    Item,
    StaticBlock,
    Mechanism,
    PowerSource,
    PowerGenerator
}
