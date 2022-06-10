using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class StorageBuilding : PlaceableObject2D
{
    private StorageUI storageUI;

    private int currentTotal = 0;
    private int storageMax = 100;

    public string Name { get; private set; }

    [SerializeField] private GameObject windowPrefab;
    [SerializeField] private List<Tool> itemsToIncrease;

    private Dictionary<CollectibleItem, int> items;
    private Dictionary<CollectibleItem, int> tools;


    
    public void Initialize(Dictionary<CollectibleItem, int> itemAmounts, string name)
    {
        Name = name;

        GameObject window = Instantiate(windowPrefab, UIManager.Instance.canvas.transform);
        window.SetActive(false);
        storageUI = window.GetComponent<StorageUI>();

        storageUI.SetNameText(name);
        //initialize the items
        items = itemAmounts;
        //calculate the current total
        currentTotal = itemAmounts.Values.Sum();

        //initialize tools
        tools = new Dictionary<CollectibleItem, int>();
        foreach (var item in itemsToIncrease)
        {
            //add the amount of items needed
            tools.Add(item, 1);
        }
        
        //initialize UI
        storageUI.Initialize(currentTotal, storageMax, items, tools, IncreaseStorage);

    }

    ///increase the storage space
    private void IncreaseStorage()
    {
        //Go through each tool in tool dictionary
        foreach (var toolPair in tools)
        {
            //check if the player has enough of it
            if (!StorageManager.Instance.IsEnoughOf(toolPair.Key, toolPair.Value))
            {
                Debug.Log("Not enough tools");
                return;
            }
            
        }

        StorageManager.Instance.UpdateItems(tools, false);

        //increase Storage
        storageMax += 50;

        //initialize storage UI again
        storageUI.Initialize(currentTotal, storageMax, items, tools, IncreaseStorage);
    }

    public virtual void onClick()
    {
        //initialize storage UI again
        storageUI.Initialize(currentTotal, storageMax, items, tools, IncreaseStorage);

        //make the UI visible after click on the building
        storageUI.gameObject.SetActive(true);
    }

    private void OnMouseUpAsButton() 
    {
        onClick();
    }

}
