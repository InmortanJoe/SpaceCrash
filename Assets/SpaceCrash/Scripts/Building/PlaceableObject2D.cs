using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



//Objects in game that the player can put in the world space
public class PlaceableObject2D : MonoBehaviour
{
    //The object is in the world?
    public bool Placed { get; private set; }

    //Position of the object
    private Vector3 origin;

    //Area under the Object
    public BoundsInt area;

    private void Awake() 
    {
        //The camera follow the object
        PanZoom.Instance.FollowObject(transform);
    }

    public void Load()
    {
        PanZoom.Instance.UnfollowObject();
        Destroy(GetComponent<ObjectDrag2D>());
        Place();
    }


    //Check if the object can be placed at the current position
    public bool CanBePlaced()
    {
        //Create area under the building
        Vector3Int positionInt = BuildingSystem2D.Instance.gridLayout.LocalToCell(transform.position);
        BoundsInt areaTemp = area;
        areaTemp.position = positionInt;

        //Call the grid Building System
        return BuildingSystem2D.Instance.CanTakeArea(areaTemp);
     
    }

    //Activate the placement of the object in the world space
    public virtual void Place()
    {
        //Create area under the building
        Vector3Int positionInt = BuildingSystem2D.Instance.gridLayout.LocalToCell(transform.position);
        BoundsInt areaTemp = area;
        areaTemp.position = positionInt;

        //Set the bool Placed
        Placed = true;

        //Save position
        origin = transform.position;

        //Call the System
        BuildingSystem2D.Instance.TakeArea(areaTemp);
    }

    //Check if the building can be placed
    public void CheckPlacement()
    {
        PanZoom.Instance.UnfollowObject();

        //Object is new an haven't been placed before
        if(!Placed)
        {
            //If it can be placed
            if(CanBePlaced())
            {
                Place();
            }
            else
            {
                //Destroy this object (is new)
                Destroy(transform.gameObject);
            }
            
            //Open the shop
            ShopManager.Instance.ShopButton_Click();
        }
        //Editing the map, object has been placed before
        else
        {
            //If cannot be placed
            if(!CanBePlaced())
            {
                //Reset the position
                transform.position = origin;
            }
            
            Place();
                     
        }

    }

    //Time elapsed since the touch begun
    private float time = 0f;
    private bool touching;
    private bool moving;

   /* private void Update() 
    {
        if(!touching && Input.GetMouseButton(0))
        {
            //Increase time elapsed
            time += Time.deltaTime;
            
            //Time limit exceeded
            if(time > 3f)
            {
                touching = false;
                moving = true;
                //Add component to Drag
                gameObject.AddComponent<ObjectDrag2D>();

                //Prepare area
                Vector3Int positionInt = BuildingSystem2D.Instance.gridLayout.WorldToCell(transform.position);
                BoundsInt areaTemp = area;
                areaTemp.position = positionInt;
                
                //Clear area on which the object was standing on
                BuildingSystem2D.Instance.ClearArea(areaTemp, BuildingSystem2D.Instance.MainTilemap);
            }
        }

    }*/

    private void OnMouseDown() 
    {
        time = 0;
        touching = true;
    }

    protected virtual void OnClick() { }

    private void OnMouseUpAsButton() 
    {
        if(moving)
        {
            moving = false;
            return;
        }

        OnClick();
    }
}
