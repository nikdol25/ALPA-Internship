using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameController : MonoBehaviour
{
    [System.Serializable]
    public class Animal
    {
        public string name;     // Name of the animal
        public Sprite image;    // Image of the animal
        public AudioClip sound; // Sound of the animal
    }

    public List<Animal> animals; // List of all animals
    public List<Image> animalSlots;  // Drop zones (slots) where animal images are dropped
    public List<GameObject> draggableAnimals; // List of draggable animal game objects
    public AudioSource audioSource;  // AudioSource for playing sounds
    public AudioClip incorrectSound; // AudioClip for the incorrect placement sound effect
    public AudioClip correctSound;   // AudioClip for the correct placement sound effect
    public Button replayButton;  // Button to replay the sequence of sounds
    public Canvas canvas; // Reference to the Canvas

    private List<Animal> currentLevelAnimals = new List<Animal>(); // Animals used for the current level
    private Dictionary<GameObject, Vector2> initialPositions = new Dictionary<GameObject, Vector2>(); // Store initial positions
    private int currentLevel = 1;
    private int correctMatches = 0;  // Counter for correct matches
    private bool isPlayingSequence = false; // To prevent multiple plays at the same time

    private void Start()
    {
        LoadLevel(currentLevel); // Load the first level when the game starts

        // Add a listener to the replay button
        replayButton.onClick.AddListener(ReplaySounds);
    }

    // Method to load a level, randomize animals, and set up UI
    public void LoadLevel(int level)
    {
        correctMatches = 0; // Reset the correct match counter
        currentLevelAnimals.Clear(); // Clear the previous level's animals
        initialPositions.Clear(); // Clear the initial positions dictionary
        int numberOfAnimals = GetNumberOfAnimalsForLevel(level);

        // Shuffle the animal list and select animals for the current level
        List<Animal> shuffledAnimals = new List<Animal>(animals);
        shuffledAnimals.Shuffle();
        currentLevelAnimals = shuffledAnimals.GetRange(0, numberOfAnimals); // Get animals for this level

        // Store initial positions of the draggable animals
        StoreInitialPositions();

        // Activate the correct number of drop zones
        ActivateDropZones(numberOfAnimals);

        // Play the sounds in sequence at the start of the level
        StartCoroutine(PlaySoundsInSequence());
    }

    // Store the initial positions of the draggable animal game objects
    private void StoreInitialPositions()
    {
        foreach (GameObject draggable in draggableAnimals)
        {
            RectTransform rectTransform = draggable.GetComponent<RectTransform>();
            initialPositions[draggable] = rectTransform.anchoredPosition;
        }
    }

    // Reset all draggable animals to their initial positions
    private void ResetAnimalPositions()
    {
        foreach (var item in initialPositions)
        {
            RectTransform rectTransform = item.Key.GetComponent<RectTransform>();
            rectTransform.anchoredPosition = item.Value;
        }
    }

    // Coroutine to play animal sounds automatically when the level starts
    private IEnumerator PlaySoundsInSequence()
    {
        if (!isPlayingSequence) // Prevent playing multiple times simultaneously
        {
            isPlayingSequence = true;

            // Create a copy of the currentLevelAnimals list to safely enumerate over
            List<Animal> animalsToPlay = new List<Animal>(currentLevelAnimals);

            foreach (var animal in animalsToPlay)
            {
                audioSource.PlayOneShot(animal.sound);  // Play the sound of the current animal
                yield return new WaitForSeconds(2f);    // Wait for 2 seconds before playing the next sound
            }

            isPlayingSequence = false;
        }
    }

    // Method to replay the sounds when the replay button is clicked
    public void ReplaySounds()
    {
        StartCoroutine(PlaySoundsInSequence()); // Replay the sequence
    }

    // Method to determine the number of animals (and drop zones) based on the level
    private int GetNumberOfAnimalsForLevel(int level)
    {
        switch (level)
        {
            case 1: return 2;  // Level 1: 2 animals, 2 drop zones
            case 2:
            case 3: return 3;  // Level 2 and 3: 3 animals, 3 drop zones
            case 4: return 4;  // Level 4: 4 animals, 4 drop zones
            default: return 2; // Default case
        }
    }

    // Method to activate the correct number of drop zones for the level
    private void ActivateDropZones(int numberOfDropZones)
    {
        // Activate the required number of drop zones and hide the rest
        for (int i = 0; i < animalSlots.Count; i++)
        {
            if (i < numberOfDropZones)
            {
                animalSlots[i].gameObject.SetActive(true);  // Activate the drop zone
            }
            else
            {
                animalSlots[i].gameObject.SetActive(false); // Hide the drop zone
            }
        }
    }

    // Check if the dropped animal matches the correct animal in the drop zone
    public void CheckIfCorrectDrop(string droppedAnimalName, int dropZoneIndex)
    {
        // Ensure the index is within the valid range
        if (dropZoneIndex >= 0 && dropZoneIndex < currentLevelAnimals.Count)
        {
            // Check if the dropped animal matches the correct one for this drop zone
            if (droppedAnimalName == currentLevelAnimals[dropZoneIndex].name)
            {
                // Correct match, proceed with the game logic
                Debug.Log("Correct match!");

                // Snap the image into the correct drop zone
                RectTransform dropZoneRectTransform = animalSlots[dropZoneIndex].GetComponent<RectTransform>();
                GameObject droppedObject = GameObject.Find(droppedAnimalName);
                droppedObject.GetComponent<RectTransform>().anchoredPosition = dropZoneRectTransform.anchoredPosition;

                // Increment correct match counter
                correctMatches++;

                // Play correct sound effect
                PlayCorrectSound();

                // Check if all animals are matched correctly
                if (correctMatches == currentLevelAnimals.Count)
                {
                    OnNextLevel();
                }
            }
            else
            {
                // Incorrect match
                Debug.Log("Incorrect match, try again.");
                PlayIncorrectSound(); // Play incorrect sound effect
            }
        }
        else
        {
            // Index is out of range, handle the error
            Debug.LogError("Drop zone index is out of range: " + dropZoneIndex);
        }
    }

    // Play sound effect for incorrect drop
    private void PlayIncorrectSound()
    {
        audioSource.PlayOneShot(incorrectSound);  // Play incorrect placement sound
    }

    // Play sound effect for correct drop
    private void PlayCorrectSound()
    {
        audioSource.PlayOneShot(correctSound);  // Play correct placement sound
    }

    // Move to the next level after all correct matches
    public void OnNextLevel()
    {
        Debug.Log("All animals matched! Moving to next level...");

        // Reset positions of animals before moving to next level
        ResetAnimalPositions();

        currentLevel++;

        // If you've reached the maximum level, you can restart from Level 1 or end the game
        if (currentLevel > 4)
        {
            Debug.Log("All levels completed. Restarting from Level 1.");
            currentLevel = 1;
        }

        // Load the next level
        LoadLevel(currentLevel);
    }
}

// Top-level static class to define extension methods
public static class ListExtensions
{
    private static System.Random rng = new System.Random();

    // Extension method to shuffle any list
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