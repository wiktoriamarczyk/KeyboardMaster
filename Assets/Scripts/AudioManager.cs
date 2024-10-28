using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private List<AudioClip> musicClips;
    [SerializeField] private float fadeDuration = 1.5f; 
    private int currentClipIndex = 0;
    private bool isTransitioning = false;

    private void Start()
    {
        if (musicClips.Count > 0)
        {
            PlayClip(currentClipIndex);
        }
        else
        {
            Debug.LogWarning("Lista klipów muzycznych jest pusta.");
        }
    }

    private void Update()
    {
        if (!musicSource.isPlaying && musicSource.clip != null)
        {
            StartCoroutine(FadeOutAndPlayNext());
        }
    }

    private void PlayClip(int index)
    {
        if (index < musicClips.Count)
        {
            musicSource.clip = musicClips[index];
            musicSource.Play();
            StartCoroutine(FadeIn());
            Debug.Log($"Odtwarzanie klipu: {musicClips[index].name}");
        }
        else
        {
            Debug.LogWarning("Indeks klipu wykracza poza listê.");
        }
    }

    private IEnumerator FadeOutAndPlayNext()
    {
        isTransitioning = true;

        // Fade-out
        float startVolume = musicSource.volume;
        for (float t = 0; t < fadeDuration; t += Time.deltaTime)
        {
            musicSource.volume = Mathf.Lerp(startVolume, 0, t / fadeDuration);
            yield return null;
        }
        musicSource.volume = 0;
        musicSource.Stop();

        // Ustawienie na nastêpny klip
        currentClipIndex = (currentClipIndex + 1) % musicClips.Count;
        PlayClip(currentClipIndex);

        isTransitioning = false;
    }

    private IEnumerator FadeIn()
    {
        // Fade-in
        float targetVolume = 1.0f;
        musicSource.volume = 0;
        for (float t = 0; t < fadeDuration; t += Time.deltaTime)
        {
            musicSource.volume = Mathf.Lerp(0, targetVolume, t / fadeDuration);
            yield return null;
        }
        musicSource.volume = targetVolume;
    }
}
