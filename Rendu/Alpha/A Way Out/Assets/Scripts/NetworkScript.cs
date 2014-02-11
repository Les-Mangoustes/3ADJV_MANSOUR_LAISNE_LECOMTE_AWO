using UnityEngine;
using System.Collections;

public class NetworkScript : MonoBehaviour {
	
	private int _range = 50;
	
	[SerializeField]
	private bool _isBuildingServer = true;
	
	[SerializeField]
	private Transform _slender;
	
	[SerializeField]
	private Transform _player1;
	
	[SerializeField]
	private Transform _player2;
	
	[SerializeField]
	private string _ip;
	
	private bool _clientWantToMoveDown = false;
	private bool _clientWantToMoveUp = false;
	private bool _clientWantToMoveLeft = false;
	private bool _clientWantToMoveRight = false;
	
	// Use this for initialization
	void Start () {
		Application.runInBackground = true;
		
		if (_isBuildingServer)
		{
			Network.InitializeSecurity();
			Network.InitializeServer(1, 9090, true);
			Destroy(_player2.Find("CameraPlayer2").gameObject);
			Destroy(_player2.Find("light").gameObject);
		}
		else
		{
			Network.Connect(_ip, 9090); //"127.0.0.1"
			Destroy(_player1.Find("CameraPlayer2").gameObject);
			Destroy(_player1.Find("light").gameObject);
		}
	}
	
	void OnConnectedToServer()
	{ 
		
	}
	
	void OnPlayerConnected(NetworkPlayer player)
	{
		if(Network.connections.Length == 1){
			Vector3 position_slender = new Vector3(Random.Range(-_range,_range),0,Random.Range(-_range,_range));
			Network.Instantiate(_slender, position_slender, Quaternion.identity, 0);
		}
	}
	
	void Update()
	{
		if(Network.isClient)
		{
			if (Input.GetKeyDown(KeyCode.S))
			{
				networkView.RPC("ClientBeginMoveDown", RPCMode.Server);
			}
			if (Input.GetKeyDown(KeyCode.Z))
			{
				networkView.RPC("ClientBeginMoveUp", RPCMode.Server);
			}
			if (Input.GetKeyUp(KeyCode.S))
			{
				networkView.RPC("ClientEndMoveDown", RPCMode.Server);
			}
			if (Input.GetKeyUp(KeyCode.Z))
			{
				networkView.RPC("ClientEndMoveUp", RPCMode.Server);
			}
			if (Input.GetKeyDown(KeyCode.D))
			{
				networkView.RPC("ClientBeginMoveRight", RPCMode.Server);
			}
			if (Input.GetKeyDown(KeyCode.Q))
			{
				networkView.RPC("ClientBeginMoveLeft", RPCMode.Server);
			}
			if (Input.GetKeyUp(KeyCode.D))
			{
				networkView.RPC("ClientEndMoveRight", RPCMode.Server);
			}
			if (Input.GetKeyUp(KeyCode.Q))
			{
				networkView.RPC("ClientEndMoveLeft", RPCMode.Server);
			}
		}
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		check_deplacement(15f);
	}
	
	void check_deplacement(float walkSpeed){
		if (Network.isServer)
		{
			if (Input.GetKey(KeyCode.S))
			{
				_player1.position += -Vector3.forward *
					walkSpeed * Time.deltaTime;
			}
			if (Input.GetKey(KeyCode.Z))
			{
				_player1.position += Vector3.forward *
					walkSpeed * Time.deltaTime;
			}
			if (Input.GetKey(KeyCode.D))
			{
				_player1.position += Vector3.right *
					walkSpeed * Time.deltaTime;
			}
			if (Input.GetKey(KeyCode.Q))
			{
				_player1.position += Vector3.left *
					walkSpeed * Time.deltaTime;
			}
			if (_clientWantToMoveDown)
			{
				_player2.position += -Vector3.forward *
					walkSpeed * Time.deltaTime;
			}
			if (_clientWantToMoveUp)
			{
				_player2.position += Vector3.forward *
					walkSpeed * Time.deltaTime;
			}
			if (_clientWantToMoveRight)
			{
				_player2.position += Vector3.right *
					walkSpeed * Time.deltaTime;
			}
			if (_clientWantToMoveLeft)
			{
				_player2.position += Vector3.left *
					walkSpeed * Time.deltaTime;
			}
		}
	}
	
	[RPC]
	public void ClientBeginMoveUp()
	{
		_clientWantToMoveUp = true;
	}
	
	[RPC]
	public void ClientBeginMoveDown()
	{
		_clientWantToMoveDown = true;
	}
	
	[RPC]
	public void ClientEndMoveUp()
	{
		_clientWantToMoveUp = false;
	}
	
	[RPC]
	public void ClientEndMoveDown()
	{
		_clientWantToMoveDown = false;
	}
	
	[RPC]
	public void ClientBeginMoveRight()
	{
		_clientWantToMoveRight = true;
	}
	
	[RPC]
	public void ClientBeginMoveLeft()
	{
		_clientWantToMoveLeft = true;
	}
	
	[RPC]
	public void ClientEndMoveRight()
	{
		_clientWantToMoveRight = false;
	}
	
	[RPC]
	public void ClientEndMoveLeft()
	{
		_clientWantToMoveLeft = false;
	}
}
