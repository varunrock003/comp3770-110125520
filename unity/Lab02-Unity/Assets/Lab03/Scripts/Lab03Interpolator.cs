using UnityEngine;

public class Lab03Interpolator : MonoBehaviour
{
    public enum Mode { Linear, Smooth }

    public Mode mode = Mode.Linear;
    public float distance = 12f;
    public float duration = 4f;
    public float smoothing = 3f;
    public bool useDeltaTime = true;

    Vector3 start, end, target;
    Quaternion startRotation, endRotation;
    float elapsed;
    bool towardsEnd = true;

    void Start()
    {
        start = transform.position;
        end = start + Vector3.right * distance;
        target = end;
        startRotation = transform.rotation;
        endRotation = Quaternion.Euler(0f, 90f, 0f) * startRotation;
    }

    void Update()
    {
        if (mode == Mode.Linear)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);
            transform.position = Vector3.Lerp(start, end, t);
            transform.rotation = Quaternion.Slerp(startRotation, endRotation, t);

            if (t >= 1f)
            {
                (start, end) = (end, start);
                (startRotation, endRotation) = (endRotation, startRotation);
                elapsed = 0f;
            }
        }
        else
        {
            float k = useDeltaTime ? smoothing * Time.deltaTime : 0.1f;
            transform.position = Vector3.Lerp(transform.position, target, k);

            if ((target - transform.position).sqrMagnitude < 0.0025f)
            {
                towardsEnd = !towardsEnd;
                target = towardsEnd ? end : start;
            }
        }
    }
}