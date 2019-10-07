using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ScoreZone : MonoBehaviour
{
    public List<BaseItem> items = new List<BaseItem>();
    public UnityEvent itemEnterScoreZone = new UnityEvent();

    private void OnTriggerEnter(Collider other)
    {
        BaseItem newItem = other.GetComponentInParent<BaseItem>();
        if ( newItem && other.gameObject.activeInHierarchy) {
            items.Add(newItem);
            Debug.Log("New Item" + newItem.name + " entered the score zone.");
            itemEnterScoreZone.Invoke();
        }
    }

    //private void OnTriggerExit(Collider other)
    //{
    //    BaseItem newItem;
    //    if (other.TryGetComponent(out newItem))
    //    {
    //        if (items.Contains(newItem)) {
    //            items.Remove(newItem);
    //        }
    //    }
    // }

}
