using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StorageManager : MonoBehaviour
{
    public static StorageManager Instance;

    [SerializeField] private GameObject hangarPrefab;
    [SerializeField] private GameObject siloPrefab;

    private string itemsPath = "Storage";
    private Dictionary<MechanicProduct, int> mechanicsProduct;
    private Dictionary<RecycleProduct, int> recyclesProduct;
    private Dictionary<ResourceProduct, int> resourcesProduct;
    private Dictionary<VegetalProduct, int> vegetalsProduct;
    private Dictionary<Tool, int> tools;

    private Dictionary<CollectibleItem, int> hangarItems;
    private Dictionary<CollectibleItem, int> siloItems;

    private StorageBuilding Hangar;
    private StorageBuilding Silo;

    private void Awake() 
    {
        Instance = this;
        Dictionary<CollectibleItem, int> itemAmounts = LoadItems();
        Sort(itemAmounts);

        Mine.Initialize(resourcesProduct);
    }

    private Dictionary<CollectibleItem, int> LoadItems()
    {
        Dictionary<CollectibleItem, int> itemAmounts = new Dictionary<CollectibleItem, int>();
        CollectibleItem[] allItems = Resources.LoadAll<CollectibleItem>(itemsPath);
        
        for (int i = 0; i < allItems.Length; i++)
        {
            //Remove 2 in the game
            itemAmounts.Add(allItems[i], 1);
            /*if (allItems[i].Level >= LevelSystem.Instance.level)
            {
            
            }*/
            
        }

        return itemAmounts;
    }

    private void Sort(Dictionary<CollectibleItem, int> itemsAmounts)
    {
        mechanicsProduct = new Dictionary<MechanicProduct, int>();
        recyclesProduct = new Dictionary<RecycleProduct, int>();
        resourcesProduct = new Dictionary<ResourceProduct, int>();
        vegetalsProduct = new Dictionary<VegetalProduct, int>();
        tools = new Dictionary<Tool, int>();

        hangarItems = new Dictionary<CollectibleItem, int>();
        siloItems = new Dictionary<CollectibleItem, int>();

        foreach (var itemPair in itemsAmounts)
        {
            if (itemPair.Key is MechanicProduct mechanicProduct)
            {
                mechanicsProduct.Add(mechanicProduct, itemPair.Value);
                hangarItems.Add(mechanicProduct, itemPair.Value);
            }
            else if (itemPair.Key is RecycleProduct recycleProduct)
            {
                recyclesProduct.Add(recycleProduct, itemPair.Value);
                hangarItems.Add(recycleProduct, itemPair.Value);                
            }
            else if (itemPair.Key is ResourceProduct resourceProduct)
            {
                resourcesProduct.Add(resourceProduct, itemPair.Value);
                siloItems.Add(resourceProduct, itemPair.Value);                
            }
            else if (itemPair.Key is VegetalProduct vegetalProduct)
            {
                vegetalsProduct.Add(vegetalProduct, itemPair.Value);
                siloItems.Add(vegetalProduct, itemPair.Value); 
            }
            else if (itemPair.Key is Tool tool)
            {
                tools.Add(tool, itemPair.Value);
                hangarItems.Add(tool, itemPair.Value); 
            }

        }
    }

    private void Start() 
    {
        GameObject siloObject = BuildingSystem2D.Instance.InitializeWithObject(siloPrefab, new Vector3(2f, 6.25f));
        Silo = siloObject.GetComponent<StorageBuilding>();
        Silo.Load();
        //
        Silo.Initialize(siloItems, "Silo");

        GameObject hangarObject = BuildingSystem2D.Instance.InitializeWithObject(hangarPrefab, new Vector3(3f, 3.25f));
        Hangar = hangarObject.GetComponent<StorageBuilding>();
        Hangar.Load();
        // 
        Hangar.Initialize(hangarItems, "Hangar");       
    }

    public int GetAmount(CollectibleItem item)
    {
        int amount = 0;
        if (item is MechanicProduct mechanicProduct)
        {
            mechanicsProduct.TryGetValue(mechanicProduct, out amount);
        }
        else if (item is RecycleProduct recycleProduct)
        {
            recyclesProduct.TryGetValue(recycleProduct, out amount);
        }
        else if (item is ResourceProduct resourceProduct)
        {
            resourcesProduct.TryGetValue(resourceProduct, out amount);                            
        }
        else if (item is VegetalProduct vegetalProduct)
        {
            vegetalsProduct.TryGetValue(vegetalProduct, out amount); 
        }
        else if (item is Tool tool)
        {
            tools.TryGetValue(tool, out amount); 
        }
        
        return amount;
    }

    public bool IsEnoughOf(CollectibleItem item, int amount)
    {
        return GetAmount(item) >= amount;
    }

    public void UpdateItems(Dictionary<CollectibleItem, int> items, bool add)
    {
        foreach (var itemPair in items)
        {
            var item = itemPair.Key;
            var amount = itemPair.Value;

            if (!add)
            {
                amount = -amount;
            }

            if (item is MechanicProduct mechanicProduct)
            {
                mechanicsProduct[mechanicProduct] += amount;
                hangarItems[item] += amount;
            }
            else if (item is RecycleProduct recycleProduct)
            {
                recyclesProduct[recycleProduct] += amount;
                hangarItems[item] += amount;                
            }
            else if (item is ResourceProduct resourceProduct)
            {
                resourcesProduct[resourceProduct] += amount;
                siloItems[item] += amount;                
            }
            else if (item is VegetalProduct vegetalProduct)
            {
                vegetalsProduct[vegetalProduct] += amount;
                siloItems[item] += amount;                
            }
            else if (item is Tool tool)
            {
                tools[tool] += amount;
                hangarItems[item] += amount;                
            }            
        }
    }
}
