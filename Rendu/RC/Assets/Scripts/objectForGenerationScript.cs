using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class objectForGenerationScript {

    public Transform visual;

    public int sizeX = 0;

    public int sizeZ = 0;

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

    public int getSizeX()
    {
        if (this.sizeX == 0)
            sizeX = (int)(this.visual.renderer.bounds.size.x);
        return sizeX;
    }

    public int getSizeZ()
    {
        if (this.sizeZ == 0)
            sizeZ = (int)(this.visual.renderer.bounds.size.z);
        return sizeZ;
    }

    public int surface()
    {
        return this.sizeX * this.sizeZ;
    }
}
