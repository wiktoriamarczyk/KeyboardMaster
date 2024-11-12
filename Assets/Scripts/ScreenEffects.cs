using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ScreenEffects : MonoBehaviour
{
    [SerializeField] private float minInterval = 10f; // minimalny czas miêdzy efektami
    [SerializeField] private float maxInterval = 20f; // maksymalny czas miêdzy efektami
    [SerializeField] private float shakeDuration = 5f; // czas trwania efektu trzêsienia
    [SerializeField] private float shakeIntensity = 0.2f; // intensywnoœæ trzêsienia
    [SerializeField] public float fadeDuration = 1f; // Czas trwania fade in i fade out
    [SerializeField] public float waitTime = 2f;

    [SerializeField] public Image blackScreenImage; // panel UI do efektu czarnego ekranu

    private Camera mainCamera;
    private Vector3 originalCameraPosition;

    enum ScreenEffect
    {
        Shake = 0,
        Blackout,
        Count
    }

    void Start()
    {
        mainCamera = Camera.main;
        originalCameraPosition = mainCamera.transform.position;
        if (blackScreenImage != null)
        {
            blackScreenImage.color = new Color(0, 0, 0, 0);
        }

        StartCoroutine(ActivateRandomEffects());
    }

    void OnDestroy()
    {
        StopAllCoroutines();
    }

    private IEnumerator ActivateRandomEffects()
    {
        while (true)
        {
            float interval = Random.Range(minInterval, maxInterval);
            yield return new WaitForSeconds(interval);

            int randomEffect = Random.Range((int)ScreenEffect.Shake, (int)ScreenEffect.Count);

            switch (randomEffect)
            {
                case (int)ScreenEffect.Shake:
                    StartCoroutine(ScreenShake());
                    Debug.Log("Screen Shake");
                    break;
                case (int)ScreenEffect.Blackout:
                    StartCoroutine(Blackout());
                    Debug.Log("Blackout");
                    break;
            }
        }
    }

    private IEnumerator ScreenShake()
    {
        float elapsedTime = 0f;

        while (elapsedTime < shakeDuration)
        {
            Vector3 randomOffset = Random.insideUnitSphere * shakeIntensity;
            mainCamera.transform.position = originalCameraPosition + randomOffset;

            elapsedTime += Time.deltaTime;
            yield return null;
        }

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

