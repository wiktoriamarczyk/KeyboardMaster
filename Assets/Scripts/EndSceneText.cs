using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EndSceneText : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI resultText;

    void Start()
    {
        if (SceneController.Instance.win)
        {
            resultText.text = "Congratulations! You won!";
            resultText.color = Color.green;
        }
        else
        {
            resultText.text = "Game Over! You lost!";
            resultText.color = Color.red;
        }
    }
}
