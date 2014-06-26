using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpawnKeyScript : MonoBehaviour {

	// Update is called once per frame
    public void generateKey()
    {
        GameObject[] allCylinder = GameObject.FindGameObjectsWithTag("cylinder");
        int rand = 0;
        for (int i = 0; i < 3; i++)
        {
            rand = Random.Range(0, allCylinder.Length - 1);
            while (allCylinder[rand].transform.renderer.material.color != Color.black)
                rand = Random.Range(0, allCylinder.Length - 1);
            allCylinder[rand].transform.renderer.material.color = Color.yellow;
            allCylinder[rand].transform.parent.tag = "cylinderWithKey";
            allCylinder[rand].networkView.RPC("setColor", RPCMode.Others, allCylinder[rand].networkView.viewID);
        }
        rand = Random.Range(0, allCylinder.Length - 1);
        while (allCylinder[rand].transform.renderer.material.color != Color.black)
            rand = Random.Range(0, allCylinder.Length - 1);
        allCylinder[rand].transform.renderer.material.color = Color.green;
        allCylinder[rand].transform.parent.tag = "exit";
        GameObject exit = GameObject.Find("Exit");
        exit.transform.parent = allCylinder[rand].transform;
        exit.transform.localPosition = new Vector3(-1.5f, 1.5f, 1.5f);
        allCylinder[rand].networkView.RPC("setExit", RPCMode.Others, allCylinder[rand].networkView.viewID);

    }

    [RPC]
    public void setColor(NetworkViewID id)
    {
        NetworkView view = NetworkView.Find(id);
        view.observed.renderer.material.color = Color.yellow;
        view.observed.transform.parent.tag = "cylinderWithKey";
    }

    [RPC]
    public void setExit(NetworkViewID id)
    {
        NetworkView view = NetworkView.Find(id);
        GameObject exit = GameObject.Find("Exit");
        view.observed.renderer.material.color = Color.green;
        view.observed.transform.parent.tag = "exit";
        exit.transform.parent = view.observed.transform;
        exit.transform.localPosition = new Vector3(-1.5f, 1.5f, 1.5f);
    }

    [RPC]
    public void respawnKey()
    {
        Debug.Log("respawn key");
        GameObject[] allCylinder = GameObject.FindGameObjectsWithTag("cylinder");
        int rand = Random.Range(0, allCylinder.Length - 1);
        while (allCylinder[rand].transform.renderer.material.color != Color.black)
            rand = Random.Range(0, allCylinder.Length - 1);
        allCylinder[rand].transform.renderer.material.color = Color.yellow;
        allCylinder[rand].transform.parent.tag = "cylinderWithKey";
        allCylinder[rand].networkView.RPC("setColor", RPCMode.Others, allCylinder[rand].networkView.viewID);
    }

    [RPC]
    public void keyDropped(NetworkViewID id)
    {
        NetworkView view = NetworkView.Find(id);
        view.observed.renderer.material.color = Color.black;
        view.observed.transform.parent.tag = "Untagged";
    }


}
