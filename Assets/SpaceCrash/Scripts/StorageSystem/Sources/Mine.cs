using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Mine : PlaceableObject2D, ISource
{
    private static Dictionary<ResourceProduct, int> allResources;
    private static int amount = 2;

    public State currentState { get; set; }
    private ResourceProduct currentRP;
    private Timer timer;

    private SpriteRenderer sr;
    private Sprite emptyMineSprite;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        emptyMineSprite = sr.sprite;
    }

    public static void Initialize(Dictionary<ResourceProduct, int> resource)
    {
        allResources = resource;
    }

    protected override void OnClick()
    {
        //check the state
        switch (currentState)
        {
            case State.Empty:
                //the field is empty 
                ItemsTooltip.ShowTooltip_Static(gameObject, allResources);
                break;
            case State.InProgress:
                //is growing on the field -> display the timer
                TimerTooltip.ShowTimer_Static(gameObject);
                break;
            case State.Ready:
                //the field is ready -> display the tooltip to collect
                CollectorTooltip.ShowTooltip_Static(gameObject);
                break;
        }
    }

    public void Produce(Dictionary<CollectibleItem, int> itemsNeeded, CollectibleItem itemToProduce)
    {
        if (currentState != State.Empty)
        {
            return;
        }
        
        if (itemToProduce is ResourceProduct resource)
        {
            currentRP = resource;
        }
        else
        {
            return;
        }

        //Check if the player has enough items
        foreach (var itemPair in itemsNeeded)
        {
            if (!StorageManager.Instance.IsEnoughOf(itemPair.Key, itemPair.Value))
            {
                Debug.Log("Not enough items");
                return;
            }
        }

        //Take items from the storage
        StorageManager.Instance.UpdateItems(itemsNeeded, false);

        //Change the state and sprite
        currentState = State.InProgress;
        sr.sprite = currentRP.growingResource;

        //Add timer
        timer = gameObject.AddComponent<Timer>();
        timer.Initialize(currentRP.name, DateTime.Now, currentRP.productionTime);
        timer.TimerFinishedEvent.AddListener(delegate
        {
            currentState = State.Ready;
            sr.sprite = currentRP.readyResource;

            Destroy(timer);
            timer = null;
        });
        timer.StartTimer();
    }

    public void Collect()
    {   
        Dictionary<CollectibleItem, int> result = new Dictionary<CollectibleItem, int>();
        //add resource to the storage
        StorageManager.Instance.UpdateItems(result, true);
        //change the stage and sprite
        currentState = State.Empty;
        sr.sprite = emptyMineSprite;
        //remove the current resource
        currentRP = null;

    }
}