using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIDrag : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{   
    public static Canvas canvas;

    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    private Image img;

    private Vector3 originPos;
    private bool drag;

    protected PlaceableObject2D source;

    public void Initialize(PlaceableObject2D src)
    {
        source = src;
    }

    private void Awake() 
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();

        img = transform.Find("Icon").GetComponent<Image>();
        originPos = rectTransform.anchoredPosition;
    }

    private void FixedUpdate() 
    {
        //Dragging
        if (drag)
        {
            if (source.GetType() != typeof(ProductionBuilding))
            {
                //Get the position and convert it to world point
                Vector2 touchPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

                //Raycast from that point
                RaycastHit2D hit = Physics2D.Raycast(touchPos, Vector2.positiveInfinity);

                //If the ray hit something
                if(hit.collider != null)
                {
                    //Get the placeable object from the hit
                    PlaceableObject2D selected = hit.transform.GetComponent<PlaceableObject2D>();

                    //Check if types match
                    if (selected.GetType() == source.GetType())
                    {
                        //trigger collision
                        OnCollide(selected);
                    }
                }
                
            }
            
        }        
    }
    protected virtual void OnCollide(PlaceableObject2D collidedSource){ }

    private bool overSlot;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Slots"))
        {
            overSlot = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        overSlot = false;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        //Handle begin drag
        drag = true;
        canvasGroup.blocksRaycasts = false;
        //Make the image not maskeable
        img.maskable = false;

        //disable PanZoom
        PanZoom.Instance.moveAllowed = false;

        
    }

    public void OnDrag(PointerEventData eventData)
    {
        //move the object considering the scale factor
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;

    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (overSlot)
        {
            OnCollide(source);
        }
        //Handle end drag
        drag = false;
        canvasGroup.blocksRaycasts = true;
        //Enable PanZoom
        PanZoom.Instance.moveAllowed = true;
        //Make the image maskeable
        img.maskable = true;
        //return the object to origin position
        rectTransform.anchoredPosition = originPos;
    }
}