using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    private readonly bool isThieving;

    public Player player;
    public ScoreZone scoreZone;
    public BaseItem winItem;
    public int numItems;
    public List<BaseItem> allItems = new List<BaseItem>();
    public PlayerDetection[] playerDetectors;
    public int visionReduction = 7;

    public bool IsThieving { get => CheckPlayerHasItem(); }

    private void Start()
    {
        playerDetectors = GetComponentsInChildren<PlayerDetection>();

        if (scoreZone != null)
        {
            scoreZone.itemEnterScoreZone.AddListener(CheckWin);
        }
        for (var i = 0; i <= numItems; i++) {
            var newItem = GenerateItem();
            // TODO figure out how to not spawn them on top of each other
        }

        player.crouched.AddListener(ReduceVision);
        player.stand.AddListener(ReturnVision);
    }

    /// <summary>
    /// Go through all the player detectors and increase their vision by some fraction
    /// </summary>
    private void ReturnVision()
    {
        foreach (PlayerDetection detector in playerDetectors) {
            detector.viewDistance += visionReduction;
        }
    }

    /// <summary>
    /// Go through all the player detectors and reduce their vision by some fraction
    /// </summary>
    private void ReduceVision()
    {
        foreach (PlayerDetection detector in playerDetectors)
        {
            detector.viewDistance -= visionReduction;
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
                GetComponentInChildren<DialogSystem>().WinDialog();
            }
            else
            {
                Debug.Log("Not Winning");
                sendClueInfoToDialog(scoreZone.items[scoreZone.items.Count - 1]);
            }
            // Probably transition to some other screen
            // Or lock the player down and show the credits and a play again button
            // Something like that.
            // Or say go get me another item!!
        }
    }

    BaseItem GenerateItem()
    {
        BaseItem newItem;
        newItem = Instantiate<BaseItem>(winItem);
        newItem.color = BaseItem.ATTR.COLOR.colors[UnityEngine.Random.Range(0, BaseItem.ATTR.COLOR.colors.Count)];
        newItem.size = BaseItem.ATTR.SIZE.sizes[UnityEngine.Random.Range(0, BaseItem.ATTR.SIZE.sizes.Count)];
        newItem.shape = BaseItem.ATTR.SHAPE.shapes[UnityEngine.Random.Range(0, BaseItem.ATTR.SHAPE.shapes.Count)];
        newItem.weight = BaseItem.ATTR.WEIGHT.weights[UnityEngine.Random.Range(0, BaseItem.ATTR.WEIGHT.weights.Count)];
        return newItem;
    }

    void sendClueInfoToDialog(BaseItem item)
    {
        int amountIncorrect = 0;
        string randomDifference = "";
        List<string> allDifferences = new List<string>();

        if (winItem.color != item.color)
        {
            amountIncorrect += 1;
            allDifferences.Add("Color");
        }
        if (winItem.size != item.size)
        {
            amountIncorrect += 1;
            allDifferences.Add("Size");
        }
        if (winItem.shape != item.shape)
        {
            amountIncorrect += 1;
            allDifferences.Add("Shape");
        }
        if (winItem.weight != item.weight)
        {
            amountIncorrect += 1;
            allDifferences.Add("Weight");
        }
        randomDifference = allDifferences[UnityEngine.Random.Range(0, allDifferences.Count)];

        GetComponentInChildren<DialogSystem>().GenerateClue(amountIncorrect, randomDifference);
    }
}
