using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectorTooltip : MonoBehaviour 
{
    private static CollectorTooltip Instance;

    [SerializeField] private Camera uiCamera;
    [SerializeField] private GameObject collectorHolders;

    private void Awake() 
    {
        Instance = this;
        transform.parent.gameObject.SetActive(false);
    }

    private void ShowTooltip(GameObject caller)
    {
        collectorHolders.GetComponent<Collector>().Initialize(caller.GetComponent<PlaceableObject2D>());
        Vector3 position = caller.transform.position - uiCamera.transform.position;
        position = uiCamera.WorldToScreenPoint(uiCamera.transform.TransformPoint(position));
        transform.position = position;

        transform.parent.gameObject.SetActive(true);
    }

    public void HideTooltip ()
    {
        transform.parent.gameObject.SetActive(false);
    }

    public static void ShowTooltip_Static(GameObject caller)
    {
        Instance.ShowTooltip(caller);
    }

    public static void HideTooltip_Static()
    {
        Instance.HideTooltip();
    }    
}