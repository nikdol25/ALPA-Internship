using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameController : MonoBehaviour
{
    [System.Serializable]
    public class Animal
    {
        public string name;
        public Sprite image;
        public AudioClip sound;
    }

    public List<Animal> animals;
    public List<Image> animalSlots;
    public List<GameObject> draggableAnimals;
    public AudioSource audioSource;
    public AudioClip incorrectSound;
    public AudioClip correctSound;
    public Button replayButton;
    public Canvas canvas;
    public GameObject victoryPanel;

    private List<Animal> currentLevelAnimals = new List<Animal>();
    private Dictionary<GameObject, Vector2> initialPositions = new Dictionary<GameObject, Vector2>();
    private int currentLevel = 1;
    private int correctMatches = 0;
    private bool isPlayingSequence = false;

    private void Start()
    {
        LoadLevel(currentLevel);
        replayButton.onClick.AddListener(ReplaySounds);
        victoryPanel.SetActive(false);
    }

    public void LoadLevel(int level)
    {
        correctMatches = 0;
        currentLevelAnimals.Clear();
        initialPositions.Clear();
        int numberOfAnimals = GetNumberOfAnimalsForLevel(level);

        List<Animal> shuffledAnimals = new List<Animal>(animals);
        shuffledAnimals.Shuffle();
        currentLevelAnimals = shuffledAnimals.GetRange(0, numberOfAnimals);

        StoreInitialPositions();

        ActivateDropZones(numberOfAnimals);

        StartCoroutine(PlaySoundsInSequence());
    }

    private void StoreInitialPositions()
    {
        foreach (GameObject draggable in draggableAnimals)
        {
            RectTransform rectTransform = draggable.GetComponent<RectTransform>();
            initialPositions[draggable] = rectTransform.anchoredPosition;
        }
    }

    private void ResetAnimalPositions()
    {
        foreach (var item in initialPositions)
        {
            RectTransform rectTransform = item.Key.GetComponent<RectTransform>();
            rectTransform.anchoredPosition = item.Value;
        }
    }

    private IEnumerator PlaySoundsInSequence()
    {
        if (!isPlayingSequence)
        {
            isPlayingSequence = true;

            List<Animal> animalsToPlay = new List<Animal>(currentLevelAnimals);

            foreach (var animal in animalsToPlay)
            {
                audioSource.PlayOneShot(animal.sound);
                yield return new WaitForSeconds(2f);
            }

            isPlayingSequence = false;
        }
    }

    public void ReplaySounds()
    {
        StartCoroutine(PlaySoundsInSequence());
    }

    private int GetNumberOfAnimalsForLevel(int level)
    {
        switch (level)
        {
            case 1: return 2;
            case 2:
            case 3: return 3;
            case 4: return 4;
            default: return 2;
        }
    }

    private void ActivateDropZones(int numberOfDropZones)
    {
        for (int i = 0; i < animalSlots.Count; i++)
        {
            if (i < numberOfDropZones)
            {
                animalSlots[i].gameObject.SetActive(true);
            }
            else
            {
                animalSlots[i].gameObject.SetActive(false);
            }
        }
    }

    public void CheckIfCorrectDrop(string droppedAnimalName, int dropZoneIndex)
    {
        if (dropZoneIndex >= 0 && dropZoneIndex < currentLevelAnimals.Count)
        {
            if (droppedAnimalName == currentLevelAnimals[dropZoneIndex].name)
            {
                Debug.Log("Correct match!");

                RectTransform dropZoneRectTransform = animalSlots[dropZoneIndex].GetComponent<RectTransform>();
                GameObject droppedObject = GameObject.Find(droppedAnimalName);
                droppedObject.GetComponent<RectTransform>().anchoredPosition = dropZoneRectTransform.anchoredPosition;

                correctMatches++;

                PlayCorrectSound();

                if (correctMatches == currentLevelAnimals.Count)
                {
                    OnNextLevel();
                }
            }
            else
            {
                Debug.Log("Incorrect match, try again.");
                PlayIncorrectSound();
            }
        }
        else
        {

            Debug.LogError("Drop zone index is out of range: " + dropZoneIndex);
        }
    }

    private void PlayIncorrectSound()
    {
        audioSource.PlayOneShot(incorrectSound);
    }

    private void PlayCorrectSound()
    {
        audioSource.PlayOneShot(correctSound);
    }

    public void OnNextLevel()
    {
        Debug.Log("All animals matched! Moving to next level...");

        ResetAnimalPositions();

        currentLevel++;

        if (currentLevel > 4)
        {
            ShowVictoryScreen();
        }
        else
        {
            LoadLevel(currentLevel);
        }
    }

    private void ShowVictoryScreen()
    {
        Debug.Log("All levels completed. Victory!");
        victoryPanel.SetActive(true);
    }
}

public static class ListExtensions
{
    private static System.Random rng = new System.Random();

    public static void Shuffle<T>(this IList<T> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }
}