using UnityEngine;

public class Lab04Prop : MonoBehaviour
{
    public string displayName = "Prop";
    public int points = 10;
    public float spinSpeed = 30f;

    void Update()
    {
        transform.Rotate(0f, spinSpeed * Time.deltaTime, 0f);
    }
}