using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacingBlock : MonoBehaviour {

    public bool isOverlapping = false;

    private void Update () {
        IsOverlapping();
    }

    public void Placing_Block (Vector3 coords) {
        if (transform.parent.parent != null) {
            transform.parent.parent = null;
        }

        transform.parent.position = coords;
        transform.parent.rotation = Quaternion.identity;
    }

    public void IsOverlapping () {
        isOverlapping = false;
        Bounds bounds = GetComponent<Renderer>().bounds;
        Collider[] cols = Physics.OverlapSphere(transform.GetComponent<Renderer>().bounds.center, 0.1f);

        foreach (Collider col in cols) {
            if (col.gameObject != gameObject) {
                isOverlapping = true;
            }
        }

        Renderer r = GetComponent<MeshRenderer>();
        if (!r.enabled && !isOverlapping) {
            r.enabled = true;
        }

        if (r.enabled && isOverlapping) {
            r.enabled = false;
        }
    }

    public void Placing_Block_Disable () {
        GetComponent<MeshRenderer>().enabled = false;
    }
}
