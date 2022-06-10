using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using UnityEngine;


public class Slots : MonoBehaviour
{
    public static Slots Instance;

    public PlaceableObject2D caller { get; private set; }
    private Timer timer;

    [SerializeField] private TextMeshProUGUI timeLeftText;
    [SerializeField] private List<GameObject> slots;

    private bool countdown;

    private void Awake() 
    {
        Instance = this;
        transform.gameObject.SetActive(false);
    }

    public void ShowSlots<T>(GameObject callerObj, List<T> itemsQueue)
        where T : Producible
    {
        caller = callerObj.GetComponent<PlaceableObject2D>();

        if (itemsQueue != null)
        {
            ClearSlots();
            InitializeSlots(itemsQueue);

            timer = callerObj.GetComponent<Timer>();

            countdown = true;
        }
        else
        {
            ClearSlots();
        }

        transform.gameObject.SetActive(true);
    }

    private void InitializeSlots<T>(List<T> itemsQueue)
        where T : Producible
    {
        for (int i = 0; i < slots.Count && i < itemsQueue.Count; i++)
        {
            slots[i].transform.Find("Icon").GetComponent<Image>().sprite = itemsQueue[i].Icon;
        }
    }

    private void ClearSlots()
    {
        timeLeftText.text = "Empty";
        for (int i = 0; i < slots.Count; i++)
        {
            slots[i].transform.Find("Icon").GetComponent<Image>().sprite = null;
        }
    }

    private void FixedUpdate() 
    {
        if (countdown)
        {
            timeLeftText.text = timer.DisplayTime();
        }
    }

    private void OnDisable() 
    {
        caller = null;
        timer = null;
        countdown = false;
    }
}
