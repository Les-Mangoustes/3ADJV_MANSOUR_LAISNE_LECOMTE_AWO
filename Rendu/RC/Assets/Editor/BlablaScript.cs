using UnityEngine;
using UnityEditor;
using System.Collections;

public class BlablaScript : Editor {

    [MenuItem("blabla/blabla")]
    public static void blabla()
    {
        checkPosition.addRandomPos();
    }
}
