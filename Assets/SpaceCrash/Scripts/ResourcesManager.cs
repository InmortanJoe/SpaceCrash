using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourcesManager : MonoBehaviour
{
    [Header("Resources")]

    [Space(8)]

    //Sets the max amount of Metal
    public int maxMetal;
    int metal = 0;

    //Sets the max amount of Carbon
    public int maxCarbon;
    int carbon = 0;

    //Sets the max amount of Gas
    public int maxGas;
    int gas = 0;

    public static ResourcesManager Instance;

    public bool debugBool = false;

    public int Metal
    {
        get
        {
            return metal;
        }

        set
        {
            metal = value;
        }
    }
    public int Carbon
    {
        get
        {
            return carbon;
        }

        set
        {
            carbon = value;
        }
    }
    public int Gas
    {
        get
        {
            return gas;
        }

        set
        {
            gas = value;
        }
    }

    private void Awake() 
    {
        Instance = this;
    }

    private void Update()
    {
        if(debugBool)
        {
            PrintCurrentResources();
            debugBool = false;
        }
    }

    /// <summary>
    /// Adds more Metal to the inventory
    /// </summary>
    /// <param name="amount">Amount to add directly to the existing metal</param>

    public bool AddMetal(int amount)
    {
        if ((metal + amount) <= maxMetal)
        {
            metal += amount;
            //Updates the corresponding UI.
            UIManager.Instance.UpdateMetalUI(metal, maxMetal);

            return true;
        }
        else
        {
            return false;
        }

    }

    public void IncreaseMaxMetal(int amount)
    {
        maxMetal += amount;
        UIManager.Instance.UpdateMetalUI(metal, maxMetal);
    }

    /// <summary>
    /// Adds more Carbon to the inventory
    /// </summary>
    /// <param name="amount">Amount to add directly to the existing carbon</param>

    public bool AddCarbon(int amount)
    {
        if ((carbon + amount) <= maxCarbon)
        {
            carbon += amount;
            //Updates the corresponding UI.        
            UIManager.Instance.UpdateCarbonUI(carbon, maxCarbon);

            return true;
        }
        else
        {
            return false;
        }

    }

        public void IncreaseMaxCarbon(int amount)
    {
        maxCarbon += amount;
        UIManager.Instance.UpdateCarbonUI(carbon, maxCarbon);
    }

    /// <summary>
    /// Adds more Gas to the inventory
    /// </summary>
    /// <param name="amount">Amount to add directly to the existing gas</param>

    public bool AddGas(int amount)
    {
        if ((gas + amount) <= maxGas)
        {
            gas += amount;
            //Updates the corresponding UI.        
            UIManager.Instance.UpdateGasUI(gas, maxGas);

            return true;
        }
        else
        {
            return false;
        }
    }

    public void IncreaseMaxGas(int amount)
    {
        maxGas += amount;
        UIManager.Instance.UpdateGasUI(gas, maxGas);
    }


    void PrintCurrentResources()
    {
        Debug.Log("Metal " + metal);
        Debug.Log("Carbon " + carbon);
        Debug.Log("Gas " + gas);
    }

}
