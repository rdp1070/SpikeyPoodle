using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;
using UnityEngine.Events;

public class Player : MonoBehaviour
{
    public PickupItem pickupItem;
    public FirstPersonController firstPersonController;
    public GameObject playerBody;
    public float ogWalkSpeed;
    public float ogRunSpeed;
    public float ogMouseLookSpeed;
    public bool isCrouching = false;
    private float speedModifier = .9f;
    public UnityEvent crouched = new UnityEvent();
    public UnityEvent stand = new UnityEvent();



    private void Start()
    {
        pickupItem = GetComponentInChildren<PickupItem>();
        firstPersonController = GetComponentInChildren<FirstPersonController>();
        if (firstPersonController != null) {
            playerBody = firstPersonController.gameObject;
            ogWalkSpeed = firstPersonController.m_WalkSpeed; // equal to original walk speed.
            ogRunSpeed = firstPersonController.m_RunSpeed;
            ogMouseLookSpeed = firstPersonController.m_MouseLook.XSensitivity;
        }
    }

    private void Update()
    {
        ApplyWeight();
        if (Input.GetKeyUp(KeyCode.C)) {
            if (isCrouching)
                StopCrouch();
            else
                Crouch();
        }
        if (Input.GetKeyDown(KeyCode.LeftShift) && isCrouching) {
            StopCrouch();
        }
    }

    private void Crouch()
    {
        isCrouching = true;
        crouched.Invoke();
        if (playerBody != null) {
            playerBody.transform.localScale = new Vector3(1f, .75f, 1f);
        }
    }

    private void StopCrouch() {
        isCrouching = false;
        stand.Invoke();
        if (playerBody != null)
        { 
            playerBody.transform.localScale = new Vector3(1f, 1f, 1f);
        }
    }

    private void ApplyWeight()
    {
        var mod = 1f;
        if (isCrouching)
            mod = speedModifier;

        if (pickupItem.heldItem != null)
        {
            
            firstPersonController.m_RunSpeed = (ogRunSpeed * pickupItem.heldItem.weight) * mod;
            firstPersonController.m_WalkSpeed = (ogWalkSpeed * pickupItem.heldItem.weight) * mod;
            firstPersonController.m_MouseLook.XSensitivity = ogMouseLookSpeed * pickupItem.heldItem.weight;
            firstPersonController.m_MouseLook.YSensitivity = ogMouseLookSpeed * pickupItem.heldItem.weight;
        }
        else
        {
            firstPersonController.m_RunSpeed = ogRunSpeed * mod;
            firstPersonController.m_WalkSpeed = ogWalkSpeed * mod;
            firstPersonController.m_MouseLook.XSensitivity = ogMouseLookSpeed;
            firstPersonController.m_MouseLook.YSensitivity = ogMouseLookSpeed;
        }
    }
}
