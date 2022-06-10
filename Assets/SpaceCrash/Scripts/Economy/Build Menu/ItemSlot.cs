using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "GameObject/ItemSlot", order = 0)]
public class ItemSlot : ScriptableObject
{
    public string name = "Default";
    public string description = "Description";
    public int level;
    //Item cost
    public int cost;
    //The resource spent when buy the item
    public ResourceType resource;
    public ObjectType Type;
    public Sprite Icon;
    public GameObject prefab;

}

public enum ObjectType
{
    Building,
    Droid,
    Blueprint
}