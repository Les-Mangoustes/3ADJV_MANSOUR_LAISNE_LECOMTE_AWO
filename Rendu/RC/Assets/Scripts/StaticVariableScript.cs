using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StaticVariableScript : MonoBehaviour
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

    private static string _gameMode;

    public string GameMode
    {
        get { return _gameMode; }
        set { _gameMode = value; }
    }

    private static Dictionary<string, objectClass> _objectList;

    public Dictionary<string, objectClass> ObjectList
    {
        get { return _objectList; }
        set { _objectList = value; }
    }

    public void AddObjectlist(string key, objectClass value)
    {
        _objectList.Add(key,value);
    }
}
