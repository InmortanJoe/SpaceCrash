using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ResourceProduct", menuName = "GameObject/StorageItems/ResourceProduct")]
public class ResourceProduct : Producible
{
    public Sprite growingResource;
    public Sprite readyResource;

    private new void OnValidate() 
    {
        base.OnValidate();

        ItemsNeeded = new Dictionary<CollectibleItem, int>() {{this, 1}};
    }

}
