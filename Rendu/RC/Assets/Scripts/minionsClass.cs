using UnityEngine;
using System.Collections;

public class minionsClass {
    public string name;

    public GameObject prefab;

    public float buffValue;
    public float buffTime;
    public float baseSpeed = 2.5f;

    public float spawnTime;

    public minionsClass(string name, GameObject Prefab, float buffValue, float buffTime,float spawnTime)
    {
        this.name = name;
        this.prefab = Prefab;
        this.buffValue = buffValue;
        this.buffTime = buffTime;
        this.spawnTime = spawnTime;
    }
}
