using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Lab04DataManager : MonoBehaviour
{
    public string fileName = "Lab04/Lab04_Entities.json";

    readonly List<Lab04EntityData> all = new List<Lab04EntityData>();
    readonly Dictionary<string, Lab04EntityData> byId =
        new Dictionary<string, Lab04EntityData>();

    void Awake()
    {
        Load(fileName);
    }

    public bool Load(string file)
    {
        all.Clear();
        byId.Clear();

        string path = Path.Combine(Application.streamingAssetsPath, file);

        try
        {
            string json = File.ReadAllText(path);
            Lab04EntityList list =
                JsonUtility.FromJson<Lab04EntityList>(json);

            if (list == null || list.entities == null)
            {
                Debug.LogError(
                    $"[Lab04] DataManager: no 'entities' array in {path}"
                );
                return false;
            }

            foreach (Lab04EntityData data in list.entities)
            {
                all.Add(data);
                byId[data.id] = data;

                Debug.Log(
                    $"[Lab04] DataManager: {data.id} prefab {data.prefab} points {data.points}"
                );
            }

            Debug.Log(
                $"[Lab04] DataManager loaded {all.Count} entities from {path}"
            );

            return true;
        }
        catch (IOException e)
        {
            Debug.LogError(
                $"[Lab04] DataManager cannot read {path}: {e.Message}"
            );
        }
        catch (ArgumentException e)
        {
            Debug.LogError(
                $"[Lab04] DataManager cannot parse {path}: {e.Message}"
            );
        }

        return false;
    }

    public bool TryGetEntity(string id, out Lab04EntityData data)
    {
        return byId.TryGetValue(id, out data);
    }

    public IReadOnlyList<Lab04EntityData> GetAll()
    {
        return all;
    }
}