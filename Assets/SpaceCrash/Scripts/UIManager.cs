using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("References")]

    [Space(8)]
    /*References for UI containers */
    public StandardUIReference metalUI;
    public StandardUIReference carbonUI;
    public StandardUIReference gasUI;


    //Instance handling for singleton
    public static UIManager Instance;

    private void Awake() 
    {
        //Initializing the singleton pattern(not for production)
        Instance = this;
    }
    private void Start() 
    {
        UpdateAll();
    }

    /// <summary>
    /// Updates the Metal UI
    /// </summary>
    /// <param name="currentAmount">Sets the current amount</param>
    /// <param name="maxAmount">Sets the maximum amount</param>
    public void UpdateMetalUI(int currentAmount, int maxAmount)
    {
        //Set the text in UI
       metalUI.currentUI.text = currentAmount.ToString();
       metalUI.maxUI.text = "MAX: " + maxAmount.ToString();
        //Set the slider in UI
       metalUI.slider.maxValue = maxAmount;
       metalUI.slider.value = currentAmount;
    }

    /// <summary>
    /// Updates the Carbon UI
    /// </summary>
    /// <param name="currentAmount">Sets the current amount</param>
    /// <param name="maxAmount">Sets the maximum amount</param>
        public void UpdateCarbonUI(int currentAmount, int maxAmount)
    {
        //Set the text in UI        
       carbonUI.currentUI.text = currentAmount.ToString();
       carbonUI.maxUI.text = "MAX: " + maxAmount.ToString();
        //Set the slider in UI
       carbonUI.slider.maxValue = maxAmount;
       carbonUI.slider.value = currentAmount;
    }

    /// <summary>
    /// Updates the Gas UI
    /// </summary>
    /// <param name="currentAmount">Sets the current amount</param>
    /// <param name="maxAmount">Sets the maximum amount</param>
        public void UpdateGasUI(int currentAmount, int maxAmount)
    {
        //Set the text in UI
       gasUI.currentUI.text = currentAmount.ToString();
       gasUI.maxUI.text = "MAX: " + maxAmount.ToString();
        //Set the slider in UI
       gasUI.slider.maxValue = maxAmount;
       gasUI.slider.value = currentAmount;
    }

        void UpdateAll()
    {
        UpdateMetalUI(ResourcesManager.Instance.Metal, ResourcesManager.Instance.maxMetal);
        UpdateCarbonUI(ResourcesManager.Instance.Carbon, ResourcesManager.Instance.maxCarbon);
        UpdateGasUI(ResourcesManager.Instance.Gas, ResourcesManager.Instance.maxGas);
    }
}
//Main class for containers
[System.Serializable]
public class StandardUIReference
{
    public Slider slider;
    public Text maxUI;
    public Text currentUI;
}
