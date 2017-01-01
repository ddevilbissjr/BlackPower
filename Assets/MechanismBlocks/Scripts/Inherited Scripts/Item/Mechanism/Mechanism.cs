using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Mechanism : Item {

    public string itemDescription;
    public List<Mechanism> surroundingMechanisms;

    void Update () {
        if(!inInventory) {
            surroundingMechanisms = SurroundingMechanisms();
        }
    } 
    
    public List<Mechanism> SurroundingMechanisms () {
        List<Mechanism> blocks = new List<Mechanism>();
        Vector3[] sidesToCheckFor = {
            Vector3.right,
            Vector3.left,
            Vector3.down,
            Vector3.up,
            Vector3.forward,
            Vector3.back
        };

        for (int i = 0; i < sidesToCheckFor.Length; i++) {
            Vector3 coord = transform.position + sidesToCheckFor[i];
            Collider[] hitColliders = Physics.OverlapSphere(coord, 0.1f);

            foreach (Collider col in hitColliders) {
                if (col.GetComponent<Mechanism>() != null && col.gameObject != gameObject) {
                    blocks.Add(col.GetComponent<Mechanism>());
                }
            }
        }

        return blocks;
    }

}
