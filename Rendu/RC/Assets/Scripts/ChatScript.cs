using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;


public class ChatScript : MonoBehaviour {

    public GUISkin skin;

    public static bool usingChat = false;  //Can be used to determine if we need to stop player movement since we're chatting

    static bool showChat = true;           //Show/Hide the chat
 
    //Private vars used by the script
    private String inputField = "";
    private Vector2 scrollPosition = new Vector2();
    private int width = 190;
    private int height = 180;
    private String playerName;
    private float lastUnfocus = 0;
    private Rect window;
    private float lastEntry = 0;
    private ArrayList chatEntries = new ArrayList();
    private ChatScript thisScript;
    private NetworkView netWiew;
    private String inputFieldFocus = "iFF";
 
    private string messBox = "", messageToSend = "", user = "";

    class ChatEntry
    {
       public String name = "";
       public String text = "";

       public ChatEntry()
       {
           this.text = "";
           if (Network.isServer) { 
            this.name = "Player 1";
           }
           else
           {
               this.name = "Player 2";
           }
       }
    }

    void Awake()
    {
        usingChat = false;
        netWiew = networkView;
        thisScript = this;

        window = new Rect(Screen.width - 200, Screen.height- 200 , width, height);
        
        lastEntry = Time.time;
    }

    void CloseChatWindow()
    {
        showChat = false;
        inputField = "";
        chatEntries = new ArrayList();
    }
 
    void ShowChatWindow ()
    {
        showChat = true;
        inputField = "";
        chatEntries = new ArrayList();
    }
 
    void OnGUI()
    {
        if(!showChat){
            return;
        }
   
        GUI.skin = skin;

        if (Event.current.type == EventType.keyDown && Event.current.character == '\n' && !usingChat)//&& inputField.Length <= 0
        {
            //if (lastUnfocus + 0.25 < Time.time)
            //{
                usingChat = true;
                GUI.FocusWindow(5);
                GUI.FocusControl(inputFieldFocus);
                Screen.lockCursor = false;
            //}
        }
 
        //Screen.lockCursor = screenLock;
        window = GUI.Window (5, window, GlobalChatWindow, "");
    }
 
 
    void GlobalChatWindow(int id ) {

        GUILayout.BeginVertical();
        GUILayout.Space(10);
        GUILayout.EndVertical();
     
   
        // Begin a scroll view. All rects are calculated automatically -
        // it will use up any available screen space and make sure contents flow correctly.
        // This is kept small with the last two parameters to force scrollbars to appear.
        scrollPosition = GUILayout.BeginScrollView(scrollPosition);
 
        foreach (ChatEntry entry in chatEntries)
        {
            GUILayout.BeginHorizontal();
            GUI.skin.label.wordWrap = true;
            GUILayout.TextField(entry.name+": "+entry.text);
            GUILayout.EndHorizontal();
            GUILayout.Space(3);
        }
        // End the scrollview we began above.
        GUILayout.EndScrollView ();

   
        
        if (Event.current.type == EventType.keyDown && Event.current.character == '\n' && inputField.Length > 0)
        {
            HitEnter(inputField);
        }
        else if (Event.current.type == EventType.keyDown && Event.current.character == '\n' && inputField.Length == 0){
            inputField = ""; //Clear line
            GUI.UnfocusWindow ();//Deselect chat
            lastUnfocus=Time.time;
            usingChat=false;
            Screen.lockCursor = true;
        }
        GUI.SetNextControlName(inputFieldFocus);
        inputField = GUILayout.TextField(inputField);

        if (Input.GetKeyDown("mouse 0"))
        {
            if (usingChat)
            {
                usingChat = false;
                GUI.UnfocusWindow();//Deselect chat
                lastUnfocus = Time.time;
            }
        }
        scrollPosition.y = float.PositiveInfinity;

 
    }
 
    void HitEnter(String msg ){
        Debug.Log(msg);
        msg = msg.Replace("\n", "");
        netWiew.RPC("ApplyGlobalChatText", RPCMode.All, msg);
        inputField = ""; //Clear line
        GUI.UnfocusWindow ();//Deselect chat
        lastUnfocus=Time.time;
        usingChat=false;
        Screen.lockCursor = true;
    }   

    [RPC]
    void ApplyGlobalChatText( String msg)
    {
        ChatEntry entry = new ChatEntry();
        entry.text = msg;
    Debug.Log(msg);
        chatEntries.Add(entry);
        Debug.Log(((ChatEntry)chatEntries[0]).name);

        lastEntry = Time.time;
   
        //Remove old entries
        if (chatEntries.Count > 4){
            chatEntries.RemoveAt(0);
        }
        scrollPosition.y = float.PositiveInfinity;
    }

}