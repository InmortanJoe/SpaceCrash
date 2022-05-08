using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildingObject : MonoBehaviour
{
    public Building data;

    [Header("Resource Generation")]
    [Space(8)]

    //This will be the resource that has created by the building
    public float resource = 0;

    //Limit that this building can generate or do
    public float resourceLimit;

    //Speed that the resource is generated
    public float generationSpeed = 100;

    Coroutine buildingBehaviour;

    [Header("UI")]
    [Space(8)]

    public GameObject canvasObject;
    public Slider progressSlider;

    private void Start() 
    {
        if(data.resourceType  == Building.ResourceType.Metal || data.resourceType  != Building.ResourceType.Carbon)
        buildingBehaviour = StartCoroutine(CreateResource());


        if(data.resourceType  == Building.ResourceType.Storage)
        {
            IncreaseMaxStorage();
            canvasObject.SetActive(false);
            Debug.Log("increase storage");

        }
    }

    private void OnMouseDown()
    {   
        if(data.resourceType == Building.ResourceType.Storage)
            return;

        switch (data.resourceType)
        {
            case Building.ResourceType.None:
                break;
            case Building.ResourceType.Metal:

                ResourcesManager.Instance.AddMetal((int)resource);
                break;
            case Building.ResourceType.Carbon:

                ResourcesManager.Instance.AddCarbon((int)resource);
                break;
            case Building.ResourceType.Gas:

                ResourcesManager.Instance.AddGas((int)resource);
                break;
        }

        EmptyResource();
    }

    void EmptyResource()
    {
        resource = 0;
    }

    void IncreaseMaxStorage()
    {
        switch (data.storageType)
        {
            case Building.StorageType.Metal:

                ResourcesManager.Instance.IncreaseMaxMetal((int)resource);
                break;
            case Building.StorageType.Carbon:

                ResourcesManager.Instance.IncreaseMaxCarbon((int)resource);
                break;
            case Building.StorageType.Gas:

                ResourcesManager.Instance.IncreaseMaxGas((int)resource);
                break;        
        }
    }

    IEnumerator CreateResource()
    {
        //It will create resources infinitely
        while (true)
        {
            if (resource < resourceLimit)
            {
                resource += generationSpeed * Time.deltaTime;

            }
            else
            {
                resource = resourceLimit;
            }

            UpdateUI(resource, resourceLimit);

            yield return null;            
        }            
    }

    public void UpdateUI(float current, float maxValue)
    {
        progressSlider.value = current;
        progressSlider.maxValue = maxValue;
    }

}
