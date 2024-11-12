using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class Combo : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI comboText;
    [SerializeField] TextMeshProUGUI complementText;

    public static Action<int> onComboChanged;

    int comboCounter = 0;
    float attackPower = 1f;
    float pulseScale = 1.2f;
    float pulseDuration = 0.3f;
    float shakeMagnitude = 5f;
    float shakeDuration = 0.1f;
    int shakeTweenId;
    int complementShakeTweenId;

    enum ComboState
    {
        Amazing = 15,
        SlayinIt = 30
    }

    void Start()
    {
        ResetCombo();
        StartCoroutine(ChangeColor());
    }

    public void IncrementCombo()
    {
        comboCounter++;
        onComboChanged?.Invoke(comboCounter);

        if (comboCounter > 1)
        {
            comboText.text = $"x{comboCounter}";
            if (comboCounter >= (int)ComboState.SlayinIt)
            {
                complementText.text = $"GURL U SLAYIN' IT!";
            }
            else if (comboCounter >= (int)ComboState.Amazing)
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
            comboText.text = string.Empty;
            complementText.text = string.Empty;
            LeanTween.scale(comboText.rectTransform, Vector3.one, pulseDuration).setEase(LeanTweenType.easeOutQuad);
            StopShake();
            StopShakeForComplementText();
        }
    }

    public void ResetCombo()
    {
        comboCounter = 0;
        onComboChanged?.Invoke(comboCounter);
        attackPower = 1f;
        comboText.text = string.Empty;
        complementText.text = string.Empty;
        comboText.color = Color.white;
        comboText.rectTransform.localScale = Vector3.one;
        StopShake();
        StopShakeForComplementText();
    }

    public float GetCurrentAttackPower()
    {
        return attackPower;
    }

    void PulseText()
    {
        comboText.rectTransform.localScale = Vector3.one;
        LeanTween.scale(comboText.rectTransform, Vector3.one * pulseScale, pulseDuration).setEase(LeanTweenType.easeOutElastic)
            .setOnComplete(() =>
            {
                LeanTween.scale(comboText.rectTransform, Vector3.one, pulseDuration).setEase(LeanTweenType.easeOutElastic);
            });
    }

    void StartShake()
    {
        // Check if the text is already shaking
        if (LeanTween.isTweening(shakeTweenId)) return;

        // Initialize the shake effect as repeatable, creating random movements around the original position
        shakeTweenId = LeanTween.moveLocalX(comboText.gameObject, shakeMagnitude, shakeDuration)
            .setEase(LeanTweenType.easeShake)
            .setLoopPingPong()
            .setRepeat(-1) // Repeat infinitely
            .id;

        LeanTween.moveLocalY(comboText.gameObject, shakeMagnitude, shakeDuration)
            .setEase(LeanTweenType.easeShake)
            .setLoopPingPong()
            .setRepeat(-1);
    }

    void StopShake()
    {
        // Stop the shake effect
        LeanTween.cancel(comboText.gameObject);
        // Reset the text position to the original
        comboText.rectTransform.localPosition = Vector3.zero;
    }

    void StartShakeForComplementText()
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

    void StopShakeForComplementText()
    {
        LeanTween.cancel(complementText.gameObject);
        complementText.rectTransform.localPosition = Vector3.zero;
    }

    IEnumerator ChangeColor()
    {
        float hue = 0f;
        while (true)
        {
            // Continuous color change
            hue += 0.002f;
            // Reset hue when it reaches maximum value
            if (hue > 1f)
            {
                hue = 0f;
            }
            // Change color only when the combo is active
            if (comboCounter > 1)
            {
                comboText.color = Color.HSVToRGB(hue, 1f, 1f);
            }
            // Wait for the next frame
            yield return null;
        }
    }
}
