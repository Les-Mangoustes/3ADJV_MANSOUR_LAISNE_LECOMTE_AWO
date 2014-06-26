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
        //pour des problèmes de performance on ne check qu'a l'initialisation.
        //Cela est du au faite que le Physics.elapseSphere est couteux et donc le faire plusieurs fois consomme beaucoup de ressource
        if (Network.isServer)
        {
            Vector3 pos = pos = new Vector3(Random.Range(-checkPosition._rangeX, checkPosition._rangeX), 1, Random.Range(-checkPosition._rangeZ, checkPosition._rangeZ));
            deplacementScript.setTarget(_pnj.position, pos);
        }
    }

    [RPC]
    private void changeDirection(string pathCorner)
    {
        deplacementScript.changeDirection(pathCorner);
    }
}
