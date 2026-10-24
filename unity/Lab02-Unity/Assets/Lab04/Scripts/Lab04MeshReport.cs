using UnityEngine;

public class Lab04MeshReport : MonoBehaviour
{
    public GameObject[] targets;

    void Start()
    {
        foreach (GameObject target in targets)
        {
            foreach (MeshFilter filter in target.GetComponentsInChildren<MeshFilter>())
            {
                Mesh mesh = filter.sharedMesh;
                Debug.Log($"[Lab04] {target.name}/{mesh.name}: " +
                          $"vertices {mesh.vertexCount}, triangles {mesh.triangles.Length / 3}, " +
                          $"uv {mesh.uv.Length > 0}, normals {mesh.normals.Length}, size {mesh.bounds.size}");
            }
        }
    }
}
