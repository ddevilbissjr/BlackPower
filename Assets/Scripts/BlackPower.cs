using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackPower : MonoBehaviour {

    public static int getPower (BlackPowerType b) {
        return (int) b;
    }

    public static int setPower (int basePower, int increment) {
        return basePower + increment;
    }

}

public enum BlackPowerType {
    lowPower = 500,
    medPower = 1000,
    highPower = 2000
}

public enum BlackPowerItems {
    Item,
    StaticBlock,
    Mechanism,
    PowerSource,
    PowerGenerator
}
