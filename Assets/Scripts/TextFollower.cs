using System.Collections;
using TMPro;
using UnityEngine;

public class TextFollower : MonoBehaviour
{
    [SerializeField] TMP_Text textDisplay;
    [SerializeField] TMP_Text scoreText;
    [SerializeField] float letterTimeLimit = 2f;

    string targetText;
    int score = 0;
    int currentLetterIndex = 0;
    float letterTimer = 0f;
    bool awaitingInput = true;

    const int htmlCodeOffset = 23;    // <color=#RRGGBB></color> - 23 characters

    void Start()
    {
        targetText = textDisplay.text;
        UpdateScoreText();
    }

    void Update()
    {
        letterTimer += Time.deltaTime;

        if (letterTimer >= letterTimeLimit && awaitingInput)
        {
            MissedLetter();
        }

        if (awaitingInput && Input.anyKeyDown)
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
        awaitingInput = false;
        char correctLetter = targetText[currentLetterIndex];

        if (char.ToLower(userInputLetter) == char.ToLower(correctLetter))
        {
            textDisplay.text = ReplaceWithColor(textDisplay.text, currentLetterIndex, Color.green);
            score++;
            Debug.Log("git wpisane");
        }
        else
        {
            textDisplay.text = ReplaceWithColor(textDisplay.text, currentLetterIndex, Color.red);
            score--;
            Debug.Log("dupa");
        }

        UpdateScoreText();
        StartCoroutine(NextLetterDelay());
    }

    string ReplaceWithColor(string text, int index, Color color)
    {
        int finalIndex = htmlCodeOffset * currentLetterIndex + index;

        string coloredText = text.Substring(0, finalIndex);

        coloredText += $"<color=#{ColorUtility.ToHtmlStringRGB(color)}>{text[finalIndex]}</color>";

        coloredText += text.Substring(finalIndex + 1);

        return coloredText;
    }

    void MissedLetter()
    {
        Debug.Log("miss");
        textDisplay.text = ReplaceWithColor(textDisplay.text, currentLetterIndex, Color.black);
        score--;
        UpdateScoreText();

        awaitingInput = false;
        StartCoroutine(NextLetterDelay());
    }

    IEnumerator NextLetterDelay()
    {
        yield return new WaitForSeconds(0.5f);

        currentLetterIndex++;

        if (currentLetterIndex < targetText.Length)
        {
            awaitingInput = true;
            letterTimer = 0f;
        }
        else
        {
            Debug.Log("dupa koniec");
        }
    }

    void UpdateScoreText()
    {
        scoreText.text = score.ToString();
    }
}
