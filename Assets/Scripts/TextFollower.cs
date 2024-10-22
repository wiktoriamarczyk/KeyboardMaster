using System;
using TMPro;
using UnityEngine;

public class TextFollower : MonoBehaviour
{
    [SerializeField] TMP_Text                       textDisplay;
    [SerializeField] float                          letterTimeLimit = 2f;
    [SerializeField] UserInterfaceItemsActivator    textBoxActivator;
    [SerializeField] int                            scoreModifierOnMiss = 1;
    [SerializeField] int                            scoreModifier = 2;

    public Action<int> onScoreChanged;

    string  targetText;
    int     currentLetterIndex = 0;
    float   letterTimer = 0f;

    const int singleHTMLInstructLength = 23;    // <color=#RRGGBB></color> - 23 characters

    void Start()
    {
        // TODO: targetText load from a file
        targetText = textDisplay.text;
    }

    void Update()
    {
        if (currentLetterIndex >= targetText.Length)
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
                if (char.IsLetter(c) || c == ' ')
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
            textDisplay.text = ReplaceWithColor(textDisplay.text, currentLetterIndex, Color.green);
            score = scoreModifier;
        }
        else
        {
            textDisplay.text = ReplaceWithColor(textDisplay.text, currentLetterIndex, Color.red);
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
        textDisplay.text = ReplaceWithColor(textDisplay.text, currentLetterIndex, Color.black);
        onScoreChanged?.Invoke(-scoreModifierOnMiss);
        NextLetter();
    }

    void NextLetter()
    {
        if (currentLetterIndex < targetText.Length)
        {
            currentLetterIndex++;
            letterTimer = 0f;
        }
    }
}
