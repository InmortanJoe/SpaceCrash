using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleObject : MonoBehaviour
{
    public ObstacleType obstacleType;
    public int resourceAmount = 10;

    TileObject refTile;


    /// <summary>
    /// This is a method that it is called whenever the item has been clicked or tapped.
    /// Works on Mobile and PC.
    /// </summary>
 private void OnMouseDown()
    {
        Debug.Log("Clicked on " + gameObject.name);

        //OnClick Event
        bool usedResource = false;

        //We can call directly the method that adds the resource
        switch (obstacleType)
        {
            case ObstacleType.Metal:

                usedResource = ResourcesManager.Instance.AddMetal(resourceAmount);

                break;
            case ObstacleType.Carbon:

                usedResource = ResourcesManager.Instance.AddCarbon(resourceAmount);

                break;
            case ObstacleType.Gas:

                usedResource = ResourcesManager.Instance.AddGas(5);

                break;
        }

        if(usedResource)
        {
            refTile.data.CleanTile();
            Destroy(gameObject);
        }
        else
            Debug.Log("Inventario lleno");

    }

    public void SetTileReference(TileObject obj)
    {
        refTile = obj;
    }

    public enum ObstacleType
    {
        Metal,
        Carbon,
        Gas
    }
}
