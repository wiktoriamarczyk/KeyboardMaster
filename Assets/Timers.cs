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

    private float remainingFireballTime;
    private float remainingLightTime;
    private float remainingDrinkTime;
    private float remainingDefendTime;

    private bool isFireballRunning;
    private bool isLightRunning;
    private bool isDrinkRunning;
    private bool isDefendRunning;

    public void StartTimer(float cooldownDuration, int value)
    {
        // Rozpoczyna timer dla odpowiedniej umiejêtnoœci
        switch (value)
        {
            case 0:
                remainingFireballTime = cooldownDuration;
                isFireballRunning = true;
                ActivateTimerText(timerFireball, "Fireball Cooldown: ");
                break;
            case 1:
                remainingLightTime = cooldownDuration;
                isLightRunning = true;
                ActivateTimerText(timerLight, "Lightning Cooldown: ");
                break;
            case 2:
                remainingDrinkTime = cooldownDuration;
                isDrinkRunning = true;
                ActivateTimerText(timerDrink, "Drink potion Cooldown: ");
                break;
            case 3:
                remainingDefendTime = cooldownDuration;
                isDefendRunning = true;
                ActivateTimerText(timerDefend, "Defense active for: ");
                break;
            default:
                Debug.LogWarning("Nieprawid³owa wartoœæ timera.");
                break;
        }
    }

    void Update()
    {
        UpdateTimer(ref remainingFireballTime, ref isFireballRunning, timerFireball);
        UpdateTimer(ref remainingLightTime, ref isLightRunning, timerLight);
        UpdateTimer(ref remainingDrinkTime, ref isDrinkRunning, timerDrink);
        UpdateTimer(ref remainingDefendTime, ref isDefendRunning, timerDefend);
    }

    private void UpdateTimer(ref float remainingTime, ref bool isRunning, TextMeshProUGUI timerText)
    {
        if (isRunning)
        {
            remainingTime -= Time.deltaTime;

            if (remainingTime <= 0)
            {
                remainingTime = 0;
                isRunning = false;
                timerText.text = "Ready";
                timerText.gameObject.SetActive(false); // Dezaktywacja tekstu po zakoñczeniu
            }
            else
            {
                // Aktualizuje tekst z pozosta³ym czasem
                timerText.text = timerText.text.Split(':')[0] + ": " + remainingTime.ToString("F1") + "s";
            }
        }
    }

    private void ActivateTimerText(TextMeshProUGUI timerText, string skillName)
    {
        timerText.text = skillName;
        timerText.gameObject.SetActive(true); // Aktywacja tekstu na pocz¹tku odliczania
    }

    public bool IsReady(int value)
    {
        // Sprawdza, czy dany timer jest gotowy
        return value switch
        {
            0 => !isFireballRunning,
            1 => !isLightRunning,
            2 => !isDrinkRunning,
            3 => !isDefendRunning,
            _ => false
        };
    }
}
