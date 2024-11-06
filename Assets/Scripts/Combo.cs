using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Combo : MonoBehaviour
{
    private int comboCounter = 0;
    private float attackPower = 1f;
    private float pulseScale = 1.2f; 
    private float pulseDuration = 0.3f;
    private float shakeMagnitude = 5f;       
    private float shakeDuration = 0.1f;      
    private int shakeTweenId;
    private int complementShakeTweenId;

    [SerializeField] private TextMeshProUGUI comboText;
    [SerializeField] private TextMeshProUGUI complementText;

    private void Start()
    {
        ResetCombo();
        comboText.text = "";
        complementText.text = "";
        StartCoroutine(ChangeColor());
    }

    public void IncrementCombo()
    {
        comboCounter++;
        attackPower *=comboCounter;
        Debug.Log($"Attack Power: {attackPower}");

        if (comboCounter > 1)
        {
            comboText.text = $"x{comboCounter}";
            if (comboCounter > 9) 
            {
                complementText.text = $"GURL U SLAYIN' IT!";
            } 
            else if (comboCounter > 4)
            {
                complementText.text = $"AMAZING!!!";
            } 
            else {
                complementText.text = $"WOW";
            }
            PulseText();
            StartShake();
            StartShakeForComplementText();
        }
        else
        {
            comboText.text = ""; // Wyczyœæ tekst, jeœli comboCounter <= 2
            complementText.text = "";
            LeanTween.scale(comboText.rectTransform, Vector3.one, pulseDuration).setEase(LeanTweenType.easeOutQuad);
            StopShake();
            StopShakeForComplementText();
        }
    }

    public void ResetCombo()
    {
        comboCounter = 0;
        attackPower = 1f;
        Debug.Log("Combo Reset. Attack Power reset to base.");
        comboText.text = "";
        complementText.text = "";
        comboText.color = Color.white;
        comboText.rectTransform.localScale = Vector3.one;
        StopShake();
        StopShakeForComplementText();
    }

    private void PulseText()
    {
        comboText.rectTransform.localScale = Vector3.one;
        LeanTween.scale(comboText.rectTransform, Vector3.one * pulseScale, pulseDuration).setEase(LeanTweenType.easeOutElastic)
            .setOnComplete(() =>
            {
                LeanTween.scale(comboText.rectTransform, Vector3.one, pulseDuration).setEase(LeanTweenType.easeOutElastic);
            });
    }

    private void StartShake()
    {
        // SprawdŸ, czy ju¿ nie ma aktywnego efektu trzêsienia
        if (LeanTween.isTweening(shakeTweenId)) return;

        // Zainicjuj efekt trzêsienia jako powtarzalny, tworz¹c losowe ruchy wokó³ oryginalnej pozycji
        shakeTweenId = LeanTween.moveLocalX(comboText.gameObject, shakeMagnitude, shakeDuration)
            .setEase(LeanTweenType.easeShake)
            .setLoopPingPong()
            .setRepeat(-1) // Powtarzaj w nieskoñczonoœæ
            .id;

        LeanTween.moveLocalY(comboText.gameObject, shakeMagnitude, shakeDuration)
            .setEase(LeanTweenType.easeShake)
            .setLoopPingPong()
            .setRepeat(-1);
    }

    private void StopShake()
    {
        // Zatrzymaj trzêsienie
        LeanTween.cancel(comboText.gameObject);
        comboText.rectTransform.localPosition = Vector3.zero; // Resetuj pozycjê tekstu
    }

    private void StartShakeForComplementText()
    {
        if (LeanTween.isTweening(complementShakeTweenId)) return;

        complementShakeTweenId = LeanTween.moveLocalX(complementText.gameObject, shakeMagnitude, shakeDuration)
            .setEase(LeanTweenType.easeShake)
            .setLoopPingPong()
            .setRepeat(-1)
            .id;

        LeanTween.moveLocalY(complementText.gameObject, shakeMagnitude, shakeDuration)
            .setEase(LeanTweenType.easeShake)
            .setLoopPingPong()
            .setRepeat(-1);
    }

    private void StopShakeForComplementText()
    {
        LeanTween.cancel(complementText.gameObject);
        complementText.rectTransform.localPosition = Vector3.zero;
    }
    private IEnumerator ChangeColor()
    {
        float hue = 0f;
        while (true)
        {
            hue += 0.002f; // Powolna, p³ynna zmiana odcienia dla p³ynniejszego przejœcia
            if (hue > 1f) hue = 0f; // Przechodzimy ponownie przez kolory têczy, gdy osi¹gniemy pe³en zakres

            if (comboCounter > 1) // Zmieniaj kolor tylko wtedy, gdy comboCounter > 1
            {
                comboText.color = Color.HSVToRGB(hue, 1f, 1f); // Ustawienie koloru w przestrzeni HSV
            }
            yield return null; // Przechodzenie do nastêpnej klatki
        }
    }

    public float GetCurrentAttackPower()
    {
        return attackPower;
    }
}
