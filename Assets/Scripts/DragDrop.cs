using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DragDrop : MonoBehaviour, IDragHandler
{
    private Canvas canvas;
    private RectTransform rectTransform;
    [SerializeField] private GameObject window;

    Vector3 offset;
    RectTransform rt;

    private void Start()
    {
        Vector3 minBounds, maxBounds;
        Collider parentCollider = window.GetComponent<Collider>();
        minBounds = parentCollider.bounds.min;
        maxBounds = parentCollider.bounds.max;        

        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector3 minBounds, maxBounds;
        Collider parentCollider = window.GetComponent<Collider>();
        minBounds = parentCollider.bounds.min;
        maxBounds = parentCollider.bounds.max;

        float xPos = Mathf.Clamp(transform.position.x, minBounds.x, maxBounds.x);
        float yPos = Mathf.Clamp(transform.position.y, minBounds.y, maxBounds.y);

        if (xPos > minBounds.x && xPos < maxBounds.x && yPos > minBounds.y && yPos < maxBounds.y)
        {
            rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
        }
        else
        {
            float step = 0.05f;

            if (xPos <= minBounds.x)
            {
                rectTransform.position = new Vector3(transform.position.x + step, transform.position.y, 0);
            } else if (xPos >= maxBounds.x)
            {
                rectTransform.position = new Vector3(transform.position.x - step, transform.position.y, 0);
            }
            else if (yPos <= minBounds.y)
            {
                rectTransform.position = new Vector3(transform.position.x, transform.position.y + step, 0);
            } else if (yPos >= maxBounds.y)
            {
                rectTransform.position = new Vector3(transform.position.x, transform.position.y - step, 0);
            }
        }
    }
}
