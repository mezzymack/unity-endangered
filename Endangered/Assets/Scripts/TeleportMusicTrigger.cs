using UnityEngine;

public class TeleportMusicTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            FindObjectOfType<musicManager>().SwitchToMazeMusic();
            // Then handle teleport logic here if needed
        }
    }
}