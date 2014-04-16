using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerCaracScript : MonoBehaviour {

    //Class a tester après l'implémentation des objets
    //bug les deux players cours

    [SerializeField]
    private float baseSpeed = 5.0f;

    public float BaseSpeed
    {
        get { return baseSpeed; }
        set { this.baseSpeed = value; }
    }

    public float speed;

    private float max_energy = 100.0f;
    private float energy;

    private List<float> alteration;
    private bool canRun = true;
    private bool _playerWantToRun = false;

    private Rect rec = new Rect((Screen.width / 3) * 2, (Screen.height / 3) * 2, Screen.width / 3, 20);

	// Use this for initialization
    public float getSpeed()
    {
        return this.speed;
    }

    public void setPlayerRun(bool want)
    {
        _playerWantToRun = want;
    }

	void Start () {
        this.speed = baseSpeed;
        this.energy = max_energy;
        this.alteration = new List<float>();
	}

    void Update()
    {
        if ((Network.isServer && gameObject.name == "Player") || (Network.isClient && gameObject.name == "Player2") || gameObject.name == "SphereEnemy")
        {
            if ((Input.GetKey(KeyCode.LeftShift) && energy > 0.0f && canRun) || _playerWantToRun)
            {
                networkView.RPC("playerWantToRun", RPCMode.Others, gameObject.name, true);
                this.speed = baseSpeed + 3.0f;
                energy -= 1;
            }
            else
            {
                canRun = false;
                networkView.RPC("playerWantToRun", RPCMode.Others, gameObject.name, false);
                this.speed = baseSpeed;
                if (energy < max_energy)
                    energy += 1;
                else
                    canRun = true;
            }
            applyAlteration();
            //for debug
            /*if (Input.GetKeyDown(KeyCode.B))
            {
                setTemporaryAlteration(10.0f, 3, false);
            }

            if(Input.GetKeyDown(KeyCode.N))
            {
                setTemporaryAlteration(-10.0f, 3, false);
            }*/
        }

    }

    void OnGUI()
    {
        if ((Network.isServer && gameObject.name == "Player") || (Network.isClient && gameObject.name == "Player2"))
            GUI.Label(rec, "Energy : " + energy + "/" + max_energy);
    }

    public void setTemporaryAlteration(float valueAlt,float time,bool fromRPC)
    {
        alteration.Add(valueAlt);
        object[] obj = { valueAlt, time }; 
        StartCoroutine("removeAlteration", obj);
        if (!fromRPC)
            networkView.RPC("addAlterationOnPlayer", RPCMode.Others, gameObject.name, valueAlt, time);
    }

    private void applyAlteration()
    {
        if (alteration.Count > 0)
        {
            foreach (int value in alteration)
            {
                Debug.Log(gameObject.name + ":" + value);
                speed += value;
            }
            if (speed < 0)
                speed = 0;
            if (speed > 15)
                speed = 15;
        }
    }

    private IEnumerator removeAlteration(object[] obj)
    {
        yield return new WaitForSeconds((float)obj[1]);
        alteration.Remove((float)obj[0]);
    }

    [RPC]
    private void playerWantToRun(string name,bool want)
    {
        GameObject.Find(name).GetComponent<PlayerCaracScript>().setPlayerRun(want);
    }

    [RPC]
    private void addAlterationOnPlayer(string name, float valueAlt,float time)
    {
        if (name == gameObject.name)
        {
            setTemporaryAlteration(valueAlt, time, true);
        }
    }
}
