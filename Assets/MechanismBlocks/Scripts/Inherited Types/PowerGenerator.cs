using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerGenerator : Mechanism {

    private void Reset () {
        type = BlackPowerItems.PowerGenerator;
    }

    // Use this for initialization
    void Start () {
        currentPower = BlackPower.getPower(maxPower) / BlackPower.getPower(BlackPowerType.lowPower);
    }
	
	// Update is called once per frame
	void Update () {
        if (!inInventory) {
            surroundingMechanisms = SurroundingItems();
        }
    }
}
