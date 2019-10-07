using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseItem : MonoBehaviour
{
    public bool isHeld = false;
    private float moveSpeed = .2f;
    private Rigidbody rigidbody;
    public string size = ATTR.SIZE.SMALL;
    public float weight = ATTR.WEIGHT.LIGHT;
    public Color color = ATTR.COLOR.BLUE;
    public string shape = ATTR.SHAPE.CUBE;

    public void Init()
    {
        PickShape();
        rigidbody = this.gameObject.GetComponentInChildren<Rigidbody>();
        gameObject.GetComponentInChildren<Renderer>().material.color = color;
        AdjustScale();
    }

    private void PickShape()
    {
        transform.Find(shape).gameObject.SetActive(true);
    }

    private void AdjustScale()
    {
        if (size == ATTR.SIZE.SMALL)
        {
            transform.localScale = new Vector3(.25f, .25f, .25f);
        }
        else if (size == ATTR.SIZE.MEDIUM)
        {
            transform.localScale = new Vector3(.5f, .5f, .5f);
        }
        else
        {
            transform.localScale = new Vector3(1f, 1f, 1f);
        }

    }

    /// <summary>
    /// Move the Item, towards the Position passed in over time
    /// </summary>
    /// <param name="holderPos"></param>
    public void MoveTowardsPos(Vector3 holderPos)
    {
        transform.position = Vector3.MoveTowards(this.transform.position, holderPos, moveSpeed);
    }

    private void FixedUpdate()
    {
        if (isHeld)
        {
            rigidbody.useGravity = false;
        }
        else
        {
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

        if (size == item.size && shape == item.shape && weight == item.weight && color == item.color)
        {
            return true;
        }

        return false;
    }

    /// <summary>
    /// Where the strings for the Attributes are defined.
    /// </summary>
    public static class ATTR
    {
        public static class SIZE
        {
            public static string SMALL = "SMALL";
            public static string MEDIUM = "MEDIUM";
            public static string LARGE = "LARGE";
            public static List<string> sizes = new List<string>{
                LARGE, MEDIUM, SMALL
            };
        }
        public static class SHAPE
        {
            public static string CUBE = "CUBE";
            public static string SPHERE = "SPHERE";
            public static string CYLINDER = "CYLINDER";
            public static string CAPSULE = "CAPSULE";
            public static List<string> shapes = new List<string>{
                CUBE, SPHERE, CYLINDER, CAPSULE
            };
        }
        public static class WEIGHT
        {
            public static float LIGHT = 1f;
            public static float MEDIUM = .75f;
            public static float HEAVY = .5f;
            public static List<float> weights = new List<float>{
                LIGHT, MEDIUM, HEAVY
            };
        }
        public static class COLOR
        {
            public static Color PURPLE = Color.magenta;
            public static Color YELLOW = Color.yellow;
            public static Color BLUE = Color.blue;
            public static Color RED = Color.red;
            public static List<Color> colors = new List<Color>{
                PURPLE, YELLOW, BLUE, RED
            };
        }
    }

}
