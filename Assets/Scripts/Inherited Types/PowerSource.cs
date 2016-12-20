using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PowerSource : Mechanism {

    private void Reset () {
        type = BlackPowerItems.PowerSource;
    }

    public List<PowerGenerator> generators = new List<PowerGenerator>();

    public bool generatorAttached = false;
    bool isTiming = true;

    static float startTimer = 1.0f;
    float timer = startTimer;

    // Use this for initialization
    void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
        if (!inInventory) {
            bool checkIfGeneratorsPresent = false;
            surroundingMechanisms = SurroundingItems();

            foreach(Mechanism m in surroundingMechanisms) {
                if (m.type == BlackPowerItems.PowerGenerator) {
                    if (!checkIfGeneratorsPresent) {
                        checkIfGeneratorsPresent = true;
                    }

                    if (!generators.Contains((PowerGenerator) m)) {
                        generators.Add((PowerGenerator) m);
                    }
                }
            }

            generators = generators.Where(item => item != null).ToList();

            generatorAttached = checkIfGeneratorsPresent;
        }

        if (generatorAttached && !inInventory) { Timer(); } else { isTiming = false; timer = startTimer; }
    }

    void Timer () {
        if (!isTiming) {
            timer = startTimer;
            isTiming = true;
        } else {
            if (timer > 0) {
                timer -= Time.deltaTime;
            } else {
                AddPower();
                isTiming = false;
            }
        }
    }

    void AddPower () {
        int totalIncrementPower = 0;

        foreach(PowerGenerator pg in generators) {
            totalIncrementPower += BlackPower.getPower(pg.maxPower) / BlackPower.getPower(BlackPowerType.lowPower);
        }

        if (currentPower + totalIncrementPower < BlackPower.getPower(maxPower)) {
            currentPower = BlackPower.setPower(currentPower, totalIncrementPower);
        } else {
            currentPower = BlackPower.getPower(maxPower);
        }
    }
}
