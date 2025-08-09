using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class fireAudio : MonoBehaviour
{
    private AudioSource fireAudioSource;
    private Coroutine fadeCoroutine;

    [SerializeField] private float fadeDuration = 1f; // duration of fade in seconds
    [SerializeField] private float maxVolume = 1f;    // max volume for the fire sound

    private void Awake()
    {
        fireAudioSource = GetComponent<AudioSource>();
        fireAudioSource.loop = true;
        fireAudioSource.playOnAwake = false;
        fireAudioSource.volume = 0f;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (fadeCoroutine != null) StopCoroutine(fadeCoroutine);
            fadeCoroutine = StartCoroutine(FadeAudio(true));
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (fadeCoroutine != null) StopCoroutine(fadeCoroutine);
            fadeCoroutine = StartCoroutine(FadeAudio(false));
        }
    }

    private IEnumerator FadeAudio(bool fadeIn)
    {
        float startVolume = fireAudioSource.volume;
        float targetVolume = fadeIn ? maxVolume : 0f;
        float elapsed = 0f;

        if (fadeIn && !fireAudioSource.isPlaying)
        {
            fireAudioSource.Play();
        }

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            fireAudioSource.volume = Mathf.Lerp(startVolume, targetVolume, elapsed / fadeDuration);
            yield return null;
        }

        fireAudioSource.volume = targetVolume;

        if (!fadeIn)
        {
            fireAudioSource.Stop();
        }
    }
}
