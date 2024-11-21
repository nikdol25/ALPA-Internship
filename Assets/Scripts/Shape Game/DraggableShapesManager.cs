using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class DraggableShapesManager : MonoBehaviour
{
    public List<RectTransform> draggableShapes; // Assign original draggable shapes in the Inspector
    public List<RectTransform> duplicateShapes; // Assign duplicate draggable shapes in the Inspector
    public List<Image> dropAreaShapes; // Assign all drop area shapes in the Inspector

    private Dictionary<RectTransform, Vector2> initialPositions = new Dictionary<RectTransform, Vector2>();
    private Dictionary<Image, Color> assignedColors = new Dictionary<Image, Color>();

    private Color[] colors = {
        Color.red, Color.green, Color.blue, Color.yellow,
        new Color(0.6f, 0.3f, 0.0f), // brown
        new Color(0.5f, 0.0f, 0.5f), // purple
        Color.cyan, Color.magenta, Color.gray,
        Color.black, Color.white, // New colors
        new Color(1.0f, 0.65f, 0.0f), // orange
        new Color(0.54f, 0.17f, 0.89f), // indigo
        new Color(0.25f, 0.88f, 0.82f), // turquoise
        new Color(0.98f, 0.5f, 0.45f) // coral
    };

    void Start()
    {
        if (dropAreaShapes.Count > colors.Length)
        {
            Debug.LogWarning("Not enough unique colors for all drop zones. Some colors will repeat.");
        }

        RandomizeShapePositions();
        AssignColorsToShapes();
    }

    public void RandomizeShapePositions()
    {
        List<Vector2> positions = new List<Vector2>();

        // Store the original positions of the shapes
        foreach (RectTransform shape in draggableShapes)
        {
            positions.Add(shape.anchoredPosition);
        }
        foreach (RectTransform duplicate in duplicateShapes)
        {
            positions.Add(duplicate.anchoredPosition);
        }

        // Shuffle the positions
        ShuffleList(positions);

        // Assign the shuffled positions and store them as initial positions
        int index = 0;
        foreach (RectTransform shape in draggableShapes)
        {
            shape.anchoredPosition = positions[index];
            initialPositions[shape] = positions[index];
            index++;
        }
        foreach (RectTransform duplicate in duplicateShapes)
        {
            duplicate.anchoredPosition = positions[index];
            initialPositions[duplicate] = positions[index];
            index++;
        }
    }

    public void AssignColorsToShapes()
    {
        List<Color> availableColors = new List<Color>(colors);
        ShuffleList(availableColors);

        for (int i = 0; i < dropAreaShapes.Count; i++)
        {
            if (i >= draggableShapes.Count || i >= duplicateShapes.Count) break;

            // Assign a random color to each drop area shape
            Color assignedColor = availableColors[i % availableColors.Count];
            dropAreaShapes[i].color = assignedColor;

            // Assign the same color to the matching draggable shape
            Image draggableImage = draggableShapes[i].GetComponent<Image>();
            draggableImage.color = assignedColor;

            // Assign a different random color to the duplicate
            Color differentColor = GetRandomDifferentColor(availableColors, assignedColor);
            Image duplicateImage = duplicateShapes[i].GetComponent<Image>();
            duplicateImage.color = differentColor;

            // Store the assigned color
            assignedColors[dropAreaShapes[i]] = assignedColor;

            Debug.Log($"DropZone {dropAreaShapes[i].name} assigned color {ColorToString(assignedColor)}");
            Debug.Log($"Draggable Shape {draggableShapes[i].name} assigned color {ColorToString(assignedColor)}");
            Debug.Log($"Duplicate Shape {duplicateShapes[i].name} assigned different color {ColorToString(differentColor)}");
        }
    }

    public Vector2 GetInitialPosition(RectTransform shape)
    {
        return initialPositions.ContainsKey(shape) ? initialPositions[shape] : Vector2.zero;
    }

    public Color GetAssignedColor(Image dropAreaImage)
    {
        if (assignedColors.ContainsKey(dropAreaImage))
        {
            return assignedColors[dropAreaImage];
        }
        else
        {
            Debug.LogWarning($"Color not found for DropZone {dropAreaImage.name}. Defaulting to transparent.");
            return Color.clear; // Default to transparent if no color is found
        }
    }

    private Color GetRandomDifferentColor(List<Color> availableColors, Color excludeColor)
    {
        List<Color> filteredColors = new List<Color>(availableColors);
        filteredColors.Remove(excludeColor);
        return filteredColors[Random.Range(0, filteredColors.Count)];
    }

    private void ShuffleList<T>(List<T> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int randomIndex = Random.Range(0, i + 1);
            T temp = list[i];
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
    }

    private string ColorToString(Color color)
    {
        return $"R:{color.r:F2}, G:{color.g:F2}, B:{color.b:F2}, A:{color.a:F2}";
    }
}