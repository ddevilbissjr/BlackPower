using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerGenerator : BlackPower {

    private void Reset () {
        type = ItemTypes.PowerGenerator;
    }

    // Use this for initialization
    void Start () {
        currentPower = getPower(maxPower) / getPower(BlackPowerType.lowPower);
    }
	
	// Update is called once per frame
	void Update () {
        if (!inInventory) {
            surroundingMechanisms = SurroundingMechanisms();
        }
    }
}
