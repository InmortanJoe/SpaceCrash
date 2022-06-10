using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ProductionBuilding : Building, ISource
{
    [SerializeField] private List<Producible> allProducts;
    private int maxSlots = 3;
    private int prodAmount = 1;

    public State currentState { get; set; }
    private Queue<Producible> currentQueue = new Queue<Producible>();
    private Queue<Producible> produced = new Queue<Producible>();
    private Producible currentProd;
    private Timer timer;

    private void StartNextProduction()
    {
        currentProd = currentQueue.Peek();

        timer.Initialize(currentProd.name, DateTime.Now, currentProd.productionTime);
        timer.TimerFinishedEvent.AddListener(delegate
        {
            currentState = State.Ready;
            currentQueue.Dequeue();
            produced.Enqueue(currentProd);

            if (currentQueue.Count > 0)
            {
                StartNextProduction();
            }
            else
            {
                Destroy(timer);
                timer = null;
            }
        });

        timer.StartTimer();
    }

    public void Produce(Dictionary<CollectibleItem, int> itemsNeeded, CollectibleItem itemToProduce)
    {
        if (currentQueue.Count >= maxSlots)
        {
            return;
        }

        if (itemToProduce is Producible prod)
        {
            foreach (var itemPair in itemsNeeded)
            {
                if (!StorageManager.Instance.IsEnoughOf(itemPair.Key, itemPair.Value))
                {
                    Debug.Log("Not enough items");
                    return;
                }

                StorageManager.Instance.UpdateItems(itemsNeeded, false);

                currentQueue.Enqueue(prod);

                if (currentState == State.Empty)
                {
                    currentState = State.InProgress;
                    timer = gameObject.AddComponent<Timer>();
                    StartNextProduction();
                }

                ItemsTooltip.ShowTooltip_Static(gameObject, allProducts, currentQueue.ToList());
            }
        }
    }

    public void Collect()
    {
        Dictionary<CollectibleItem, int> result = new Dictionary<CollectibleItem, int>();
        result.Add(produced.Dequeue(), prodAmount);
        StorageManager.Instance.UpdateItems(result, true);

        if (produced.Count == 0)
        {
            if (currentQueue.Count == 0)
            {
                currentState = State.Empty;
            }
            else
            {
                currentState = State.InProgress;
            }
        }
    }
    
    protected override void OnClick()
    {
        if (built)
        {
            switch (currentState)
            {
                case State.Empty:
                    ItemsTooltip.ShowTooltip_Static(gameObject, allProducts);
                    break;
                case State.InProgress:
                    ItemsTooltip.ShowTooltip_Static(gameObject, allProducts, currentQueue.ToList());
                    break;
                case State.Ready:
                    Collect();
                    break;       
            }
        }
        else
        {
            base.OnClick();
        }
    }
}
