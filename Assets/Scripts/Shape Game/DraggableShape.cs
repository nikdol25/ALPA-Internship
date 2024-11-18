using UnityEngine;
using UnityEngine.EventSystems;

public class DraggableShape : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    private Vector2 originalPosition;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
    }

    void Start()
    {
        // Ask the DraggableShapesManager for the initial position
        DraggableShapesManager manager = FindObjectOfType<DraggableShapesManager>();
        if (manager != null)
        {
            originalPosition = manager.GetInitialPosition(rectTransform);
        }
        else
        {
            originalPosition = rectTransform.anchoredPosition; // Fallback
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta / canvasGroup.transform.lossyScale;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;

        // If the object is not dropped in a valid drop zone, return it to its original position
        if (transform.parent == transform.root)
        {
            ReturnToStart();
        }
    }

    public void ReturnToStart()
    {
        rectTransform.anchoredPosition = originalPosition;
    }
}