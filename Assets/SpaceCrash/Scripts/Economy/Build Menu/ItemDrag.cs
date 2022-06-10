using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine;

public class ItemDrag : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    private ItemSlot Item;
    public static Canvas canvas;

    private RectTransform rt;
    private CanvasGroup cg;
    private Image img;

    private Vector3 originPos;
    private bool drag;

    public void Initialize(ItemSlot item)
    {
        Item = item;
    }

    private void Awake() 
    {
        rt = GetComponent<RectTransform>();
        cg = GetComponent<CanvasGroup>();

        img = GetComponent<Image>();
        originPos = rt.anchoredPosition;    
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        drag = true;
        cg.blocksRaycasts = false;
        img.maskable = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        rt.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        drag = false;
        cg.blocksRaycasts = true;
        img.maskable = true;
        rt.anchoredPosition = originPos;
    }

    //Active the world space entry of the object
    private void OnTriggerEnter2D(Collider2D other) 
    {
        ShopManager.Instance.ShopButton_Click();

        Color c = img.color;
        c.a = 0f;
        img.color = c;

        Vector3 position = new Vector3(transform.position.x, transform.position.y);
        position = Camera.main.ScreenToWorldPoint(position);

        BuildingSystem2D.Instance.InitializeWithObject(Item.prefab, position);
    }

    //Reactive the object state in the Shop
    private void OnEnable() 
    {
      drag = false;
      cg.blocksRaycasts = true;
      img.maskable = true;
      rt.anchoredPosition = originPos;

      Color c = img.color;
      c.a = 1f;
      img.color = c;

    }

}
