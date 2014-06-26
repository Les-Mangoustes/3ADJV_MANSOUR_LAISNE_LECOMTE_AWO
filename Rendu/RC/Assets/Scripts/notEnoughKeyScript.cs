using UnityEngine;
using System.Collections;

public class notEnoughKeyScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
        object[] obj = { 10.0f };
        StartCoroutine("closeMessage", obj);
	}

    void OnGUI()
    {
        GUI.Label(new Rect(Screen.width / 2 - 50, Screen.height / 2 - 25, 100, 50), "Not enough key to escape ! ");
    }

    public IEnumerator closeMessage(object[] obj)
    {
        yield return new WaitForSeconds((float)obj[0]);
        this.enabled = false;
    }
}
