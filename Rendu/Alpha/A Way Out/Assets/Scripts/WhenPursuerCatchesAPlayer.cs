using UnityEngine;
using System.Collections;

public class WhenPursuerCatchesAPlayer : MonoBehaviour {

	[SerializeField]
	bool GameFinished = false;
	string _messageBoard = "";



	void OnTriggerEnter(Collider col) {
	
		GameFinished = true;

	}
	void OnGUI()
	{
		GUI.Box(new Rect (0, 0, Screen.width, Screen.height), GUIContent.none);
		GUI.Label(new Rect (Screen.width/2-50, Screen.height/2-25, 100, 50), _messageBoard);
	}


	void Update(){
		if(GameFinished == true){
			_messageBoard = "YOU ARE DEAD";
			GameObject.FindGameObjectWithTag("MainCamera").transform.parent = null;
			GameObject.FindGameObjectWithTag("Target").transform.localPosition = new Vector3(0,-50,0);
			GameObject.FindGameObjectWithTag("Light").transform.localPosition = new Vector3(0,-50,0);
		}
	}
}
