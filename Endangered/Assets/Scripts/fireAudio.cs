using UnityEngine;
using System.Collections;

public class fireAudio : MonoBehaviour
{
    private AudioSource fireAudioSource;
    private Coroutine fadeCoroutine;

    [SerializeField] private float fadeDuration = 1f;
    [SerializeField] private float maxVolume = 1f;

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
            if (!gameObject.activeInHierarchy)
            {
                Debug.LogWarning("GameObject or parent is inactive; skipping coroutine.");
                return;  // Don't start coroutine if object is inactive
            }

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
