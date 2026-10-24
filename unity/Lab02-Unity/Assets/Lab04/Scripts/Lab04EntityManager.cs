using System.Collections.Generic;
using UnityEngine;

public class Lab04EntityManager : MonoBehaviour
{
    public Lab04DataManager dataManager;
    public Transform root;

    readonly Dictionary<string, GameObject> live =
        new Dictionary<string, GameObject>();

    void Start()
    {
    }

    public void SpawnAll()
    {
        foreach (Lab04EntityData data in dataManager.GetAll())
        {
            Spawn(data.id);
        }
    }

    public GameObject Spawn(string id)
    {
        if (!dataManager.TryGetEntity(id, out Lab04EntityData data))
        {
            Debug.LogWarning($"[Lab04] EntityManager: unknown id {id}");
            return null;
        }

        GameObject prefab =
            Resources.Load<GameObject>("Prefabs/" + data.prefab);

        if (prefab == null)
        {
            Debug.LogError(
                $"[Lab04] EntityManager: prefab not found {data.prefab}"
            );
            return null;
        }

        Vector3 position = new Vector3(data.x, data.y, data.z);

        GameObject entity =
            Instantiate(prefab, position, Quaternion.identity, root);

        entity.name = id;

        Lab04Prop prop = entity.GetComponent<Lab04Prop>();

        if (prop != null)
        {
            prop.points = data.points;
            prop.spinSpeed = data.spinSpeed;
        }

        live[id] = entity;

        Debug.Log($"[Lab04] EntityManager spawned {id}");

        return entity;
    }

    public void Despawn(string id)
    {
        if (!live.TryGetValue(id, out GameObject entity))
        {
            Debug.LogWarning(
                $"[Lab04] EntityManager: cannot despawn unknown id {id}"
            );
            return;
        }

        Destroy(entity);
        live.Remove(id);

        Debug.Log($"[Lab04] EntityManager despawned {id}");
    }

    public GameObject Get(string id)
    {
        live.TryGetValue(id, out GameObject entity);
        return entity;
    }

    public void Clear()
    {
        foreach (GameObject entity in live.Values)
        {
            Destroy(entity);
        }

        live.Clear();
        Debug.Log("[Lab04] EntityManager cleared");
    }
}