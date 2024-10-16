using UnityEngine;
using UnityEngine.EventSystems;

public class DragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    public Canvas canvas;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        canvasGroup.alpha = 0.6f;  // Make the image semi-transparent while dragging
        canvasGroup.blocksRaycasts = false;  // Allow the image to pass through raycasts during drag
    }

    public void OnDrag(PointerEventData eventData)
    {
        // Update position, taking into account the Canvas scale
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.alpha = 1.0f;  // Restore full opacity
        canvasGroup.blocksRaycasts = true;  // Re-enable raycasts when the drag is done
    }
}