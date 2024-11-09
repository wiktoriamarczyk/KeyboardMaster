using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Timers : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI timerFireball;
    [SerializeField] TextMeshProUGUI timerLight;
    [SerializeField] TextMeshProUGUI timerDrink;
    [SerializeField] TextMeshProUGUI timerDefend;

    private float remainingTime;
    private bool isRunning;

    public void StartTimer(float cooldownDuration)
    {
        remainingTime = cooldownDuration;
        isRunning = true;
        UpdateDisplay();
    }

    void Update()
    {
        if (isRunning)
        {
            remainingTime -= Time.deltaTime;
            UpdateDisplay();

            if (remainingTime <= 0)
            {
                remainingTime = 0;
                isRunning = false;
                UpdateDisplay(); 
            }
        }
    }

    private void UpdateDisplay()
    {
        if (timerFireball != null)
        {
            timerFireball.text = remainingTime > 0 ? remainingTime.ToString("F1") + "s" : "Ready";
        }
    }

    public bool IsReady()
    {
        return remainingTime <= 0;
    }
}
