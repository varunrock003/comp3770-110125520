using System;
using System.Collections.Generic;

[Serializable]
public class Lab04EntityData
{
    public string id;
    public string prefab;
    public float x;
    public float y;
    public float z;
    public int points;
    public float spinSpeed;
}

[Serializable]
public class Lab04EntityList
{
    public List<Lab04EntityData> entities;
}