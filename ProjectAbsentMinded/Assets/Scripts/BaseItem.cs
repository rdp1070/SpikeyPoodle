using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseItem : MonoBehaviour
{
    public bool isHeld = false;
    private float moveSpeed = .2f;
    private Rigidbody rigidbody;
    public string size = ATTR.SIZE.SMALL;

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


    // override object.Equals
    public override bool Equals(object obj)
    {
        BaseItem item = (BaseItem)obj;
        if (item == null || GetType() != item.GetType())
        {
            return false;
        }

        if (size == item.size) {
            // Make sure you add extra qualifiers here.
            return true;
        }
        return false;
    }

    /// <summary>
    /// Where the strings for the Attributes are defined.
    /// </summary>
    public static class ATTR {
        public static class SIZE {
            public static string SMALL = "SMALL";
            public static string MEDIUM = "MEDIUM";
            public static string LARGE = "LARGE";
        }
    }

}
