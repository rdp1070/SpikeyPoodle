using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class PlayerDetection : MonoBehaviour
{
    public float viewDistance = 20f;
    public float viewSpread = .45f;
    private List<Ray> searchRays = new List<Ray>();
    private List<Vector3> searchVectors = new List<Vector3>();
    public UnityEvent sawPlayer = new UnityEvent();
    public UnityEvent sawPlayerStealing = new UnityEvent();

    void Update()
    {
        PopulateVectors();
        GenerateRays();
        LookForPlayer();
    }

    private void PopulateVectors()
    {
        searchVectors = new List<Vector3>
        {
            new Vector3(transform.forward.x, transform.forward.y, transform.forward.z),
            new Vector3(transform.forward.x, transform.forward.y + viewSpread, transform.forward.z),
            new Vector3(transform.forward.x, transform.forward.y + viewSpread / 2, transform.forward.z),
            new Vector3(transform.forward.x, transform.forward.y - viewSpread, transform.forward.z),
            new Vector3(transform.forward.x, transform.forward.y - viewSpread / 2, transform.forward.z),
            new Vector3(transform.forward.x + viewSpread, transform.forward.y, transform.forward.z),
            new Vector3(transform.forward.x + viewSpread / 2, transform.forward.y, transform.forward.z),
            new Vector3(transform.forward.x - viewSpread, transform.forward.y, transform.forward.z),
            new Vector3(transform.forward.x - viewSpread / 2, transform.forward.y, transform.forward.z)
        };
    }

    private void GenerateRays()
    {
        searchRays = new List<Ray>();
        foreach (Vector3 v in searchVectors) {
            searchRays.Add(new Ray(transform.position, v));
        }
    }

    void LookForPlayer()
    {

        if (searchRays.Count > 0)
        {
            var doesCollide = false;
            RaycastHit raycastHit = new RaycastHit();
            foreach (Ray r in searchRays)
            {
                doesCollide = Physics.Raycast(r, out raycastHit, viewDistance, LayerMask.GetMask("Player"));
                if (doesCollide == true)
                {
                    Debug.DrawRay(r.origin, r.direction * viewDistance, Color.yellow);
                    if (raycastHit.collider.gameObject.TryGetComponent(out PickupItem picker))
                    {
                        if (picker.HasItem == true)
                        {
                            sawPlayerStealing.Invoke();
                            break;
                        }
                        else
                        {
                            sawPlayer.Invoke();
                            break;
                        }
                    }
                }
                else {
                    Debug.DrawRay(r.origin, r.direction * viewDistance, Color.red);
                }
                    
            }
        }
    }
}
