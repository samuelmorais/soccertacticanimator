using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using thelab.mvc;
using FootTactic;
using NG.Patterns.Structure.ObserverPattern;
using UnityEngine.UI;

public class DragUIItem : EventTrigger
{
    public Camera cam2D;

    Vector3 screenPoint, offset;

    void Start()
    {
        if(cam2D == null)
            cam2D = GameObject.Find("Camera2D").GetComponent<Camera>();
    }

    
    public override void OnDrag(PointerEventData eventData)
    {
        base.OnDrag(eventData);

        Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);

        Vector3 curPosition = cam2D.ScreenToWorldPoint(curScreenPoint) + offset;

        transform.position = curPosition;

    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        screenPoint = cam2D.WorldToScreenPoint(transform.position);

        offset = transform.position - cam2D.ScreenToWorldPoint(
            new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z)
        );        
    }

    
    
}