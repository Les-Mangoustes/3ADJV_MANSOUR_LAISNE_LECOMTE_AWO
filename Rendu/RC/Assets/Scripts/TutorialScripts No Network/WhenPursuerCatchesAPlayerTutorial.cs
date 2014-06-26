using UnityEngine;
using System.Collections;

public class WhenPursuerCatchesAPlayerTutorial : MonoBehaviour {

    [SerializeField]
    Transform _slender;

	bool GameFinished = false;

	string _messageBoard = "";

	void OnTriggerEnter(Collider col) {
        if (GameObject.Find("SphereEnemy") != null && col.transform == GameObject.Find("SphereEnemy").transform)
        {
            GameFinished = true;
            audio.Play();
            if (true)
                if (gameObject.name == "Player")
                    _messageBoard = "You are dead, that's bad";
                else
                    _messageBoard = "Tutorial is over, and your opponent is dead";
        }
	}

	void OnGUI()
	{
        if (GameFinished)
        {
            GUI.Box(new Rect(0, 0, Screen.width, Screen.height), GUIContent.none);
            GUI.Label(new Rect(Screen.width / 2 - 50, Screen.height / 2 - 25, 100, 50), _messageBoard);

                if (GUI.Button(new Rect(Screen.width / 2 - 50, Screen.height / 2 + 55, 100, 25), "Return To Menu"))
                    Application.LoadLevel("MenuScene");	

            
        }
	}
}
