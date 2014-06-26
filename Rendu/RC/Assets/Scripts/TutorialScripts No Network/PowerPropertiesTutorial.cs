using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PowerPropertiesTutorial : MonoBehaviour {

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
            Debug.Log("start id 2");
            if (Network.isServer)
            {
                oldTarget = ias.getTarget();
                ias.setTarget(gameObject.transform);
            }
            else
            {
                oldTarget = ias.getTarget();
                networkView.RPC("serverChangeTarget", RPCMode.Server, gameObject.networkView.viewID,true);
            }

        }
	}
	void OnTriggerEnter(Collider col){
			
		switch (idPower) {
			
		case 1 : {
			
			//if (col.gameObject.tag == "Player" || col.gameObject.tag== "Enemy")
			//{
                PlayerCaracScript pcs = col.gameObject.GetComponent<PlayerCaracScript>();
                pcs.setTemporaryAlteration((-20), 3.0f, false);
                Network.Destroy(this.gameObject);	
			//}
		}
		break;
		case 2 : {
            if (col.gameObject.tag == "Enemy")
            {
                IAScript ias = GameObject.Find(_slender.name + "(Clone)").GetComponent<IAScript>();
                PlayerCaracScript pcs = col.gameObject.GetComponent<PlayerCaracScript>();
                pcs.setTemporaryAlteration(-20.0f, 1.2f, false);
                if (oldTarget != null)
                {
                    ias.setTarget(oldTarget);
                    if (Network.isClient)
                        networkView.RPC("serverChangeTarget", RPCMode.Server, oldTarget.networkView.viewID, false);
                }
                else
                {
                    ias._isOnAttack = false;
                    if (Network.isClient)
                        networkView.RPC("iaStopAttack", RPCMode.Server, false);
                }
                Network.Destroy(this.gameObject);
            }
        
        
        }
			break;
		case 3 : { }
			break;
		default :;
			break;
			
		}
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
    private void serverChangeTarget(NetworkViewID id,bool save_old)
    {
        IAScript ias = GameObject.Find(_slender.name + "(Clone)").GetComponent<IAScript>();
        if(save_old)
            oldTarget=ias.getTarget();
        if(!save_old && oldTarget != null)
            ias.setTarget(oldTarget);
        else
            ias.setTarget(NetworkView.Find(id).transform);

    }

    [RPC]
    private void iaStopAttack(bool isOnAttack)
    {
        IAScript ias = GameObject.Find(_slender.name + "(Clone)").GetComponent<IAScript>();
        ias._isOnAttack = isOnAttack;
    }
}
