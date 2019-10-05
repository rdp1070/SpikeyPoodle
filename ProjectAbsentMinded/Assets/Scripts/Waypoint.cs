using UnityEngine;
using System.Collections;

public class Waypoint : MonoBehaviour
{
    [SerializeField]
    public float debugDrawRadius = 1.0F;

    public virtual void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, debugDrawRadius);
    }
}