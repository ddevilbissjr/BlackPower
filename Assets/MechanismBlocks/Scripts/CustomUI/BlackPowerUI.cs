using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlackPowerUI : MonoBehaviour {

    public Mechanism item;

    public Text currentPower;
    public Text currentPower2;
    public Text currentSpeed;
    public Text connectedModules;
    public string currentPowerText;
    public string currentPowerText2;
    public string currentSpeedText;
    public string connectedModulesText;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (item != null) {
            SetCustomText(
                item.currentPower + " BP (BlackPower)",
                item.maxPower + " BP Power Level",
                (BlackPower.getPower(item.maxPower) / BlackPower.getPower(BlackPowerType.lowPower)) + " BP Per Second",
                ConnectedModules(item.SurroundingItems())
            );

            currentPower.text = currentPowerText;
            currentPower2.text = currentPowerText2;
            currentSpeed.text = currentSpeedText;
            connectedModules.text = connectedModulesText;
        }
	}

    public string ConnectedModules (List<Mechanism> mechs) {
        string str = null;

        if(mechs.Count > 0) {
            for(int i = 0; i < mechs.Count; i++) {
                if(i + 1 < mechs.Count) {
                    str += mechs[i].itemName + ", ";
                } else {
                    str += mechs[i].itemName;
                }
            }
        } else {
            str = "None found.";
        }

        return str;
    }

    public void SetCustomText (string cp, string cp2, string cs, string cm) {
        currentPowerText = cp;
        currentPowerText2 = cp2;
        currentSpeedText = cs;
        connectedModulesText = cm;
    }
}
