using UnityEngine;

public class randomPosition : MonoBehaviour
{
    public Vector2[] positions;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        int randomNumber=Random.Range(0, positions.Length);
        transform.localPosition = positions[randomNumber];
    }

}
