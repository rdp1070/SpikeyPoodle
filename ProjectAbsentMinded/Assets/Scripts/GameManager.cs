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
    [SerializeField]
    private BaseItem winItem;
    public BaseItem defaultItem;
    public int numItems;
    public List<BaseItem> allItems = new List<BaseItem>();
    public PlayerDetection[] playerDetectors;
    public SpawnPoint[] spawnPoints;
    public int visionReduction = 7;

    public bool IsThieving { get => CheckPlayerHasItem(); }


    private void Start()
    {
        playerDetectors = GetComponentsInChildren<PlayerDetection>();
        spawnPoints = GetComponentsInChildren<SpawnPoint>();

        if (scoreZone != null)
        {
            scoreZone.itemEnterScoreZone.AddListener(CheckWin);
        }
        if (player != null)
        {
            player.crouched.AddListener(ReduceVision);
            player.stand.AddListener(ReturnVision);
        }
        
        for (var i = 0; i < spawnPoints.Length; i++)
        {
            var newItem = GenerateItem(spawnPoints[i].transform.position);
            allItems.Add(newItem);
        }
        
        winItem = allItems[UnityEngine.Random.Range(0, allItems.Count)];
        defaultItem.gameObject.SetActive(false);
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

    BaseItem GenerateItem( Vector3 spawnPoint = new Vector3())
    {
        BaseItem newItem;
        newItem = Instantiate<BaseItem>(defaultItem);
        newItem.color = BaseItem.ATTR.COLOR.colors[UnityEngine.Random.Range(0, BaseItem.ATTR.COLOR.colors.Count)];
        newItem.size = BaseItem.ATTR.SIZE.sizes[UnityEngine.Random.Range(0, BaseItem.ATTR.SIZE.sizes.Count)];
        newItem.shape = BaseItem.ATTR.SHAPE.shapes[UnityEngine.Random.Range(0, BaseItem.ATTR.SHAPE.shapes.Count)];
        newItem.weight = BaseItem.ATTR.WEIGHT.weights[UnityEngine.Random.Range(0, BaseItem.ATTR.WEIGHT.weights.Count)];
        newItem.transform.position = spawnPoint;
        return newItem;
    }
}
