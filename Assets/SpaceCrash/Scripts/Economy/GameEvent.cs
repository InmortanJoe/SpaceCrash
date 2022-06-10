using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GameEvent {}

public class ResourceChangeGameEvent : GameEvent
{
    public int amount;
    public ResourceType resourceType;

    public ResourceChangeGameEvent(int amount, ResourceType resourceType)
    {
        this.amount = amount;
        this.resourceType = resourceType;
    }
}

public class NotEnoughResourceGameEvent : GameEvent
{
    public int amount;
    public ResourceType resourceType;

    public NotEnoughResourceGameEvent(int amount, ResourceType resourceType)
    {
        this.amount = amount;
        this.resourceType = resourceType;
    }
}

public class EnoughResourceGameEvent : GameEvent
{

}

public class XPAddedGameEvent : GameEvent
{
    public int amount;

    public XPAddedGameEvent(int amount)
    {
        this.amount = amount;
    }
}

public class LevelChangedGameEvent : GameEvent
{
    public int newLvl;

    public LevelChangedGameEvent(int currLvl)
    {
        newLvl = currLvl;
    }
}
