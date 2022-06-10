using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    [Header("References")]

    [Space(8)]
    public List<GameObject> buildMenus;
    public GameObject buildMenu;

    public GameObject canvas;

    //Instance handling for singleton
    public static UIManager Instance;

    private void Awake() 
    {
        //Initializing the singleton pattern(not for production)
        Instance = this;
        
        ItemDrag.canvas = canvas.GetComponent<Canvas>();
        UIDrag.canvas = canvas.GetComponent<Canvas>();       
    }
}

