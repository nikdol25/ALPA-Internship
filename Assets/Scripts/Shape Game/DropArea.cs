using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DropArea : MonoBehaviour, IDropHandler
{
    private Image dropAreaImage;
    private Color expectedColor;

    void Start()
    {
        dropAreaImage = GetComponent<Image>();

        // Get the expected color from the manager
        DraggableShapesManager manager = FindObjectOfType<DraggableShapesManager>();
        if (manager != null)
        {
            expectedColor = manager.GetAssignedColor(dropAreaImage);
        }
        else
        {
            expectedColor = dropAreaImage.color; // Fallback
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        GameObject droppedObject = eventData.pointerDrag;
        if (droppedObject != null)
        {
            Image draggableImage = droppedObject.GetComponent<Image>();
            Color draggableColor = draggableImage.color;

            if (draggableColor == expectedColor)
            {
                // Snap the shape to the drop zone
                droppedObject.transform.SetParent(transform);
                droppedObject.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;

                // Disable dragging for this shape
                DraggableShape draggable = droppedObject.GetComponent<DraggableShape>();
                if (draggable != null)
                {
                    draggable.enabled = false;
                }

                Debug.Log($"{droppedObject.name} correctly placed in {name} Drop Zone.");
            }
            else
            {
                // Return the shape to its original position
                droppedObject.GetComponent<DraggableShape>().ReturnToStart();
                Debug.LogWarning($"{droppedObject.name} does not match the color of {name} Drop Zone.");
            }
        }
    }
}