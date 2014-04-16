using UnityEngine;
using System.Collections;

public class NetworkScript : MonoBehaviour {

    private int _rangeX;
    private int _rangeZ;


    private StaticVariableScript settings;

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
            _rangeX = Mathf.FloorToInt(GameObject.Find("Plane").renderer.bounds.size.x)/2;
            _rangeZ = Mathf.FloorToInt(GameObject.Find("Plane").renderer.bounds.size.z)/2;
			Network.InitializeSecurity();
			Network.InitializeServer(1, 6600, true);
            Destroy(_player2.Find("CameraPlayer2").gameObject);
			Destroy(_player2.Find("light").gameObject);
		}
		else
		{
			Network.Connect(settings.ip, 6600);
            Destroy(_player1.Find("CameraPlayer1").gameObject);
			Destroy(_player1.Find("light").gameObject);
		}
	}

	void OnPlayerConnected(NetworkPlayer player)
	{
		if(Network.connections.Length == 1){
			Vector3 position_slender = new Vector3(Random.Range(-_rangeX,_rangeX),0,Random.Range(-_rangeZ,_rangeZ));
			Network.Instantiate(_slender, position_slender, Quaternion.identity, 0);
            GameObject.Find(_slender.name + "(Clone)").GetComponent<IAScript>().enabled = true;
            gameObject.GetComponent<InteractScript>().enabled = true;
            networkView.RPC("isInstantiate", RPCMode.Others);
            _player1.GetComponent<WhenPursuerCatchesAPlayer>().enabled = true;
            _player2.GetComponent<WhenPursuerCatchesAPlayer>().enabled = true;

            //for debug
            Vector3 position_object1 = new Vector3(Random.Range(-_rangeX, _rangeX), 0, Random.Range(-_rangeZ, _rangeZ));
            Network.Instantiate(_item, position_object1, Quaternion.identity, 0);
            Vector3 position_object2 = new Vector3(Random.Range(-_rangeX, _rangeX), 0, Random.Range(-_rangeZ, _rangeZ));
            Network.Instantiate(_item, position_object2, Quaternion.identity, 0);
            Vector3 position_object3 = new Vector3(Random.Range(-_rangeX, _rangeX), 0, Random.Range(-_rangeZ, _rangeZ));
            Network.Instantiate(_item, position_object3, Quaternion.identity, 0);
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
}
