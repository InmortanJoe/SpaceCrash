using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ResourceSystem : MonoBehaviour
{
    private static Dictionary<ResourceType, int> ResourceAmounts = new Dictionary<ResourceType, int>();

    [SerializeField] private List<GameObject> texts;

    private Dictionary<ResourceType, TextMeshProUGUI> resourceTexts =
        new Dictionary<ResourceType, TextMeshProUGUI>();

    private void Awake() 
    {
        for (int i = 0; i < texts.Count; i++)
        {
            ResourceAmounts.Add((ResourceType)i, 0);
            resourceTexts.Add((ResourceType)i, texts[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>());
        }
    }

    private void Start() 
    {
        ResourceAmounts[ResourceType.Metal] = 500;
        ResourceAmounts[ResourceType.Carbon] = 200;
        ResourceAmounts[ResourceType.Gas] = 20;

        UpdateUI();
        EventManager.Instance.AddListener<ResourceChangeGameEvent>(OnResourceChange);
        EventManager.Instance.AddListener<NotEnoughResourceGameEvent>(OnNotEnough);        
    }

    private void UpdateUI()
    {
        for (int i = 0; i < texts.Count; i++)
        {
           resourceTexts[(ResourceType)i].text = ResourceAmounts[(ResourceType)i].ToString();
        }
    }

    private void OnResourceChange(ResourceChangeGameEvent info)
    {
        if (info.amount < 0)
        {
            if (ResourceAmounts[info.resourceType] < Mathf.Abs(info.amount))
            {
               EventManager.Instance.QueueEvent(new NotEnoughResourceGameEvent(info.amount, info.resourceType));
            return;
            }
            EventManager.Instance.QueueEvent(new EnoughResourceGameEvent());
        }

        ResourceAmounts[info.resourceType] += info.amount;
        resourceTexts[info.resourceType].text = ResourceAmounts[info.resourceType].ToString();

        UpdateUI();
    }

    private void OnNotEnough(NotEnoughResourceGameEvent info)
    {
        Debug.Log(message:$"You don't have enough of {info.amount} {info.resourceType}");
    }
}

public enum ResourceType
{
    Metal,
    Carbon,
    Gas
}