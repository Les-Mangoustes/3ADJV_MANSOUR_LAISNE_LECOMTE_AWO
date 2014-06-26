using UnityEngine;
using System.Collections;

public class NetworkScriptTutorial : MonoBehaviour {

    private StaticVariableScript settings;

    [SerializeField]
    private Camera cam;

    [SerializeField]
    private Light light;

    [SerializeField]
    private Transform _item;

    [SerializeField]
    private Transform _slender;

    [SerializeField]
    private Transform _player1;

    [SerializeField]
    private Transform _player2;

	// Use this for initialization
	void Start () {

        settings = gameObject.GetComponent<StaticVariableScript>();

		Application.runInBackground = true;
		
		if (settings.isServer)
		{
            _player1.position = checkPosition.addRandomPos();
            cam.transform.parent = _player1;
            cam.transform.localPosition = new Vector3(0, 8, 0);
            light.transform.parent = _player1;
            light.transform.localPosition = new Vector3(0, 1.5f, 0);
            instantiateAll();
		}
		else
		{
            networkView.RPC("sendSpawnPosPlayer2",RPCMode.Server, checkPosition.addRandomPos());
            cam.transform.parent = _player2;
            cam.transform.localPosition = new Vector3(0, 8, 0);
            light.transform.parent = _player2;
            light.transform.localPosition = new Vector3(0, 1.5f, 0);
		}
	}

	void instantiateAll()
	{
		if(Network.connections.Length == 1){
            Network.Instantiate(_slender, checkPosition.addRandomPos(), Quaternion.identity, 0);
            GameObject.Find(_slender.name + "(Clone)").GetComponent<IAScript>().enabled = true;
            //
            gameObject.GetComponent<InteractScript>().enabled = true;
            networkView.RPC("isInstantiate", RPCMode.Others);
            //
            _player1.GetComponent<WhenPursuerCatchesAPlayer>().enabled = true;
            _player2.GetComponent<WhenPursuerCatchesAPlayer>().enabled = true;

            //for debug
            Network.Instantiate(_item, checkPosition.addRandomPos(), Quaternion.identity, 0);
            Network.Instantiate(_item, checkPosition.addRandomPos(), Quaternion.identity, 0);
            Network.Instantiate(_item, checkPosition.addRandomPos(), Quaternion.identity, 0);
		}
	}


    [RPC]
    public void isInstantiate()
    {
        gameObject.GetComponent<InteractScript>().enabled = true;
        GameObject.Find(_slender.name + "(Clone)").GetComponent<IAScript>().enabled = true;
        _player1.GetComponent<WhenPursuerCatchesAPlayer>().enabled = true;
        _player2.GetComponent<WhenPursuerCatchesAPlayer>().enabled = true;
    }

    [RPC]
    public void sendSpawnPosPlayer2(Vector3 pos)
    {
        _player2.position = pos;
    }
}
