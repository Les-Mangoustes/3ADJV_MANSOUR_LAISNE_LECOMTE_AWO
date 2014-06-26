using UnityEngine;
using System.Collections;

public class WhenPursuerCatchesAPlayer : MonoBehaviour {

    [SerializeField]
    GameObject handler;

    [SerializeField]
    Transform _slender;

	bool GameFinished = false;

	string _messageBoard = "";

	void OnTriggerEnter(Collider col) {
        if (GameObject.Find("SphereEnemy(Clone)") != null && col.transform == GameObject.Find("SphereEnemy(Clone)").transform)
        {
            GameFinished = true;
            handler.GetComponent<InteractScript>().enabled = false;
            GameObject.Find(_slender.name + "(Clone)").GetComponent<IAScript>().enabled = false;
            audio.Play();
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
            if (Network.isServer)
            {
                if (GUI.Button(new Rect(Screen.width / 2 - 50, Screen.height / 2 + 25, 100, 25), "Try Again"))
                    networkView.RPC("startGame", RPCMode.Others);
                if (GUI.Button(new Rect(Screen.width / 2 - 50, Screen.height / 2 + 55, 100, 25), "Return To Menu"))
                    networkView.RPC("returnMenu", RPCMode.Others,true);
                if (GUI.Button(new Rect(Screen.width / 2 - 50, Screen.height / 2 + 85, 100, 25), "Close"))
                {
                    networkView.RPC("returnMenu", RPCMode.Others,false);
                    Application.Quit();
                }

            }
            else
            {
                GUI.Label(new Rect(Screen.width / 2 - 50, Screen.height / 2 + 25, 100, 50), "Waiting for Server choice.");
                if (GUI.Button(new Rect(Screen.width / 2 - 50, Screen.height / 2 + 80, 100, 25), "Close"))
                    Application.Quit();

            }
        }
	}

    [RPC]
    void returnMenu(bool notClose)
    {
        Application.LoadLevel("MenuScene");
        if (Network.isClient && notClose)
            networkView.RPC("returnMenu", RPCMode.Server);
        Network.Disconnect();
    }

    [RPC]
    void startGame()
    {
        Application.LoadLevel("gameScene");
        if (Network.isClient)
            networkView.RPC("startGame", RPCMode.Server);

    }
}
