using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class Draggable : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private Vector3 originalPosition;
    private Transform originalParent;
    private CanvasGroup canvasGroup;

    void Start()
    {
        originalParent = transform.parent;
        originalPosition = transform.position;
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = false;
        transform.SetParent(originalParent.parent);
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;
        if (eventData.pointerEnter != null && eventData.pointerEnter.CompareTag("DropZone"))
        {
            transform.SetParent(eventData.pointerEnter.transform);
            transform.position = eventData.pointerEnter.transform.position;
            GameManager.Instance.CheckWordCompletion();
        }
        else
        {
            transform.SetParent(originalParent);
            transform.position = originalPosition;
        }
    }
}
