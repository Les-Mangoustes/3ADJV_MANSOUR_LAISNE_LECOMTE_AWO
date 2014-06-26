using UnityEngine;
using System.Collections;

public class LobbyScript : MonoBehaviour {

    private bool _isOnLobby;
    private bool _enableStart;
    private Rect center;
    private StaticVariableScript settings;
    private NetworkPlayer _playerConnected;

    private int heightButton = 25;
    private int heightSpace = 10;
    private string message = "Waiting for other player";

	// Use this for initialization
	void Start () {
        _isOnLobby = false;
        int tier_width = Screen.width / 3;
        int tier_height = Screen.height / 3;
        center = new Rect(tier_width, tier_height, tier_width, 25);
        _enableStart = false;
        settings = gameObject.GetComponent<StaticVariableScript>();
	}

    public void enterLobby()
    {
        Application.runInBackground = true;

        if (settings.isServer)
        {
            Network.InitializeSecurity();
            if (settings.ip != "127.0.0.1")
            {
                Network.InitializeServer(1, 8080, !Network.HavePublicAddress());
                MasterServer.RegisterHost("A Way Out", settings.GameName, "");
            }
            else {
                Debug.Log("Local Client");
                Network.InitializeServer(8,26500,false);
            }
        }
        else
        {
            try{
                Network.Connect(settings.Element);
            }
            catch (System.Exception e)
            {
                Debug.Log("Local Client");
                Network.Connect("127.0.0.1",26500);
            }
        }
        _isOnLobby = true;
    }

    void OnFailedToConnectToMasterServer(NetworkConnectionError info)
    {
        gameObject.GetComponent<MenuScript>().enabled = true;
        _isOnLobby = false;
        Debug.Log("Could not connect to master server: " + info);
    }

    void OnPlayerConnected(NetworkPlayer player)
    {
        _playerConnected = player;
        message = "Player Connected";
        _enableStart = true;
    }

    void OnGUI()
    {
        if (_isOnLobby )
        {
            if (settings.isServer)
            {
                Rect centerOnMenu = center;
                GUI.Label(center, message);
                centerOnMenu.y = nextButtonHeight(centerOnMenu);
                GUI.enabled = _enableStart;
                if (GUI.Button(centerOnMenu, "Start"))
                {
                    if(settings.GameMode != "escape")
                        networkView.RPC("startGameSurvival", _playerConnected);
                    else
                        networkView.RPC("startGameEscape", _playerConnected);
                }
            }
            else
            {
                Rect centerOnMenu = center;
                GUI.Label(center, "The game will start soon");
            }
        }           
    }

    [RPC]
    void startGameSurvival()
    {
        Application.LoadLevel("gameScene");
        if(Network.isClient)
            networkView.RPC("startGameSurvival", RPCMode.Server);
    }

    [RPC]
    void startGameEscape()
    {
        Application.LoadLevel("gameEscape");
        if (Network.isClient)
            networkView.RPC("startGameEscape", RPCMode.Server);
    }

    private float nextButtonHeight(Rect position)
    {
        return position.y + heightButton + heightSpace;
    }
}
