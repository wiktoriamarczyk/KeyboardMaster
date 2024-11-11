using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenEffects : MonoBehaviour
{
    [SerializeField] private float minInterval = 10f; // minimalny czas mi�dzy efektami
    [SerializeField] private float maxInterval = 20f; // maksymalny czas mi�dzy efektami
    [SerializeField] private float shakeDuration = 5f; // czas trwania efektu trz�sienia
    [SerializeField] private float shakeIntensity = 0.2f; // intensywno�� trz�sienia
    [SerializeField] public float fadeDuration = 1f; // Czas trwania fade in i fade out
    [SerializeField] public float waitTime = 2f;

    [SerializeField] public Image blackScreenImage; // panel UI do efektu czarnego ekranu

    private Camera mainCamera;
    private Vector3 originalCameraPosition;

    void Start()
    {
        mainCamera = Camera.main;
        originalCameraPosition = mainCamera.transform.position;
        if (blackScreenImage != null)
        {
            blackScreenImage.color = new Color(0, 0, 0, 0);  // Ustaw pocz�tkow� przezroczysto�� na 0
        }

        // Start losowego uruchamiania efekt�w
        StartCoroutine(ActivateRandomEffects());
    }

    private IEnumerator ActivateRandomEffects()
    {
        while (true)
        {
            // Poczekaj losow� ilo�� czasu przed w��czeniem efektu
            float interval = Random.Range(minInterval, maxInterval);
            yield return new WaitForSeconds(interval);

            // Wybierz losowy efekt do aktywacji
            int randomEffect = Random.Range(0, 2); // 0 to trz�sienie ekranu, 1 to czarny ekran

            if (randomEffect == 0)
            {
                StartCoroutine(ScreenShake());
                Debug.Log("SHAKE");
            }
            else if (randomEffect == 1)
            {
                StartCoroutine(Blackout());
                Debug.Log("BLACKOUT");
            }
        }
    }

    private IEnumerator ScreenShake()
    {
        float elapsedTime = 0f;

        while (elapsedTime < shakeDuration)
        {
            // Przemieszczanie kamery losowo w celu wywo�ania trz�sienia
            Vector3 randomOffset = Random.insideUnitSphere * shakeIntensity;
            mainCamera.transform.position = originalCameraPosition + randomOffset;

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Przywr�� oryginaln� pozycj� kamery
        mainCamera.transform.position = originalCameraPosition;
    }

    private IEnumerator Blackout()
    {
        if (blackScreenImage != null)
        {
            yield return StartCoroutine(Fade(0, 1)); // Fade to black
            yield return new WaitForSeconds(waitTime); // Wait in black screen
            yield return StartCoroutine(Fade(1, 0)); // Fade back to normal
        }
    }

    private IEnumerator Fade(float startAlpha, float endAlpha)
    {
        float elapsedTime = 0f;
        Color color = blackScreenImage.color;

        while (elapsedTime < fadeDuration)
        {
            color.a = Mathf.Lerp(startAlpha, endAlpha, elapsedTime / fadeDuration);
            blackScreenImage.color = color;
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        color.a = endAlpha;
        blackScreenImage.color = color;
    }
}

