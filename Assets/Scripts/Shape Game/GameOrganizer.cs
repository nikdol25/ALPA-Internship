using UnityEngine;

public class GameOrganizer : MonoBehaviour
{
    public static GameOrganizer Instance { get; private set; }
    private int totalShapes = 4;
    private int matchedShapes = 0;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void CheckWinCondition()
    {
        matchedShapes++;
        if (matchedShapes >= totalShapes)
        {
            WinGame();
        }
    }

    void WinGame()
    {
        Debug.Log("You Win!");
        // Implement further win game logic, such as showing a win screen or restarting the game
    }

    public void ResetGame()
    {
        matchedShapes = 0;
        // Reset shapes and colors if needed, or reload the scene
    }
}