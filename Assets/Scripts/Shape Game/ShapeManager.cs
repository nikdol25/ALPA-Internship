using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class ShapeManager : MonoBehaviour
{
    public Image circleImage;
    public Image triangleImage;
    public Image hexagonImage;
    public Image squareImage;

    private Color[] colors = { Color.black, Color.blue, Color.yellow, Color.green };
    private Dictionary<string, Color> shapeColors = new Dictionary<string, Color>();

    void Start()
    {
        AssignRandomColors();
    }

    void AssignRandomColors()
    {
        shapeColors["Circle"] = colors[Random.Range(0, colors.Length)];
        shapeColors["Triangle"] = colors[Random.Range(0, colors.Length)];
        shapeColors["Hexagon"] = colors[Random.Range(0, colors.Length)];
        shapeColors["Square"] = colors[Random.Range(0, colors.Length)];

        circleImage.color = shapeColors["Circle"];
        triangleImage.color = shapeColors["Triangle"];
        hexagonImage.color = shapeColors["Hexagon"];
        squareImage.color = shapeColors["Square"];
    }

    public Color GetShapeColor(string shapeName)
    {
        return shapeColors.ContainsKey(shapeName) ? shapeColors[shapeName] : Color.white;
    }
}