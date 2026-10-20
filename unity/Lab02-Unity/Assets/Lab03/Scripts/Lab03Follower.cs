using UnityEngine;

public class Lab03Follower : MonoBehaviour
{
    public enum Phase { Update, LateUpdate }

    public Lab03Mover leader;
    public Phase followsIn = Phase.Update;
    public Vector3 offset = new Vector3(0f, 0f, -1f);

    int fresh, stale;

    void Update()
    {
        if (followsIn == Phase.Update)
            Follow();
    }

    void LateUpdate()
    {
        if (followsIn == Phase.LateUpdate)
            Follow();
    }

    void Follow()
    {
        if (leader.lastMovedFrame == Time.frameCount)
            fresh++;
        else
            stale++;

        transform.position = leader.transform.position + offset;
    }

    void OnGUI()
    {
        GUI.Label(new Rect(10, 40, 600, 20),
            $"Follower ({followsIn}): fresh {fresh} / stale {stale}");
    }
}