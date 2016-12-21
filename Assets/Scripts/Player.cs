using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class Player : MonoBehaviour {

    public PlayerMode playerMode = PlayerMode.survival;
    public Camera cam;
    public Inventory inventory;
    public PlacingBlock placingBlock;
    public Vector3 point;
    public Vector3 normal;
    public Vector3 comb;
    public Vector3 placeCoords;

    private float rayDistance = 5.0f;

    void Awake () {
        
	}
	
	void Update () {
        ItemInput();
        InteractionInput();
	}

    void ItemInput () {
        if(Input.GetKeyDown(KeyCode.Alpha1)) {
            SwitchItem(0);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2)) {
            SwitchItem(1);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3)) {
            SwitchItem(2);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4)) {
            SwitchItem(3);
        }
        if (Input.GetKeyDown(KeyCode.Alpha5)) {
            SwitchItem(4);
        }
        if (Input.GetKeyDown(KeyCode.Alpha6)) {
            SwitchItem(5);
        }
        if (Input.GetKeyDown(KeyCode.Alpha7)) {
            SwitchItem(6);
        }
        if (Input.GetKeyDown(KeyCode.Alpha8)) {
            SwitchItem(7);
        }
        if (Input.GetKeyDown(KeyCode.Alpha9)) {
            SwitchItem(8);
        }

        if(Input.GetKeyDown(KeyCode.E)) {
            bool openedOrClosed = !inventory.OpenUI(UISlotType.inventory);

            GetComponent<FirstPersonController>().m_MouseLook.SetCursorLock(openedOrClosed);
            GetComponent<FirstPersonController>().enabled = openedOrClosed;
            GetComponent<CharacterController>().enabled = openedOrClosed;
        }

        if(Input.GetAxis("Mouse ScrollWheel") > 0f) { //If mouse-scrolled up.
            if(inventory.GetCurrentItemNum() + 1 < inventory.hotbar.Count) { //If mouse-scrolled up is not going to be greater than inventory count. (null)
                SwitchItem(inventory.GetCurrentItemNum() + 1);
            } else {
                SwitchItem(0);
            }
        } else if (Input.GetAxis("Mouse ScrollWheel") < 0f) { //If mouse-scrolled down.
            if (inventory.GetCurrentItemNum() > 0) { //If mouse-scrolled down is not going to be negative. (null)
                SwitchItem(inventory.GetCurrentItemNum() - 1);
            } else {
                SwitchItem(inventory.hotbar.Count - 1);
            }
        }
    }

    void InteractionInput () {
        Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, rayDistance)) { //If the raycast does hit something in the rayDistance.
            Debug.DrawRay(ray.origin, ray.direction * Vector3.Distance(ray.origin, hit.point), Color.green);
            Debug.DrawRay(hit.point, hit.normal/2, Color.green);

            point = new Vector3 (

                hit.point.x,
                hit.point.y,
                hit.point.z

            );

            normal = new Vector3(

                hit.normal.x,
                hit.normal.y,
                hit.normal.z

            );

            comb = new Vector3(

                hit.point.x + hit.normal.x/2,
                hit.point.y + hit.normal.y/2,
                hit.point.z + hit.normal.z/2

            );

            placeCoords = HitCoords(hit.point, hit.normal);
            placingBlock.Placing_Block(placeCoords);

            GameObject hitGo = hit.collider.gameObject;
            if (hitGo.GetComponent<Item>() != null) { //If the object hit has an Item-inherited component.
                Item item = hitGo.GetComponent<Item>();

                if (item.GetType().IsSubclassOf(typeof(Mechanism))) { // If type is equal to mechanism.
                    if (Input.GetKey(KeyCode.LeftControl) && !placingBlock.isOverlapping) { // If the player is crouching and the placing block is not overlapping when trying to place a block with a type Mechanism.

                        if (Input.GetMouseButtonDown(1)) {
                            PlaceBlock();
                        }
                    } else if (!Input.GetKey(KeyCode.LeftControl)) { // If the player not crouching when trying to place a block with a type Mechanism.
                        placingBlock.Placing_Block_Disable();

                        if (Input.GetMouseButtonDown(1)) {
                            UseItem(item);
                        }
                    }
                } else { // If is not equal to mechanism.

                    if (Input.GetMouseButtonDown(1)) {
                        PlaceBlock();
                    }
                }
                
            } else { //If the object hit does not have an Item-inherited component.

                if (Input.GetMouseButtonDown(1)) {
                    PlaceBlock();
                }
            }

        } else { //If the raycast does not hit something in the rayDistance.
            placingBlock.Placing_Block_Disable();
            Debug.DrawRay(ray.origin, ray.direction * rayDistance, Color.green);
        }
    }

    void PlaceBlock () {
        inventory.PlaceBlock(playerMode, placeCoords);
    }

    void SwitchItem (int num) {
        inventory.SetCurrentItem(num);
    }

    void UseItem (Item item) {
        Debug.Log("Using " + item.name);
    }

    Vector3 HitCoords (Vector3 point, Vector3 normal) {
        float x1, y1, z1;

        Vector3 combined = new Vector3();

        if (point.x < 0) {

            combined.x = point.x - Mathf.Abs(normal.x / 2) - 0.5f;

        } else if (point.x > 0) {

            combined.x = point.x + Mathf.Abs(normal.x / 2) + 0.5f;

        }

        if (point.y < 0) {

            combined.y = point.y - Mathf.Abs(normal.y / 2) - 0.5f;

        } else if (point.y > 0) {

            combined.y = point.y + Mathf.Abs(normal.y / 2) + 0.5f;

        }

        if (point.z < 0) {

            combined.z = point.z - Mathf.Abs(normal.z / 2) - 0.5f;

        } else if (point.z > 0) {

            combined.z = point.z + Mathf.Abs(normal.z / 2) + 0.5f;

        }

        x1 = (int) (combined.x) / 1.0f;
        y1 = (int) (combined.y) / 1.0f;
        z1 = (int) (combined.z) / 1.0f;

        return new Vector3(x1, y1, z1);
    }
}

public enum PlayerMode {
    survival,
    creative
}
