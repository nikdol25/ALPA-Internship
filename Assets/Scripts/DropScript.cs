using UnityEngine;
using UnityEngine.EventSystems;

public class DropScript : MonoBehaviour, IDropHandler
{
    public GameController gameController; // Reference to the GameController
    public int dropZoneIndex; // Index of this drop zone (linked to a specific animal)

    public void OnDrop(PointerEventData eventData)
    {
        GameObject droppedObject = eventData.pointerDrag;
        if (droppedObject != null)
        {
            string droppedAnimalName = droppedObject.name;
            gameController.CheckIfCorrectDrop(droppedAnimalName, dropZoneIndex);
        }
    }
}

