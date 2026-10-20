using UnityEngine;

public class Lab03LifecycleController : MonoBehaviour
{
    public Lab03LifecycleLogger cubeLogger;
    public GameObject sphere;
    public GameObject capsule;

    static readonly float[] Times = { 2f, 3f, 4f, 6f, 8f, 10f };
    int step;

    void Update()
    {
        if (step >= Times.Length || Time.time < Times[step]) return;

        switch (step)
        {
            case 0: Mark("Cube script: enabled = false"); cubeLogger.enabled = false; break;
            case 1: Mark("Capsule: SetActive(true)"); capsule.SetActive(true); break;
            case 2: Mark("Cube script: enabled = true"); cubeLogger.enabled = true; break;
            case 3: Mark("Sphere: SetActive(false)"); sphere.SetActive(false); break;
            case 4: Mark("Sphere: SetActive(true)"); sphere.SetActive(true); break;
            case 5: Mark("Capsule: Destroy"); Destroy(capsule); break;
        }

        step++;
    }

    void Mark(string action)
    {
        Debug.Log($"[t={Time.time:F2} frame={Time.frameCount}] ===== MARK: {action}");
    }
}