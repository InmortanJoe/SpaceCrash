using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class StorageUI : MonoBehaviour
{
    private Action increaseAction;

    [SerializeField] private TextMeshProUGUI storageTypeText;
    [SerializeField] private TextMeshProUGUI maxItemsText;
    [SerializeField] private Slider maxItemsSlider;

    [SerializeField] private GameObject itemsView;
    [SerializeField] private GameObject increaseView;

    [SerializeField] private Transform itemsContent;
    [SerializeField] private Transform increaseContent;

    [SerializeField] private GameObject itemPrefab;

    public void SetNameText(string name)
    {
        storageTypeText.text = name;
    }

    //
    public void Initialize(int currentAmount, int maxAmount, Dictionary<CollectibleItem, int> itemAmounts,
                                Dictionary<CollectibleItem, int> tools, Action onIncrease)
    {
        maxItemsText.text = currentAmount + "/" + maxAmount;
        maxItemsSlider.value = (float) currentAmount / maxAmount;

        InitializeItems(itemAmounts);
        InitializeTools(tools);

        increaseAction = onIncrease;
    }

    private void InitializeItems(Dictionary<CollectibleItem, int> itemAmounts)
    {
        int childCount = itemsContent.childCount;
        for (int i = 0; i < childCount; i++)
        {
            Destroy(itemsContent.GetChild(i).gameObject);
        }

        foreach (var itemPair in itemAmounts)
        {
            GameObject itemHolder = Instantiate(itemPrefab, itemsContent);
            itemHolder.transform.Find("Icon").GetComponent<Image>().sprite = itemPair.Key.Icon;
            itemHolder.transform.Find("Amount").GetComponent<TextMeshProUGUI>().text = itemPair.Value.ToString();
        }
    }

    private void InitializeTools(Dictionary<CollectibleItem, int> tools)
    {
        int i = 0;

        foreach (var itemPair in tools)
        {
            GameObject itemHolder = increaseContent.GetChild(i).gameObject;
            itemHolder.transform.Find("Icon").GetComponent<Image>().sprite = itemPair.Key.Icon;
            itemHolder.transform.Find("Amount").GetComponent<TextMeshProUGUI>().text = 
                StorageManager.Instance.GetAmount(itemPair.Key) + "/" + itemPair.Value;
            i++;
        }
    }

    #region Buttons

    public void CloseButton_Click()
    {
        gameObject.SetActive(false);
    }

    public void IncreaseButton_Click()
    {
        increaseView.SetActive(true);
    }

    public void ConfirmButton_Click()
    {
        increaseAction.Invoke();
    }

    public void BackButton_Click()
    {
        increaseView.SetActive(false);
    }


    #endregion
}
