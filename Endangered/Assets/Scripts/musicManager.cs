using UnityEngine;
using System.Collections;

public class musicManager : MonoBehaviour
{
    public AudioSource outerMusic;
    public AudioSource mazeMusic;
    public float fadeDuration = 2f;

    void Start()
    {
        outerMusic.volume = 1f;
        mazeMusic.volume = 0f;

        outerMusic.Play();
        mazeMusic.Play();
    }

    public void SwitchToMazeMusic()
    {
        StopAllCoroutines();
        StartCoroutine(Crossfade(outerMusic, mazeMusic));
    }

    public void SwitchToOuterMusic()
    {
        StopAllCoroutines();
        StartCoroutine(Crossfade(mazeMusic, outerMusic));
    }

    private IEnumerator Crossfade(AudioSource from, AudioSource to)
    {
        float time = 0f;
        float startFromVol = from.volume;
        float startToVol = to.volume;

        while (time < fadeDuration)
        {
            time += Time.deltaTime;
            float t = time / fadeDuration;

            from.volume = Mathf.Lerp(startFromVol, 0f, t);
            to.volume = Mathf.Lerp(startToVol, 1f, t);

            yield return null;
        }

        from.volume = 0f;
        to.volume = 1f;
    }
}