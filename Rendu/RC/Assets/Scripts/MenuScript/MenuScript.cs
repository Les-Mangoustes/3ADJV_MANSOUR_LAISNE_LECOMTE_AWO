using UnityEngine;
using System.Collections;

public class MenuScript : MonoBehaviour {

    [SerializeField]
    private GameObject handler;

    private int menuState;
    private Rect center;
    private int heightButton = 25;
    private int heightSpace = 10;

    private string ip = "127.0.0.1";

    private string modeType = "Survival Mode";

    private StaticVariableScript setting;

    private string message = "";

    private string name = "";

    // Use this for initialization
    void Start()
    {
        Application.runInBackground = true;
        int tier_width = Screen.width / 3;
        int tier_height = Screen.height / 3;
        center = new Rect(tier_width, tier_height, tier_width, 25);
        menuState = 0;
        setting = gameObject.GetComponent<StaticVariableScript>();
    }

    void OnGUI()
    {
        if (menuState == 0)
            basicMenu();
        else if (menuState == 1)
            serverMenu();
        else if (menuState == 2)
            clientMenu();
        else if (menuState == 3)
            optionsMenu();
        else if (menuState == 4)
            Application.LoadLevel("gameSceneTutorial");
    }

    private void basicMenu()
    {
        Rect centerOnMenu = center;
        if (GUI.Button(centerOnMenu, "Serveur"))
            menuState = 1;
        centerOnMenu.y = nextButtonHeight(centerOnMenu);
        if (GUI.Button(centerOnMenu, "Client"))
            menuState = 2;
        centerOnMenu.y = nextButtonHeight(centerOnMenu);
        if (GUI.Button(centerOnMenu, "Options"))
            menuState = 3;
        centerOnMenu.y = nextButtonHeight(centerOnMenu);
        if (GUI.Button(centerOnMenu, "Tutorial"))
            menuState = 4;
        centerOnMenu.y = nextButtonHeight(centerOnMenu);
    }

    private void serverMenu()
    {
        Rect centerOnMenu = center;
        if (GUI.Button(centerOnMenu, modeType))
        {
            if (modeType == "Survival Mode")
            {
                modeType = "Escape Mode";
                setting.GameMode = "escape";
            }
            else
            {
                modeType = "Survival Mode";
                setting.GameMode = "";
            }
        }
        centerOnMenu.y = nextButtonHeight(centerOnMenu);
        GUI.Label(centerOnMenu, "Name : ");
        centerOnMenu.y = nextButtonHeight(centerOnMenu);
        GUI.Label(centerOnMenu, message);
        centerOnMenu.y = nextButtonHeight(centerOnMenu);
        name = GUI.TextArea(centerOnMenu, name);
        centerOnMenu.y = nextButtonHeight(centerOnMenu);
        if (GUI.Button(centerOnMenu,"Create"))
        {
            if (name != "")
            {
                setting.GameName =  modeType + " - " + name;
                setting.isServer = true;
                setting.ip = "0";
                redirectToLobby();
            }
            else
            {
                message = "Please Enter a Name";
            }
        }

        centerOnMenu.y = nextButtonHeight(centerOnMenu);
        if (GUI.Button(centerOnMenu, "Local Play"))
        {
            setting.GameName = "Localhost";
            setting.isServer = true;
            setting.ip = "127.0.0.1";
            redirectToLobby();
        }

        centerOnMenu.y = nextButtonHeight(centerOnMenu);
        if (GUI.Button(centerOnMenu, "Previous"))
            menuState = 0;
    }

    private void clientMenu()
    {
        MasterServer.RequestHostList("A Way Out");
        Rect centerOnMenu = center;
        centerOnMenu.height = 50;
        HostData[] data = MasterServer.PollHostList();
	    // Go through all the hosts in the host list
	    foreach (HostData element in data)
	    {
            GUILayout.BeginArea(centerOnMenu);
		    GUILayout.BeginHorizontal();	
		    string name = element.gameName + " " + element.connectedPlayers + " / " + element.playerLimit;
		    GUILayout.Label(name);	
		    GUILayout.Space(5);
		    string hostInfo;
		    hostInfo = "[";
		    foreach (string host in element.ip)
			    hostInfo = hostInfo + host + ":" + element.port + " ";
		    hostInfo = hostInfo + "]";
		    GUILayout.Label(hostInfo);	
		    GUILayout.Space(5);
		    GUILayout.Label(element.comment);
		    GUILayout.Space(5);
		    GUILayout.FlexibleSpace();
		    if (GUILayout.Button("Connect"))
		    {
                setting.Element = element;
                if (element.gameName.Contains("Escape"))
                    setting.GameMode = "escape";
                redirectToLobby();
		    }
		    GUILayout.EndHorizontal();
            centerOnMenu.y = centerOnMenu.y + 50 + heightSpace;
            GUILayout.EndArea();
	    }

        centerOnMenu.y = nextButtonHeight(centerOnMenu);
        if (GUI.Button(centerOnMenu, "Local Play"))
        {
            redirectToLobby();
        }

        centerOnMenu.y = nextButtonHeight(centerOnMenu);
        if (GUI.Button(centerOnMenu, "Previous"))
            menuState = 0;
    }

    private void optionsMenu()
    {
        Rect centerOnMenu = center;
        GUI.Label(centerOnMenu, "No Options Availible for the moment");
        centerOnMenu.y = nextButtonHeight(centerOnMenu);
        if (GUI.Button(centerOnMenu, "Previous"))
            menuState = 0;
    }

    private void redirectToLobby()
    {
        this.enabled = false;
        handler.GetComponent<LobbyScript>().enterLobby();
    }

    private float nextButtonHeight(Rect position)
    {
        return position.y + heightButton + heightSpace;
    }
}
