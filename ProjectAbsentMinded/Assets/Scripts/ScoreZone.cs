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
        BaseItem newItem;
        if (other.TryGetComponent(out newItem)) {
            items.Add(newItem);
            itemEnterScoreZone.Invoke();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        BaseItem newItem;
        if (other.TryGetComponent(out newItem))
        {
            if (items.Contains(newItem)) {
                items.Remove(newItem);
            }
        }
    }

}
