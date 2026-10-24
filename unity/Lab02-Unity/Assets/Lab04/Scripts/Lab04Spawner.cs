using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class Lab04Spawner : MonoBehaviour
{
    public GameObject prefab;
    public int count = 3;
    public float spacing = 3f;
    public Transform parent;

    void Start()
    {
        Stopwatch watch = Stopwatch.StartNew();
        int columns = Mathf.CeilToInt(Mathf.Sqrt(count));

        for (int i = 0; i < count; i++)
        {
            Vector3 position = new Vector3(
                (i % columns) * spacing,
                0f,
                (i / columns) * spacing
            );

            GameObject copy =
                Instantiate(prefab, position, Quaternion.identity, parent);

            if (count <= 10)
                Debug.Log(
                    $"[t={Time.time:F2} frame={Time.frameCount}] after Instantiate: {copy.name}"
                );
        }

        watch.Stop();

        Debug.Log(
            $"[Lab04] {count} instantiations in {watch.Elapsed.TotalMilliseconds:F2} ms"
        );
    }
}