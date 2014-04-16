using UnityEngine;
using System.Collections;

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
}
