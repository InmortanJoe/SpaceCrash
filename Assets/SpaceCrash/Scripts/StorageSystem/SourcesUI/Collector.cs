using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collector : UIDrag
{
    protected override void OnCollide(PlaceableObject2D collidedSource)
    {
        collidedSource.GetComponent<ISource>().Collect();
    }
}