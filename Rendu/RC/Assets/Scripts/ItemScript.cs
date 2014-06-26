using UnityEngine;
using System.Collections;

public class ItemScript : MonoBehaviour {
	[SerializeField]
	private string nameItem;
	private int id;



	// Use this for initialization
	void Start () {
		id = Random.Range(1, 3);

		switch(id){
			case 1 : {
				nameItem="piège à loup";
				gameObject.renderer.material.color=Color.blue;
			}
			break;
			case 2 : {
				nameItem="Sac de viande";
				gameObject.renderer.material.color=Color.red;
			}
			break;
			case 3 : {
                nameItem = "Stèle Mystique";
				gameObject.renderer.material.color=Color.yellow;
			}
			break;
			default : nameItem="";
			break;

		}

	}

	void OnTriggerEnter(Collider col)
	{
		if (col.gameObject.tag == "Player")
		{
			Inventory inv = col.gameObject.GetComponent<Inventory>();
			if (col.gameObject.name == "Player")
			{
				inv.addItemToStuff(1, nameItem, id);
			}else{
				inv.addItemToStuff(2, nameItem, id);
			}
            if(Network.isServer)
			    Network.Destroy(this.gameObject);
		}
	}
}
