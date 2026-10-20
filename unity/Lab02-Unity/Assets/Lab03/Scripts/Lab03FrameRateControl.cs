using UnityEngine;

public class Lab03FrameRateControl : MonoBehaviour
{
    public int targetFps = 60;

    int fixedCalls, fixedCallsBeforeUpdate;

    void Awake()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = targetFps;
    }

    void FixedUpdate() { fixedCalls++; }

    void Update()
    {
        fixedCallsBeforeUpdate = fixedCalls;
        fixedCalls = 0;
    }

    void OnGUI()
    {
        GUI.Label(new Rect(10, 10, 900, 20),
            $"t = {Time.time:F1} s | target {targetFps} FPS | measured {1f / Time.unscaledDeltaTime:F0} FPS | " +
            $"fixedDeltaTime {Time.fixedDeltaTime:F3} s | FixedUpdate calls before this Update: {fixedCallsBeforeUpdate}");
    }
}