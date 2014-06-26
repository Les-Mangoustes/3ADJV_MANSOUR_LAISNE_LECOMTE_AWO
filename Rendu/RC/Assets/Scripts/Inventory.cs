using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Inventory : MonoBehaviour {

	private static int TailleStuff=5;
	private List<Item> stuffPlayer1 = new List<Item>(TailleStuff);
	private List<Item> stuffPlayer2 = new List<Item>(TailleStuff);
	private int itemSelectedP1=0;
	private int itemSelectedP2=0;

	void Start () {
	
		stuffPlayer1.Add(new Item("sac de viande", 2));
		stuffPlayer1.Add(new Item("piege à loup", 1));
		stuffPlayer2.Add(new Item("sac de viande", 2));
		stuffPlayer2.Add(new Item("piege à loup", 1));


	}

	void OnGUI(){
		
		GUI.Box(new Rect(10,10,110,130), "Items : ");
		
		if(Network.isServer && gameObject.name=="Player"){
			//if(networkView.isMine){
				for(int i=0;i<stuffPlayer1.Count;i++){
					if(i==itemSelectedP1){
						GUI.contentColor=Color.yellow;
					}else{
						GUI.contentColor=Color.blue;
					}
					GUI.Box (new Rect (15,(20*i)+30,100,20), stuffPlayer1[i].name);
				}
			//}
        }
        else if (Network.isClient && gameObject.name == "Player2")
        {
			//if(!networkView.isMine){
				for(int i=0;i<stuffPlayer2.Count;i++){
					if(i==itemSelectedP2){
						GUI.contentColor=Color.yellow;
					}else{
						GUI.contentColor=Color.blue;
					}
					GUI.Box (new Rect (15,(20*i)+30,100,20), stuffPlayer2[i].name);
				}
			//}
		}
	}


	void Update(){

		if(Network.isServer){
            if (stuffPlayer1.Count > 1)
            {
                if (Input.GetAxis("Mouse ScrollWheel") < 0 && itemSelectedP1 < stuffPlayer1.Count - 1) // back
                {
                    itemSelectedP1++;
                }
                if (Input.GetAxis("Mouse ScrollWheel") > 0 && itemSelectedP1 > 0) // forward
                {
                    itemSelectedP1--;
                }
            }
		}else{
            if (stuffPlayer2.Count > 1)
            {
                if (Input.GetAxis("Mouse ScrollWheel") < 0 && itemSelectedP2 < stuffPlayer2.Count - 1) // back
                {
                    itemSelectedP2++;
                }
                if (Input.GetAxis("Mouse ScrollWheel") > 0 && itemSelectedP2 > 0) // forward
                {
                    itemSelectedP2--;
                }
            }
		}
	}

    public int getCurrentItemId()
    {
        if (Network.isServer && gameObject.name == "Player")
        {
            if (stuffPlayer1.Count > 0)
                return stuffPlayer1[itemSelectedP1].id;
            else
                return -1;

        }
        else
        {
            if (stuffPlayer2.Count > 0)
                return stuffPlayer2[itemSelectedP2].id;
            else
                return -1;
        }
    }

    public int getCurrentItemIndex()
    {
        if (Network.isServer && gameObject.name == "Player")
        {
            if (stuffPlayer1.Count > 0)
                return itemSelectedP1;
            else
                return -1;
        }
        else if (Network.isClient && gameObject.name == "Player2")
        {
            if (stuffPlayer2.Count > 0)
                return itemSelectedP2;
            else
                return -1;
        }
        else
        {
            return -1;
        }
    }

    public void addItemToStuff(int player, string name, int id)
    {
        if (player == 1)
        {
            if (stuffPlayer1.Count < TailleStuff)
            {
                stuffPlayer1.Add(new Item(name, id));
            }
        }
        else
        {
            if (stuffPlayer2.Count < TailleStuff)
            {
                stuffPlayer2.Add(new Item(name, id));
            }
        }
    }

    public void removeItemAt(int index)
    {

        if (index >= 0 && index < TailleStuff)
        {

            if (Network.isServer)
            {
                if (index == stuffPlayer1.Count - 1)
                    itemSelectedP1--;
                stuffPlayer1.RemoveAt(index);
            }
            else
            {
                if (index == stuffPlayer2.Count - 1)
                    itemSelectedP2--;
                stuffPlayer2.RemoveAt(index);

            }
        }

    }
}

public class Item{

	public string name;
	public int id;

	public Item(){

		this.id=0;
		this.name="";
	}

	public Item(string name, int id){
	
		this.id=id;
		this.name=name;
	}

}
