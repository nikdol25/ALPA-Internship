using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public Image pictureDisplay;
    public Transform letterContainer;
    public Transform dropZoneContainer;
    public GameObject letterPrefab;
    public GameObject dropZonePrefab;
    public AudioClip letterClickSound;

    public AudioClip correctSound;
    public AudioClip incorrectSound;
    private AudioSource audioSource;

    private List<string> words = new List<string> { "JALGRATAS", "RONG", "BUSS", "AUTO", "LENNUK", "LAEV" };
    private string currentWord;
    private int wordIndex = 0;

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

    void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        ShuffleWords();
        LoadNextWord();
    }

    private void ShuffleWords()
    {
        words.Shuffle();
        words = words.GetRange(0, Mathf.Min(4, words.Count));
    }

    private void LoadNextWord()
    {
        if (wordIndex >= words.Count)
        {
            Debug.Log("Game Completed!");
            return;
        }

        currentWord = words[wordIndex];
        wordIndex++;

        pictureDisplay.sprite = Resources.Load<Sprite>(currentWord);

        SetupLetters(currentWord);
    }

    private void SetupLetters(string word)
    {
        foreach (Transform child in letterContainer) Destroy(child.gameObject);
        foreach (Transform child in dropZoneContainer) Destroy(child.gameObject);

        for (int i = 0; i < word.Length; i++)
        {
            Instantiate(dropZonePrefab, dropZoneContainer);
        }

        List<char> letters = new List<char>(word.ToCharArray());
        letters.AddRange(GenerateExtraLetters(word, 3));
        letters.Shuffle();

        foreach (char letter in letters)
        {
            GameObject letterObj = Instantiate(letterPrefab, letterContainer);
            TMP_Text textComponent = letterObj.GetComponentInChildren<TMP_Text>();
            if (textComponent != null)
            {
                textComponent.text = letter.ToString();
            }
        }
    }

    private List<char> GenerateExtraLetters(string word, int count)
    {
        string alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        List<char> extraLetters = new List<char>();

        while (extraLetters.Count < count)
        {
            char randomLetter = alphabet[Random.Range(0, alphabet.Length)];
            if (!word.Contains(randomLetter.ToString()) && !extraLetters.Contains(randomLetter))
            {
                extraLetters.Add(randomLetter);
            }
        }
        return extraLetters;
    }

    public void PlayCorrectSound()
    {
        if (correctSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(correctSound);
        }
    }

    public void PlayIncorrectSound()
    {
        if (incorrectSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(incorrectSound);
        }
    }

    public void PlayLetterClickSound()
    {
        if (letterClickSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(letterClickSound);
        }
    }

    public char GetExpectedLetter(int index)
    {
        if (index >= 0 && index < currentWord.Length)
        {
            return currentWord[index];
        }
        return '\0';
    }

    public void CheckWordCompletion()
    {
        string formedWord = "";
        foreach (Transform dropZone in dropZoneContainer)
        {
            if (dropZone.childCount > 0)
            {
                TMP_Text textComponent = dropZone.GetChild(0).GetComponentInChildren<TMP_Text>();
                if (textComponent != null)
                {
                    formedWord += textComponent.text;
                }
            }
        }

        if (formedWord == currentWord)
        {
            Debug.Log("Word Completed: " + currentWord);
            LoadNextWord();
        }
    }
}