using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using System.Collections;

public class Draggable : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private Vector3 originalPosition;
    private Transform originalParent;
    private CanvasGroup canvasGroup;
    private bool isDragging = false;

    void Start()
    {
        originalParent = transform.parent;
        originalPosition = transform.position;

        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        isDragging = true;
        canvasGroup.blocksRaycasts = false;
        transform.SetParent(originalParent.parent);
        StartCoroutine(PlayDragSoundWithDelay(0.1f));
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (isDragging)
        {
            transform.position = Input.mousePosition;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        isDragging = false;
        canvasGroup.blocksRaycasts = true;

        if (eventData.pointerEnter != null && eventData.pointerEnter.CompareTag("DropZone"))
        {
            Transform dropZone = eventData.pointerEnter.transform;

            int dropZoneIndex = dropZone.GetSiblingIndex();
            TMP_Text draggableText = GetComponentInChildren<TMP_Text>();

            if (draggableText != null && draggableText.text == GameManager.Instance.GetExpectedLetter(dropZoneIndex).ToString())
            {
                transform.SetParent(dropZone);
                transform.position = dropZone.position;
                GameManager.Instance.PlayCorrectSound();
            }
            else
            {
                transform.SetParent(dropZone);
                transform.position = dropZone.position;
                GameManager.Instance.PlayIncorrectSound();
            }
        }
        else
        {
            ResetPosition();
        }

        GameManager.Instance.CheckWordCompletion();
    }

    private void ResetPosition()
    {
        transform.SetParent(originalParent);
        transform.position = originalPosition;
    }

    private IEnumerator PlayDragSoundWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (isDragging)
        {
            GameManager.Instance.PlayLetterClickSound();
        }
    }
}