using UnityEngine;
using System.Collections;

public class objectClass {
    private GameObject prefab;

    private float timeSpawn;
    private float buffValue;
    private float buffTime;

    public GameObject Prefab
    {
        get { return this.prefab; }
    }

    public float TimeSpawn
    {
        get { return this.timeSpawn; }
    }

    public float BuffValue
    {
        get { return this.buffValue; }
    }

    public float BuffTime
    {
        get { return this.buffTime; }
    }

    public objectClass(GameObject prefab, float timeSpawn,float buffValue,float buffTime)
    {
        this.prefab = prefab;
        this.timeSpawn = timeSpawn;
        this.buffValue = buffValue;
        this.buffTime = buffTime;
    }
}
