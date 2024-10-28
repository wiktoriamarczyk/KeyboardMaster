using System;
using TMPro;
using UnityEngine;
using System.IO;
using System.Text.RegularExpressions;

public class TextFollower : MonoBehaviour
{
    [SerializeField] TMP_Text                       textDisplay;
    [SerializeField] float                          letterTimeLimit = 2f;
    [SerializeField] UserInterfaceItemsActivator    textBoxActivator;
    [SerializeField] int                            scoreModifierOnMiss = 1;
    [SerializeField] int                            scoreModifier = 2;

    public Action<int> onScoreChanged;

    string  targetText;
    string  currentText;
    int     currentLetterIndex = 0;
    float   letterTimer = 0f;
    Regex   regex = new Regex(@"^[a-zA-Z0-9\W]$");

    // <color=#RRGGBB></color> - 23 characters
    const int singleHTMLInstructLength = 23;
    const string wordsFile = "Assets/words-list.txt";

    void Start()
    {
        LoadTextFromFile(wordsFile);
        currentText = textDisplay.text = targetText;
    }

    void LoadTextFromFile(string path)
    {
        if (path == null)
        {
            return;
        }
        targetText = File.ReadAllText(path).ToLower();
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

    void CheckInput(char userInputLetter)
    {
        int score = 0;
        char correctLetter = targetText[currentLetterIndex];

        if (char.ToLower(userInputLetter) == char.ToLower(correctLetter))
        {
            score = scoreModifier;
        }
        else
        {
            score = -scoreModifier;
        }

        onScoreChanged?.Invoke(score);
        NextLetter();
    }

    string ReplaceWithColor(string text, int index, Color color)
    {
        // all letters before the current letter are colored
        int finalIndex = singleHTMLInstructLength * currentLetterIndex + index;

        string coloredText = text.Substring(0, finalIndex);
        coloredText += $"<color=#{ColorUtility.ToHtmlStringRGB(color)}>{text[finalIndex]}</color>";
        coloredText += text.Substring(finalIndex + 1);

        return coloredText;
    }

    void MissedLetter()
    {
        onScoreChanged?.Invoke(-scoreModifierOnMiss);
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
