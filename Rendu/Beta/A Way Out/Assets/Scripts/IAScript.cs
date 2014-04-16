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
        if ((deplacementChecked = deplacementScript.checkPath(_pnj.position, slenderCarac.getSpeed())) != Vector3.zero)
        {
            Debug.Log("Move");
            _pnj.position += deplacementChecked;
        }
        if (_isOnAttack)//poursuite dynamique
        {
            if (targetOnAttack.position != destTarget)
                deplacementScript.setTarget(_pnj.position, targetOnAttack.position);
        }
	}

    void Update()
    {
        timeCounter += Time.deltaTime;
        if (timeCounter > 120 && timeCounter < 200)
        {
            slenderCarac.BaseSpeed = slenderCarac.BaseSpeed + 3 / 2;
        }
        else if (timeCounter > 200 && timeCounter < 360)
        {
            slenderCarac.BaseSpeed = slenderCarac.BaseSpeed + 3;
        }
        else if (timeCounter > 360)
        {
            slenderCarac.BaseSpeed = 2 * slenderCarac.BaseSpeed;
        }    
	}

    public void setTarget(Transform target){
        targetOnAttack = target;
        destTarget = target.position;
        _isOnAttack = true;
        deplacementScript.setTarget(_pnj.position, destTarget);
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
}