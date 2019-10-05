using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private readonly bool isThieving;

    public GameObject player; 

    public bool IsThieving { get => CheckPlayerHasItem(); }

    bool CheckPlayerHasItem()
    {
        return player.GetComponentInChildren<PickupItem>().HasItem;
    }
}
