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
    public int numItems;
    public List<BaseItem> allItems = new List<BaseItem>();

    public bool IsThieving { get => CheckPlayerHasItem(); }


    private void Start()
    {
        if (scoreZone != null)
        {
            scoreZone.itemEnterScoreZone.AddListener(CheckWin);
        }
        for (var i = 0; i <= numItems; i++) {
            var newItem = GenerateItem();
            // TODO figure out how to not spawn them on top of each other
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

    BaseItem GenerateItem()
    {
        BaseItem newItem;
        newItem = Instantiate<BaseItem>(winItem);
        newItem.color = BaseItem.ATTR.COLOR.colors[Random.Range(0, BaseItem.ATTR.COLOR.colors.Count)];
        newItem.size = BaseItem.ATTR.SIZE.sizes[Random.Range(0, BaseItem.ATTR.SIZE.sizes.Count)];
        newItem.shape = BaseItem.ATTR.SHAPE.shapes[Random.Range(0, BaseItem.ATTR.SHAPE.shapes.Count)];
        newItem.weight = BaseItem.ATTR.WEIGHT.weights[Random.Range(0, BaseItem.ATTR.WEIGHT.weights.Count)];
        return newItem;
    }
}
