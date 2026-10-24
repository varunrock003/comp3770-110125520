using UnityEngine;

public class Lab04ChallengeSwitcher : MonoBehaviour
{
    public Lab04DataManager dataManager;
    public Lab04EntityManager entityManager;

    public string fileA = "Lab04/Lab04_Entities.json";
    public string fileB = "Lab04/Lab04_Entities_2.json";

    public float switchEverySeconds = 5f;

    bool usingA = true;
    float nextSwitch;

    void Start()
    {
        nextSwitch = Time.time + switchEverySeconds;
    }

    void Update()
    {
        if (Time.time < nextSwitch)
            return;

        usingA = !usingA;

        string nextFile = usingA ? fileA : fileB;

        Debug.Log(
            $"[t={Time.time:F2} frame={Time.frameCount}] ===== MARK: switch to {nextFile}"
        );

        entityManager.Clear();

        if (dataManager.Load(nextFile))
        {
            entityManager.SpawnAll();

            Debug.Log(
                $"[Lab04] DataSwitcher loaded {nextFile}"
            );
        }

        nextSwitch = Time.time + switchEverySeconds;
    }
}