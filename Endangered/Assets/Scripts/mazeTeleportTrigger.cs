using UnityEngine;

public class mazeTeleportTrigger : MonoBehaviour
{
    public audioManager audioManager;
    public Transform mazeTeleportLocation;
    public GameObject player;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == player)
        {
            // Teleport player
            player.transform.position = mazeTeleportLocation.position;

            // Switch music
            audioManager.PlayMazeTheme();
        }
    }
}
