using UnityEngine;

public class Lab03LifecycleLogger : MonoBehaviour
{
    [Header("Logging")]
    [SerializeField] string id = "Object";
    [Tooltip("Seconds between two lines of a repeating callback")]
    [Range(0.1f, 5f)] public float logInterval = 1f;
    [SerializeField] int logCount;

    float nextFixedLog, nextUpdateLog, nextLateLog;

    void Awake() { Log("Awake"); }

    void OnEnable()
    {
        Log("OnEnable");
        nextFixedLog = nextUpdateLog = nextLateLog = 0f;
    }

    void Start() { Log("Start"); }

    void FixedUpdate()
    {
        if (Due(ref nextFixedLog)) Log("FixedUpdate");
    }

    void Update()
    {
        if (Due(ref nextUpdateLog))
            Log($"Update (world {transform.position}, local {transform.localPosition})");
    }

    void LateUpdate()
    {
        if (Due(ref nextLateLog)) Log("LateUpdate");
    }

    void OnDisable() { Log("OnDisable"); }

    void OnDestroy() { Log("OnDestroy"); }

    bool Due(ref float next)
    {
        if (Time.time < next) return false;
        next = Time.time + logInterval;
        logCount++;
        return true;
    }

    void Log(string callback)
    {
        Debug.Log($"[t={Time.time:F2} frame={Time.frameCount}] {id}.{callback}");
    }
} 
