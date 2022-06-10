using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    public static ShopManager Instance;
    public static Dictionary<ResourceType, Sprite> resourceSprite = new Dictionary<ResourceType, Sprite>();

    [SerializeField] private List<Sprite> sprites;

    private RectTransform rt;
    private RectTransform prt;
    private bool opened;

    [SerializeField] private GameObject itemPrefab;
    private Dictionary<ObjectType, List<ItemSlot>> itemSlot = new Dictionary<ObjectType, List<ItemSlot>>(5);

    [SerializeField] public TabGroup shopTabs;

    private void Awake() 
    {
        Instance = this;

        rt = GetComponent<RectTransform>();
        prt = transform.parent.GetComponent<RectTransform>();

        EventManager.Instance.AddListener<LevelChangedGameEvent>(OnLevelChanged);
    }

    private void Start() 
    {
        resourceSprite.Add(ResourceType.Metal, sprites[0]);
        resourceSprite.Add(ResourceType.Carbon, sprites[1]);
        resourceSprite.Add(ResourceType.Gas, sprites[2]);

        Load();
        Initialize();

        gameObject.SetActive(false);
    }

    private void Load()
    {
        ItemSlot[] items = Resources.LoadAll<ItemSlot>("Shop");

        itemSlot.Add(ObjectType.Building, new List<ItemSlot>());
        itemSlot.Add(ObjectType.Droid, new List<ItemSlot>());
        itemSlot.Add(ObjectType.Blueprint, new List<ItemSlot>());

        foreach (var item in items)
        {
            itemSlot[item.Type].Add(item);
        }

    }

    private void Initialize()
    {
        for (int i = 0; i < itemSlot.Keys.Count; i++)
        {
            foreach (var item in itemSlot[(ObjectType)i])
            {
                GameObject itemObject = Instantiate(itemPrefab, shopTabs.objectsToSwap[i].transform);
                itemObject.GetComponent<ItemHolder>().Initialize(item);
            }
        }
    }

    private void OnLevelChanged(LevelChangedGameEvent info)
    {
        for (int i = 0; i < itemSlot.Keys.Count; i++)
        {
            ObjectType key = itemSlot.Keys.ToArray()[i];
            for (int j = 0; j < itemSlot[key].Count; j++)
            {
               ItemSlot item = itemSlot[key][j];

               if(item.level == info.newLvl)
               {
                   shopTabs.transform.GetChild(i).GetChild(j).GetComponent<ItemHolder>().UnlockItem();
               }
            }
        }
    }

    public void ShopButton_Click()
    {
        float time = 0.2f;
        if (!opened)
        {
           LeanTween.moveX(prt, prt.anchoredPosition.x + rt.sizeDelta.x, time);
           opened = true;
           gameObject.SetActive(true);
        }
        else
        {
           LeanTween.moveX(prt, prt.anchoredPosition.x - rt.sizeDelta.x, time)
                .setOnComplete(delegate()
                {
                    gameObject.SetActive(false);
                });
            opened = false;
        }
    }

    private bool dragging;
    public void OnBeginDrag()
    {
        dragging = true;
    }

    public void OnEndDrag()
    {
        dragging = false;
    }

    public void OnPointerClick()
    {
        if (!dragging)
        {
            ShopButton_Click();
        }
    }
}
