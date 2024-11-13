using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DropArea : MonoBehaviour, IDropHandler
{
    public string shapeName;
    private Image dropAreaImage;
    private ShapeManager shapeManager;

    void Start()
    {
        dropAreaImage = GetComponent<Image>();
        shapeManager = FindObjectOfType<ShapeManager>();
        dropAreaImage.color = shapeManager.GetShapeColor(shapeName);
    }

    public void OnDrop(PointerEventData eventData)
    {
        GameObject droppedObject = eventData.pointerDrag;
        if (droppedObject != null)
        {
            DraggableShape draggableShape = droppedObject.GetComponent<DraggableShape>();
            Image draggableImage = droppedObject.GetComponent<Image>();

            // Check if shape and color match
            if (draggableShape != null && draggableImage.color == dropAreaImage.color && droppedObject.name == shapeName)
            {
                droppedObject.transform.SetParent(transform);
                droppedObject.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
                GameOrganizer.Instance.CheckWinCondition();
            }
        }
    }
}