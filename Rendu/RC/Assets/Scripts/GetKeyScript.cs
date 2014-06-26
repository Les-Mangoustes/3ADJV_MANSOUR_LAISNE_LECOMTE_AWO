using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GetKeyScript : MonoBehaviour {

    [SerializeField]
    GameObject keytexture;

    List<GameObject> keyHandle;

    int current_index = 0;

    void Start()
    {
        keyHandle = new List<GameObject>();
    }

    public void getKey()
    {
        Debug.Log("get key");
        if (keyHandle.Count < 3)
        {
            keyHandle.Add((GameObject)Instantiate(keytexture, getImagePosition(), Quaternion.identity));
            current_index++;
        }
    }

    public void dropKey()
    {
        //Debug.Log("drop key");
        networkView.RPC("dropKeyEnnemy", RPCMode.Others, null);
    }

    [RPC]
    public void dropKeyEnnemy()
    {
        if (keyHandle.Count > 0)
        {
            current_index--;
            Destroy(keyHandle[current_index]);
            keyHandle.Remove(keyHandle[current_index]);
        }
    }

    public Vector3 getImagePosition()
    {
        Vector3 pos = new Vector3();
        pos.x = 0.75f - (current_index * 0.2f);
        pos.y = 0.85f;
        pos.z = 0;
        return pos;
    }

    public bool haveThreeKey()
    {
        if (keyHandle.Count >= 3)
        {
            return true;
        }
        return false;
    }

    public bool haveKey()
    {
        if (keyHandle.Count < 1)
        {
            return false;
        }
        return true;
    }
}
