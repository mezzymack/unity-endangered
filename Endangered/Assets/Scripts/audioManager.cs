using UnityEngine;

public class audioManager : MonoBehaviour
{
    public AudioSource overworldTheme;
    public AudioSource mazeTheme;

    void Start()
    {
        PlayOverworldTheme();
    }

    public void PlayOverworldTheme()
    {
        mazeTheme.Stop();
        if (!overworldTheme.isPlaying)
            overworldTheme.Play();
    }

    public void PlayMazeTheme()
    {
        overworldTheme.Stop();
        if (!mazeTheme.isPlaying)
            mazeTheme.Play();
    }
}
