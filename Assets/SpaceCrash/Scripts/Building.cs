using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Building
{
    //Construction reference
    public int buldingID;

    //Width = X grid
    public int width = 0;

    //Length = Z grid
    public int length = 0;

    //Visual of the Building
    public GameObject buildingModel;

    //Small padding in case the building is clipping through the floor
    public float yPadding = 0;

    //Constrution funcionality
    public ResourceType resourceType = ResourceType.None;

    public StorageType storageType = StorageType.None;
    
    //Resource Type
    public enum ResourceType
    {
        None,
        Metal,
        Carbon,
        Gas,
        Storage
    }

    public enum StorageType
    {
        None,
        Metal,
        Carbon,
        Gas        
    }


}
