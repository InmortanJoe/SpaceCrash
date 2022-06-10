using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;

public class Building : PlaceableObject2D
{
    protected bool built;

    private bool cantBuilt;

    public int metalAmount = 0;

    public override void Place()
    {
        EventManager.Instance.AddListenerOnce<EnoughResourceGameEvent>(OnEnoughResource);
        EventManager.Instance.AddListenerOnce<NotEnoughResourceGameEvent>(OnNotEnoughResource);
        
        base.Place();

        //add timer to the object
        Timer timer = gameObject.AddComponent<Timer>();
        //initialize timer - name of the process
        timer.Initialize("Building", DateTime.Now, TimeSpan.FromMinutes(3));
        //start timer
        timer.StartTimer();
        //when the timer finished
        timer.TimerFinishedEvent.AddListener(delegate
        {
            built = true;
            Destroy(timer);
        });
    }

    protected override void OnClick()
    {
        if (!built)
        {
            //on object click - display the tooltip
            TimerTooltip.ShowTimer_Static(gameObject);
        }
    }

    private void BuyBuilding()
    {
        //invoke Resource change event to notify the Resource system
        ResourceChangeGameEvent info = new ResourceChangeGameEvent(-metalAmount, ResourceType.Metal);
        EventManager.Instance.QueueEvent(info);
    
    }

    private void OnEnoughResource(EnoughResourceGameEvent info)
    {
        BuyBuilding();
        //remove listener of the opposite event
        EventManager.Instance.RemoveListener<NotEnoughResourceGameEvent>(OnNotEnoughResource);
    }

    private void OnNotEnoughResource(NotEnoughResourceGameEvent info)
    {
        //remove listener of the opposite event
        EventManager.Instance.RemoveListener<EnoughResourceGameEvent>(OnEnoughResource);
    }  

    private void OnMouseUpAsButton() 
    {
        OnClick();
    }
}
