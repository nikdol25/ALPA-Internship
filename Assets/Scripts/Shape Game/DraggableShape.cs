using UnityEngine;
using UnityEngine.EventSystems;

public class DraggableShape : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public string shapeType;

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
        DraggableShapesManager manager = FindObjectOfType<DraggableShapesManager>();
        if (manager != null)
        {
            originalPosition = manager.GetInitialPosition(rectTransform);
        }
        else
        {
            originalPosition = rectTransform.anchoredPosition;
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