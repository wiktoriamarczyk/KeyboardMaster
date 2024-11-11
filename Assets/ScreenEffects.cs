using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenEffects : MonoBehaviour
{
    [SerializeField] private float minInterval = 10f; // minimalny czas miêdzy efektami
    [SerializeField] private float maxInterval = 20f; // maksymalny czas miêdzy efektami
    [SerializeField] private float shakeDuration = 3f; // czas trwania efektu trzêsienia
    [SerializeField] private float shakeIntensity = 0.2f; // intensywnoœæ trzêsienia
    [SerializeField] private float blackoutDuration = 2f; // czas trwania efektu czarnego ekranu
    [SerializeField] private GameObject blackoutPanel; // panel UI do efektu czarnego ekranu

    private Camera mainCamera;
    private Vector3 originalCameraPosition;
    private CanvasGroup blackoutCanvasGroup;

    void Start()
    {
        mainCamera = Camera.main;
        originalCameraPosition = mainCamera.transform.position;

        if (blackoutPanel != null)
        {
            blackoutCanvasGroup = blackoutPanel.GetComponent<CanvasGroup>();
            if (blackoutCanvasGroup == null)
            {
                blackoutCanvasGroup = blackoutPanel.AddComponent<CanvasGroup>();
            }
            blackoutCanvasGroup.alpha = 0;
        }

        // Start losowego uruchamiania efektów
        StartCoroutine(ActivateRandomEffects());
    }

    private IEnumerator ActivateRandomEffects()
    {
        while (true)
        {
            // Poczekaj losow¹ iloœæ czasu przed w³¹czeniem efektu
            float interval = Random.Range(minInterval, maxInterval);
            yield return new WaitForSeconds(interval);

            // Wybierz losowy efekt do aktywacji
            int randomEffect = Random.Range(0, 2); // 0 to trzêsienie ekranu, 1 to czarny ekran

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
            // Przemieszczanie kamery losowo w celu wywo³ania trzêsienia
            Vector3 randomOffset = Random.insideUnitSphere * shakeIntensity;
            mainCamera.transform.position = originalCameraPosition + randomOffset;

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Przywróæ oryginaln¹ pozycjê kamery
        mainCamera.transform.position = originalCameraPosition;
    }

    private IEnumerator Blackout()
    {
        if (blackoutPanel != null)
        {
            yield return StartCoroutine(FadeCanvasGroup(blackoutCanvasGroup, 0, 1, blackoutDuration / 2));
            yield return new WaitForSeconds(blackoutDuration);
            // Wy³¹cz efekt przyciemnienia
            yield return StartCoroutine(FadeCanvasGroup(blackoutCanvasGroup, 1, 0, blackoutDuration / 2));

        }
    }

    private IEnumerator FadeCanvasGroup(CanvasGroup canvasGroup, float startAlpha, float endAlpha, float duration)
    {
        float time = 0;
        while (time < duration)
        {
            canvasGroup.alpha = Mathf.Lerp(startAlpha, endAlpha, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        canvasGroup.alpha = endAlpha;
    }
}

