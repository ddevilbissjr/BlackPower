using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackPower : Mechanism {

    public BlackPowerType maxPower = BlackPowerType.lowPower;
    public int currentPower;

    public static int getPower (BlackPowerType b) {
        return (int) b;
    }

    public int getCurrentPower () {
        return currentPower;
    }

    public void setCurrentPower (HowMany type, int bp) {
        switch (type) {
            case HowMany.totalValue:
                currentPower = bp;
                break;
            case HowMany.increment:
                currentPower += bp;
                break;
        }
    }

}

public enum BlackPowerType {
    lowPower = 500,
    medPower = 1000,
    highPower = 2000
}
