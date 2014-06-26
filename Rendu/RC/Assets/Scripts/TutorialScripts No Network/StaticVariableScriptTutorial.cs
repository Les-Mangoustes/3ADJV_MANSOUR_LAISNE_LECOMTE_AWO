using UnityEngine;
using System.Collections;

public class StaticVariableScriptTutorial : MonoBehaviour
{
    private static bool _isServer;

    public bool isServer
    {
        get { return _isServer; }
        set { _isServer = value; }
    }

    private static string _ip;

    public string ip
    {
        get { return _ip; }
        set { _ip = value; }
    }

    private static HostData _element;

    public HostData Element
    {
        get { return _element; }
        set { _element = value; }
    }

    private static string _gameName;

    public string GameName
    {
        get { return _gameName; }
        set { _gameName = value; }
    }
}
