using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PowerProperties : MonoBehaviour {

	private int idPower;

    [SerializeField]
    Transform _slender;

	// Use this for initialization
    Transform oldTarget;
    Transform newTarget;
	void Start () {
        if (idPower == 2)
        {
            IAScript ias = GameObject.Find(_slender.name + "(Clone)").GetComponent<IAScript>();
            if(ias.getTarget())
                oldTarget = ias.getTarget();
            if(Network.isServer)
                ias.setTarget(gameObject.transform);
            else
                networkView.RPC("serverChangeTarget", RPCMode.Others, gameObject.networkView.viewID);

        }
	}
	void OnTriggerEnter(Collider col){
			
		switch (idPower) {
			
		case 1 : {
			
			//if (col.gameObject.tag == "Player" || col.gameObject.tag== "Enemy")
			//{
                PlayerCaracScript pcs = col.gameObject.GetComponent<PlayerCaracScript>();
                pcs.setTemporaryAlteration((-20), 3.0f, false);
                Network.Destroy(gameObject);	
			//}
		}
		break;
		case 2 : {
            if (col.gameObject.tag == "Enemy")
            {
                IAScript ias = GameObject.Find(_slender.name + "(Clone)").GetComponent<IAScript>();
                PlayerCaracScript pcs = col.gameObject.GetComponent<PlayerCaracScript>();
                pcs.setTemporaryAlteration((-20), 3.0f, false);
                if (oldTarget)
                    if (Network.isServer)
                        ias.setTarget(oldTarget);
                    else
                        networkView.RPC("serverChangeTarget", RPCMode.Others, oldTarget.networkView.viewID);
                else
                    ias._isOnAttack = false;
                Network.Destroy(gameObject);
            }
        
        
        }
			break;
		case 3 : { }
			break;
		default :;
			break;
			
		}
	}

	// Update is called once per frame
	void Update () {
	


	}

	public void setIdPower(int id){
		this.idPower=id;
        if (id == 1)
        {
            gameObject.renderer.material.color = Color.blue;
        }
        if (id == 2)
        {
            gameObject.renderer.material.color = Color.red;
        }
	}

    [RPC]
    private void serverChangeTarget(NetworkViewID id)
    {
        IAScript ias = GameObject.Find(_slender.name + "(Clone)").GetComponent<IAScript>();
        ias.setTarget(NetworkView.Find(id).transform);
    }



}
