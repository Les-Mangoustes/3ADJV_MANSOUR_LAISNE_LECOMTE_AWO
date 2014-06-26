using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerCaracScriptTutorial : MonoBehaviour {

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

    private float alterationValue;

    private float time;
    private bool _isOnCouldown;

    private Rect rec = new Rect((Screen.width / 3) * 2, (Screen.height / 3) * 2, Screen.width / 3, 20);

	// Use this for initialization
    public float getSpeedWithAlt()
    {
        return this.speed + this.alterationValue;
    }

    public void setPlayerRun(bool want)
    {
        _playerWantToRun = want;
    }

	void Start () {
        this.speed = baseSpeed;
        this.energy = max_energy;
        this.alteration = new List<float>();
        this.time = 0;
        this._isOnCouldown = false;
	}

    void Update()
    {
        if (time > 0)
            time -= Time.deltaTime;
        else
            _isOnCouldown = false;
        applyAlteration();
        if (!_isOnCouldown)
        {
            if ((Network.isServer && gameObject.name == "Player") || (Network.isClient && gameObject.name == "Player2") || gameObject.name == "SphereEnemy")
            {
                if ((Input.GetKey(KeyCode.LeftShift) && energy > 0.0f && canRun) || _playerWantToRun)
                {

                    networkView.RPC("playerWantToRun", RPCMode.Others, gameObject.name, true);
                    setTemporaryAlteration(1.5f, 3, false);
                    energy -= 10;
                    time = 1;
                    _isOnCouldown = true;
                }
                else
                {
                    canRun = false;
                    networkView.RPC("playerWantToRun", RPCMode.Others, gameObject.name, false);
                    this.speed = baseSpeed;
                    if (energy < max_energy)
                        energy += 0.5f;
                    else
                        canRun = true;
                }
            }
        }
    }

    void OnGUI()
    {
        if ((Network.isServer && gameObject.name == "Player") || (Network.isClient && gameObject.name == "Player2"))
            GUI.Label(rec, "Energy : " + (int)energy + "/" + max_energy);
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
        alterationValue = 0;
        if (alteration.Count > 0)
        {
            foreach (int value in alteration)
                alterationValue += value;
            //vitesse mini 0 pour ne pas reculer mais le calcul des altération se fait quand même
            if (speed + alterationValue < 0)
                alterationValue = -speed;
            //vitesse maxi 15
            if (speed + alterationValue > 15)
                alterationValue = 15 - speed;
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
