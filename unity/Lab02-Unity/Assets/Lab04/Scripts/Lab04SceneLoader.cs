using UnityEngine;
using UnityEngine.SceneManagement;

public class Lab04SceneLoader : MonoBehaviour
{
    public string sceneName = "Lab04_Level";
    public float unloadAfterSeconds = 8f;
    public Lab04EntityManager entityManager;

    AsyncOperation loading;
    AsyncOperation unloading;
    float loadedAt;
    bool loadDone;
    bool unloadLogged;

    void Start()
    {
        Debug.Log($"[t={Time.time:F2} frame={Time.frameCount}] ===== MARK: load {sceneName}");
        loading = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
    }

    void Update()
    {
        if (!loadDone && loading.isDone)
        {
            loadDone = true;
            loadedAt = Time.time;

            Debug.Log(
                $"[t={Time.time:F2} frame={Time.frameCount}] ===== MARK: {sceneName} loaded"
            );

            entityManager.SpawnAll();
        }

        if (loadDone &&
            unloading == null &&
            Time.time - loadedAt >= unloadAfterSeconds)
        {
            Debug.Log(
                $"[t={Time.time:F2} frame={Time.frameCount}] ===== MARK: unload {sceneName}"
            );

            unloading = SceneManager.UnloadSceneAsync(sceneName);
        }

        if (unloading != null &&
            unloading.isDone &&
            !unloadLogged)
        {
            unloadLogged = true;

            Debug.Log(
                $"[t={Time.time:F2} frame={Time.frameCount}] ===== MARK: {sceneName} unloaded"
            );
        }
    }
}