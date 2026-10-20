using UnityEngine;

public class Lab03ChallengeMover : MonoBehaviour
{
    public float speed = 3f;

    Vector3 direction;

    void Start()
    {
        direction = new Vector3(
            Random.Range(-1f, 1f),
            0f,
            Random.Range(-1f, 1f)
        ).normalized;
    }

    void Update()
    {
        transform.position += direction * speed * Time.deltaTime;
    }
}