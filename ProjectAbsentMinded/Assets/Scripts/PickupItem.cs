using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupItem : MonoBehaviour
{

    private bool hasItem;
    public float reachDistance = 2.5f;
    public Camera camera;
    public BaseItem heldItem;
    public Vector3 holdPosition;

    public bool HasItem { get => heldItem != null; }

    // Start is called before the first frame update
    void Start()
    {
        if (camera == null) {
            camera = Camera.main;
        }
    }

    // Update, but after the physics.
    void FixedUpdate()
    {
        CheckMouse();
        GenerateHoldPosition();
        if (HasItem) {
            if (Vector3.Distance(heldItem.transform.position, holdPosition) > reachDistance)
            {
                DropItem();
            }
            else
            {
                heldItem.MoveTowardsPos(holdPosition);
            }
        }
    }

    /// <summary>
    /// Uses raycasting to pick up an item
    /// </summary>
    void PickUpItem() {
        // Set the origin of the new ray to the Camera position
        // Set the direction of the new ray to the Camera forward
        Ray ray = new Ray(camera.transform.position, camera.transform.forward);
        RaycastHit raycastHit;
        // Cast the Ray for reachDistance
        var doesCollide = Physics.Raycast(ray, out raycastHit, reachDistance);
        // if it collides with an object of type BaseItem
        Debug.DrawRay(ray.origin, ray.direction * reachDistance, Color.yellow);
        
        if (doesCollide)
        {
            BaseItem item;
            if (raycastHit.collider.gameObject.TryGetComponent(out item)) {
                // then add item to the HeldItem
                item.isHeld = true;
                this.heldItem = item;
            }
        }

    }

    /// <summary>
    /// Resets item state, so it is no longer held
    /// </summary>
    void DropItem() {
        heldItem.isHeld = false;
        heldItem = null;
    }

    /// <summary>
    /// Mouse Handling Code
    /// </summary>
    private void CheckMouse()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (HasItem == false)
            {
                PickUpItem();
            }
        }
        else if (Input.GetMouseButtonUp(0)) {
            DropItem();
        }
    }


    /// <summary>
    /// Generating a box where you will hold things in front of you
    /// </summary>
    private void GenerateHoldPosition()
    {
        holdPosition = new Vector3(
            camera.transform.forward.x * reachDistance,
            camera.transform.forward.y * reachDistance,
            camera.transform.forward.z * reachDistance) + transform.position;
    }



}
