using UnityEngine;
using UnityEngine.EventSystems;

public class DropScript : MonoBehaviour, IDropHandler
{
    public GameController gameController;
    public int dropZoneIndex;

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

