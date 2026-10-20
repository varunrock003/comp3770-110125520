using UnityEngine;

public class Lab03Mover : MonoBehaviour
{
    public enum Phase { Update, FixedUpdate, FixedUpdateInterpolated }

    public Phase movesIn = Phase.Update;
    public bool useDeltaTime = true;
    public float speed = 4f;
    public float limit = 6f;
    public float spinDegreesPerSecond = 0f;

    [HideInInspector] public int lastMovedFrame = -1;

    float direction = 1f;
    float traveled;
    Vector3 previous, current;

    void Start()
    {
        previous = current = transform.position;
    }

    void FixedUpdate()
    {
        if (movesIn == Phase.FixedUpdate)
        {
            transform.position = Advance(transform.position, Time.fixedDeltaTime);
        }
        else if (movesIn == Phase.FixedUpdateInterpolated)
        {
            previous = current;
            current = Advance(current, Time.fixedDeltaTime);
        }
    }

    void Update()
    {
        if (movesIn == Phase.Update)
        {
            transform.position = Advance(transform.position, Time.deltaTime);
            lastMovedFrame = Time.frameCount;
        }
        else if (movesIn == Phase.FixedUpdateInterpolated)
        {
            float alpha = Mathf.Clamp01((Time.time - Time.fixedTime) / Time.fixedDeltaTime);
            transform.position = Vector3.Lerp(previous, current, alpha);
        }

        if (spinDegreesPerSecond != 0f)
            transform.Rotate(0f, spinDegreesPerSecond * Time.deltaTime, 0f);
    }

    Vector3 Advance(Vector3 position, float dt)
    {
        float amount = useDeltaTime ? speed * dt : speed * 0.02f;
        position.x += direction * amount;
        traveled += amount;

        if (Mathf.Abs(position.x) > limit)
        {
            direction = -direction;
            position.x = Mathf.Clamp(position.x, -limit, limit);
        }
        return position;
    }

    void OnGUI()
    {
        Vector3 screen = Camera.main.WorldToScreenPoint(transform.position);
        GUI.Label(new Rect(screen.x + 20f, Screen.height - screen.y - 10f, 320f, 20f),
                  $"{name}: traveled {traveled:F1} u");
    }
}