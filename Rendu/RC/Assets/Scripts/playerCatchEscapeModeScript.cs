using UnityEngine;
using System.Collections;

public class playerCatchEscapeModeScript : MonoBehaviour {

    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Enemy" && GameObject.Find("handler").GetComponent<GetKeyScript>().haveKey() )
        {
            Network.RemoveRPCs(other.gameObject.networkView.viewID);
            Network.Destroy(other.gameObject);
            GameObject.Find("handler").GetComponent<GetKeyScript>().dropKeyEnnemy();
            GameObject.FindWithTag("cylinder").GetComponent<SpawnKeyScript>().respawnKey(); ;
        }
    }
}
