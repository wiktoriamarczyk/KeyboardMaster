using System;
using TMPro;
using UnityEngine;
using System.IO;
using System.Text.RegularExpressions;

public class TextFollower : MonoBehaviour
{
    [SerializeField] TMP_Text                       textDisplay;
    [SerializeField] UserInterfaceItemsActivator    textBoxActivator;
    [SerializeField] Combo                          combo;
    [SerializeField] float                          letterTimeLimit = 2f;
    [SerializeField] int                            scoreModifierOnMiss = 1;
    [SerializeField] int                            scoreModifier = 2;

    public Action<int> onScoreChanged;

    string  targetText;
    string  currentText;
    int     currentLetterIndex = 0;
    float   letterTimer = 0f;
    Regex   regex = new Regex(@"^[a-zA-Z0-9\W]$");

    const string wordsFile = "Assets/words-list.txt";

    void Start()
    {
        LoadTextFromFile(wordsFile);
        currentText = textDisplay.text = targetText;
    }

    void Update()
    {
        if (currentLetterIndex >= currentText.Length)
        {
            return;
        }

        letterTimer += Time.deltaTime;
        if (letterTimer >= letterTimeLimit)
        {
            MissedLetter();
        }

        bool isFocusOnTextBox = textBoxActivator.IsCurrentlyActivated;
        if (isFocusOnTextBox && Input.anyKeyDown)
        {
            foreach (char c in Input.inputString)
            {
                if (regex.IsMatch(c.ToString()) || c == ' ')
                {
                    CheckInput(c);
                    break;
                }
            }
        }
    }

    void LoadTextFromFile(string path)
    {
        if (path == null)
        {
            return;
        }
        targetText = File.ReadAllText(path).ToLower();
    }

    void CheckInput(char userInputLetter)
    {
        int score = 0;
        char correctLetter = targetText[currentLetterIndex];

        if (char.ToLower(userInputLetter) == char.ToLower(correctLetter))
        {
            score = scoreModifier;
            combo.IncrementCombo();
        }
        else
        {
            score = -scoreModifier;
            combo.ResetCombo();
        }

        onScoreChanged?.Invoke(score);
        NextLetter();
    }

    void MissedLetter()
    {
        onScoreChanged?.Invoke(-scoreModifierOnMiss);
        combo.ResetCombo();
        NextLetter();
    }

    void NextLetter()
    {
        if (currentLetterIndex < currentText.Length)
        {
            currentLetterIndex++;
            string newText = targetText.Substring(currentLetterIndex);
            textDisplay.text = currentText = newText;
            letterTimer = 0f;
        }
    }
}
