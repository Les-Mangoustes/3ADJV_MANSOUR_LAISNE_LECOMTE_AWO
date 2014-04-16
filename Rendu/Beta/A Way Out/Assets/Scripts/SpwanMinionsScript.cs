using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpwanMinionsScript : MonoBehaviour {

    private int _rangeX;
    private int _rangeZ;

    private Dictionary<string, minionsClass> minionsList;

    public minionsClass getMinions(string name)
    {
        return minionsList[name];
    }

    [SerializeField]
    private GameObject blueMinions;

    [SerializeField]
    private GameObject redMinions;

    [SerializeField]
    private GameObject purpleMinions;

	// Use this for initialization
	void Start () {
        _rangeX = Mathf.FloorToInt(GameObject.Find("Plane").renderer.bounds.size.x) / 2;
        _rangeZ = Mathf.FloorToInt(GameObject.Find("Plane").renderer.bounds.size.z) / 2;
        minionsList = new Dictionary<string, minionsClass>();
        minionsList.Add("BlueMinions", new minionsClass("BlueMinions",blueMinions, 2.0f, 10.0f, 15));
        minionsList.Add("RedMinions", new minionsClass("RedMinions", redMinions,5.0f, 5.0f, 30));
        minionsList.Add("PurpleMinions", new minionsClass("PurpleMinions", purpleMinions,7.5f, 2.0f, 45));
        if (Network.isServer)
            startSpawn();
	}

    private void startSpawn()
    {
        Vector3 position_minions;
        foreach(string key in minionsList.Keys)
        {
            position_minions = new Vector3(Random.Range(-_rangeX,_rangeX),0,Random.Range(-_rangeZ,_rangeZ));
            GameObject minions = (GameObject)Network.Instantiate(minionsList[key].prefab, position_minions, Quaternion.identity, 70);
            minions.name = minionsList[key].name;
            minions.GetComponent<IAMinionsScript>().speed = 5.0f + minionsList[key].baseSpeed;
        }
    }

    public void destroyMinions(string name,GameObject player)
    {
        GameObject minions = GameObject.Find(name);
        if (minions != null)
        {
            Destroy(minions);
            player.GetComponent<PlayerCaracScript>().setTemporaryAlteration(minionsList[name].buffValue, minionsList[name].buffTime, false);
            object[] obj = { name, minionsList[name].spawnTime };
            StartCoroutine("respawnMinions", obj);
        }

    }

    private IEnumerator respawnMinions(object[] obj)
    {
        yield return new WaitForSeconds((float)obj[1]);
        Vector3 position_minions = new Vector3(Random.Range(-_rangeX, _rangeX), 0, Random.Range(-_rangeZ, _rangeZ));
        Network.Instantiate(minionsList[obj[0].ToString()].prefab, position_minions, Quaternion.identity, 70).name = minionsList[obj[0].ToString()].name;
    }


}
