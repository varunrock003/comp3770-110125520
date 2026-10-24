using UnityEngine;

public class Lab04ManagerDemo : MonoBehaviour
{
    public Lab04DataManager dataManager;
    public Lab04EntityManager entityManager;

    string firstId;
    bool despawned;
    bool respawned;

    void Update()
    {
        if (firstId == null)
        {
            if (dataManager.GetAll().Count == 0)
                return;

            firstId = dataManager.GetAll()[0].id;
        }

        if (!despawned && Time.time >= 3f)
        {
            Debug.Log(
                $"[t={Time.time:F2} frame={Time.frameCount}] ===== MARK: Despawn {firstId}"
            );

            entityManager.Despawn(firstId);
            despawned = true;
        }

        if (!respawned && Time.time >= 5f)
        {
            Debug.Log(
                $"[t={Time.time:F2} frame={Time.frameCount}] ===== MARK: Spawn {firstId}"
            );

            entityManager.Spawn(firstId);
            respawned = true;
        }
    }
}