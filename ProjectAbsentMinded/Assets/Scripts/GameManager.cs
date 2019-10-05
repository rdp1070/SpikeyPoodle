using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    private readonly bool isThieving;

    public GameObject player;
    public ScoreZone scoreZone;
    public BaseItem winItem;

    public bool IsThieving { get => CheckPlayerHasItem(); }


    private void Start()
    {
        if (scoreZone != null)
        {
            scoreZone.itemEnterScoreZone.AddListener(CheckWin);
        }
    }


    bool CheckPlayerHasItem()
    {
        return player.GetComponentInChildren<PickupItem>().HasItem;
    }

    void CheckWin()
    {
        foreach (BaseItem i in scoreZone.items)
        {
            if (winItem.Equals(i))
            {
                Debug.Log("Winner!");
            }
            else
            {
                Debug.Log("Not Winning");
            }
            // Probably transition to some other screen
            // Or lock the player down and show the credits and a play again button
            // Something like that.
        }
    }


}
