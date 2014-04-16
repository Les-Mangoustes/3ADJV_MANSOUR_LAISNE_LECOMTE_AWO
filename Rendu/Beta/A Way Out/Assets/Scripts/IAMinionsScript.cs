using UnityEngine;
using System.Collections;

public class IAMinionsScript : MonoBehaviour {

    public float speed;

    //Factorisation du gameobject.transform
    private Transform _pnj;

    //spwan dist
    private float distance = 40.0f;

    //attack target
    private Transform targetOnAttack;

    //deplacement
    private AStarScript deplacementScript;

    void Start()
    {
        _pnj = gameObject.transform;
        deplacementScript = new AStarScript(networkView);
    }

    void FixedUpdate()
    {
        Vector3 deplacementChecked;
        if ((deplacementChecked = deplacementScript.checkPath(_pnj.position, speed)) != Vector3.zero)
        {
            _pnj.position += deplacementChecked;
        }
        else
        {
            randomPos();
        }
    }

    private void randomPos()
    {
        if (Network.isServer)
        {
            Vector3 newPosition = new Vector3(Random.Range(-distance, distance), 0, Random.Range(-distance, distance));
            deplacementScript.setTarget(_pnj.position, newPosition);
        }
    }

    [RPC]
    private void changeDirection(string pathCorner)
    {
        deplacementScript.changeDirection(pathCorner);
    }
}
