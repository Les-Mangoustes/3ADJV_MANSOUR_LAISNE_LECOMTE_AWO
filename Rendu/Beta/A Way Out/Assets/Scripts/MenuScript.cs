using UnityEngine;
using System.Collections;

public class MenuScript : MonoBehaviour {

    private int menuState;
    private Rect center;
    private int heightButton = 25;
    private int heightSpace = 10;

    private string ip = "127.0.0.1";

    private StaticVariableScript setting;

    // Use this for initialization
    void Start()
    {
        int tier_width = Screen.width / 3;
        int tier_height = Screen.height / 3;
        center = new Rect(tier_width, tier_height, tier_width, 25);
        menuState = 0;
        setting = gameObject.GetComponent<StaticVariableScript>();
    }

    // Update is called once per frame
    void Update()
    {

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
            Application.LoadLevel("gameScene");


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
    }

    private void serverMenu()
    {
        setting.isServer = true;
        setting.ip = "0";
        menuState = 4;
    }

    private void clientMenu()
    {
        Rect centerOnMenu = center;
        GUI.Label(centerOnMenu, "IP to join : ");
        centerOnMenu.y = nextButtonHeight(centerOnMenu);
        ip = GUI.TextArea(centerOnMenu, ip);
        centerOnMenu.y = nextButtonHeight(centerOnMenu);
        if (GUI.Button(centerOnMenu, "Validate"))
        {
            setting.isServer = false;
            setting.ip = ip;
            menuState = 4;
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

    private float nextButtonHeight(Rect position)
    {
        return position.y + heightButton + heightSpace;
    }
}
