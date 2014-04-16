using UnityEngine;
using System.Collections;

public class WhenPursuerCatchesAPlayer : MonoBehaviour {

	bool GameFinished = false;

    bool isServerWhoLoose = false;

	string _messageBoard = "";

	void OnTriggerEnter(Collider col) {
        if (GameObject.Find("SphereEnemy(Clone)") != null && col.transform == GameObject.Find("SphereEnemy(Clone)").transform)
        {
            GameFinished = true;
            if (Network.isServer)
                if (gameObject.name == "Player")
                    _messageBoard = "YOU ARE DEAD";
                else
                    _messageBoard = "YOU HAVE SURVIVED (for the moment)";
            else
                if (gameObject.name == "Player2")
                    _messageBoard = "YOU ARE DEAD";
                else
                    _messageBoard = "YOU HAVE SURVIVED (for the moment)";
        }
	}

	void OnGUI()
	{
        if (GameFinished)
        {
            GUI.Box(new Rect(0, 0, Screen.width, Screen.height), GUIContent.none);
            GUI.Label(new Rect(Screen.width / 2 - 50, Screen.height / 2 - 25, 100, 50), _messageBoard);
        }
	}
}
