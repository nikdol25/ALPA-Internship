using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DropArea : MonoBehaviour, IDropHandler
{
    public string shapeType; // Set this in the Inspector (e.g., "Square", "Circle")

    private Image dropAreaImage;
    private Color expectedColor;

    [Header("Audio Settings")]
    public AudioClip correctSound; // Assign in the Inspector
    public AudioClip incorrectSound; // Assign in the Inspector
    private AudioSource audioSource;

    void Start()
    {
        dropAreaImage = GetComponent<Image>();
        audioSource = gameObject.AddComponent<AudioSource>(); // Add AudioSource component at runtime

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

        Debug.Log($"{name} DropArea initialized with expected color {ColorToString(expectedColor)}");
    }

    public void OnDrop(PointerEventData eventData)
    {
        GameObject droppedObject = eventData.pointerDrag;
        if (droppedObject != null)
        {
            DraggableShape draggableShape = droppedObject.GetComponent<DraggableShape>();
            if (draggableShape != null)
            {
                string droppedShapeType = draggableShape.shapeType;
                Color draggableColor = draggableShape.GetComponent<Image>().color;

                Debug.Log($"Dropped {droppedObject.name} (Type: {droppedShapeType}) on {name} (Type: {shapeType}). Draggable Color: {ColorToString(draggableColor)}, Expected Color: {ColorToString(expectedColor)}");

                // Validate both shape type and color
                if (droppedShapeType == shapeType && ColorsMatch(draggableColor, expectedColor))
                {
                    // Snap the shape to the drop zone
                    droppedObject.transform.SetParent(transform);
                    droppedObject.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;

                    // Disable dragging for this shape
                    draggableShape.enabled = false;

                    // Play correct sound
                    PlaySound(correctSound);

                    Debug.Log($"{droppedObject.name} correctly placed in {name} Drop Zone.");
                }
                else
                {
                    // Return the shape to its original position
                    draggableShape.ReturnToStart();

                    // Play incorrect sound
                    PlaySound(incorrectSound);

                    Debug.LogWarning($"{droppedObject.name} does not match the type or color of {name} Drop Zone.");
                }
            }
        }
    }

    private void PlaySound(AudioClip clip)
    {
        if (clip != null && audioSource != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }

    private bool ColorsMatch(Color color1, Color color2, float tolerance = 0.01f)
    {
        bool rMatch = Mathf.Abs(color1.r - color2.r) <= tolerance;
        bool gMatch = Mathf.Abs(color1.g - color2.g) <= tolerance;
        bool bMatch = Mathf.Abs(color1.b - color2.b) <= tolerance;
        bool aMatch = Mathf.Abs(color1.a - color2.a) <= tolerance;

        Debug.Log($"Color Match Check: R: {rMatch}, G: {gMatch}, B: {bMatch}, A: {aMatch}");
        Debug.Log($"Compared Colors: Color1 = {ColorToString(color1)}, Color2 = {ColorToString(color2)}");

        return rMatch && gMatch && bMatch && aMatch;
    }

    private string ColorToString(Color color)
    {
        return $"R:{color.r:F2}, G:{color.g:F2}, B:{color.b:F2}, A:{color.a:F2}";
    }
}