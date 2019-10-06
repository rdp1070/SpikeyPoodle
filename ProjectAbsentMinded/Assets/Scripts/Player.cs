using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class Player : MonoBehaviour
{
    public PickupItem pickupItem;
    public FirstPersonController firstPersonController;
    public float ogWalkSpeed;
    public float ogRunSpeed;
    public float ogMouseLookSpeed;

    private void Start()
    {
        pickupItem = GetComponentInChildren<PickupItem>();
        firstPersonController = GetComponentInChildren<FirstPersonController>();
        if (firstPersonController != null) {
            ogWalkSpeed = firstPersonController.m_WalkSpeed; // equal to original walk speed.
            ogRunSpeed = firstPersonController.m_RunSpeed;
            ogMouseLookSpeed = firstPersonController.m_MouseLook.XSensitivity;
        }
    }

    private void Update()
    {
        ApplyWeight();
    }

    private void ApplyWeight()
    {
        if (pickupItem.heldItem != null)
        {
            firstPersonController.m_RunSpeed = ogRunSpeed * pickupItem.heldItem.weight;
            firstPersonController.m_WalkSpeed = ogWalkSpeed * pickupItem.heldItem.weight;
            firstPersonController.m_MouseLook.XSensitivity = ogMouseLookSpeed * pickupItem.heldItem.weight;
            firstPersonController.m_MouseLook.YSensitivity = ogMouseLookSpeed * pickupItem.heldItem.weight;
        }
        else
        {
            firstPersonController.m_RunSpeed = ogRunSpeed;
            firstPersonController.m_WalkSpeed = ogWalkSpeed;
            firstPersonController.m_MouseLook.XSensitivity = ogMouseLookSpeed;
            firstPersonController.m_MouseLook.YSensitivity = ogMouseLookSpeed;
        }
    }
}
