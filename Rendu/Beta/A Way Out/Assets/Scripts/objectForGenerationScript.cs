using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class objectForGenerationScript {

    public Transform visual;

    private int sizeX;
    private int sizeZ;

    [SerializeField]
    private float probabiliy;

    public float Probability
    {
        get
        { return probabiliy; }
        set
        {
            if (value <= 0)
                probabiliy = 0;
            else if (value >= 1)
                probabiliy = 1;
            else
                probabiliy = value;
        }
    }

    [SerializeField]
    public List<sortiecoord> exits;

    [System.Serializable]
    public class sortiecoord
    {
        public int x;
        public int z;
    }

    public int getSizeX()
    {
        if (this.sizeX == null)
            sizeX = (int)(this.visual.renderer.bounds.size.x);
        return sizeX;
    }

    public int getSizeZ()
    {
        if (this.sizeZ == null)
            sizeZ = (int)(this.visual.renderer.bounds.size.z);
        return sizeZ;
    }

    public int surface()
    {
        return this.sizeX * this.sizeZ;
    }
}
