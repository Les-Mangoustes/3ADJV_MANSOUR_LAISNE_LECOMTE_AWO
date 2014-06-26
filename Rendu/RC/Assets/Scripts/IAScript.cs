using UnityEngine;
using System.Collections;

public class IAScript : MonoBehaviour {

    private float timeCounter;
    private PlayerCaracScript slenderCarac;

    //Factorisation du gameobject.transform
    private Transform _pnj;

    //attack target
    private Transform targetOnAttack;
    private Vector3 destTarget;
    public bool _isOnAttack;

    //deplacement
    private AStarScript deplacementScript;

	void Start() 
    {
        _pnj = gameObject.transform;
        deplacementScript = new AStarScript(networkView);
        timeCounter = 0;
        slenderCarac = gameObject.GetComponent<PlayerCaracScript>();
	}

	void FixedUpdate () 
    {
        Vector3 deplacementChecked;
        if ((deplacementChecked = deplacementScript.checkPath(_pnj.position, slenderCarac.getSpeedWithAlt())) != Vector3.zero)
        {
            _pnj.position += deplacementChecked;
            if (_isOnAttack)//poursuite dynamique
            {
                if (targetOnAttack.position != destTarget)
                    deplacementScript.setTarget(_pnj.position, targetOnAttack.position);
            }
        }        
        else
        {
            randomPos();
        }
	}

    private void randomPos()
    {
        //pour des problèmes de performance on ne check qu'a l'initialisation.
        //Cela est du au faite que le Physics.elapseSphere est couteux et donc le faire plusieurs fois consomme beaucoup de ressource
        if (Network.isServer)
        {
            Vector3 pos = pos = new Vector3(Random.Range(-checkPosition._rangeX, checkPosition._rangeX), 1, Random.Range(-checkPosition._rangeZ, checkPosition._rangeZ));
            deplacementScript.setTarget(_pnj.position, pos);
        }
    }

    void Update()
    {
        timeCounter += Time.deltaTime;
        if (timeCounter > 0 && timeCounter < 12000)
        {
            slenderCarac.speed += slenderCarac.BaseSpeed / 6000;
        }
        else if (timeCounter >= 12000 && timeCounter < 24000)
        {
            slenderCarac.speed += slenderCarac.BaseSpeed / 3000;
        }  
	}

    public void setTarget(Transform target)
    {
        //Debug.Log("change target : " + target);
        targetOnAttack = target;
        destTarget = target.position;
        _isOnAttack = true;
        try //erreur possible a l'initialisation
        {
            deplacementScript.setTarget(_pnj.position, destTarget);
        }
        catch
        {

        }
    }

    public Transform getTarget()
    {
        return this.targetOnAttack;
    }

    [RPC]
    private void changeDirection(string pathCorner)
    {
        deplacementScript.changeDirection(pathCorner);
    }

    [RPC]
    private void setTargetByPlayer(NetworkViewID id)
    {
        setTarget(NetworkView.Find(id).transform);
    }
}