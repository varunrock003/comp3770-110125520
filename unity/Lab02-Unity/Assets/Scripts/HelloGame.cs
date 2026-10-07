using UnityEngine;

public class HelloGame : MonoBehaviour
{
    private int frameCount = 0;

    void Start()
    {
        Debug.Log("Start: the game loop has begun.");
    }

    void Update()
    {
        frameCount++;
        if (frameCount == 1)
            Debug.Log("Update: running on frame " + frameCount + ".");
    }
}
