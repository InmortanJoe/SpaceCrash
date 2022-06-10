using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine;
using System;

public class ItemHolder : MonoBehaviour
{
    private ItemSlot Item;

    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private TextMeshProUGUI descriptionText;
    [SerializeField] private TextMeshProUGUI amountText;
    [SerializeField] private Image iconImage;
    [SerializeField] private Image resourceImage;
    [SerializeField] private TextMeshProUGUI costText;

    //Initialize the fields in Item Holder from the parameters of Item Slot
    public void Initialize(ItemSlot item)
    {
        Item = item;

        iconImage.sprite = Item.Icon;
        titleText.text = Item.name;
        descriptionText.text = Item.description;
        resourceImage.sprite = ShopManager.resourceSprite[Item.resource];
        costText.text = Item.cost.ToString();

        //Unlock the item with the level system
        if (Item.level >= LevelSystem.Instance.level)
        {
            UnlockItem();
        }

    }

    public void UnlockItem()
    {
        //Add Item Drag to the Icon
        iconImage.gameObject.AddComponent<ItemDrag>().Initialize(Item);
        //Enable the arrow on the side of the icon
        iconImage.transform.GetChild(0).gameObject.SetActive(true);
    }
}
