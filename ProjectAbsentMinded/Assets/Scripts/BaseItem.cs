using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseItem : MonoBehaviour
{
    public bool isHeld = false;
    private float moveSpeed = .2f;
    private Rigidbody rigidbody;

    private void Start()
    {
        rigidbody = this.gameObject.GetComponent<Rigidbody>();
    }

    /// <summary>
    /// Move the Item, towards the Position passed in over time
    /// </summary>
    /// <param name="holderPos"></param>
    public void MoveTowardsPos( Vector3 holderPos) {
        transform.position = Vector3.MoveTowards(this.transform.position, holderPos, moveSpeed);
    }

    private void FixedUpdate()
    {
        if (isHeld)
        {
            rigidbody.useGravity = false;
        }
        else {
            rigidbody.useGravity = true;
        }
    }
}
