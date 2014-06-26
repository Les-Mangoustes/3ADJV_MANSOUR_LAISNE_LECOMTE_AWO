using UnityEngine;
using System.Collections;

public class MoveToUp : MonoBehaviour {

	//[SerializeField]
	//NavMeshAgent _agent;

	public float _slenderSpeed;
	[SerializeField]
	private float _slenderBaseSpeed;

	//[SerializeField]
	//Vector3 _targetPosition;

	[SerializeField]
	Transform _target;

	[SerializeField]
	float _top;
	[SerializeField]
	float _left;
	[SerializeField]
	float _width;



	[SerializeField]
	string _messageBoard = "";

	float timeCounter = 0;
	// Use this for initialization
	void Start () {
		_slenderSpeed = _slenderBaseSpeed;
	}

	public void setTarget(Transform _targetTransform){
		this._target = _targetTransform;
	}

	void OnGUI()
	{
		GUI.Label(new Rect(0, Screen.height-50, Screen.width, Screen.height), _messageBoard);
	}
	
	// Update is called once per frame
	void Update () {
		//_agent.SetDestination(_target.position);
		//_agent.speed = _slenderSpeed;

		timeCounter += Time.deltaTime;
		if (timeCounter > 120 && timeCounter < 200){
			_slenderSpeed = _slenderBaseSpeed + 3/2;
		}
		else if (timeCounter > 200 && timeCounter < 360){
			_slenderSpeed = _slenderBaseSpeed + 3;
		}
		else if (timeCounter > 360){
			_slenderSpeed = 2 * _slenderBaseSpeed;
		}
		if (timeCounter > 5 && timeCounter < 12){ 
			_messageBoard = "It is so dark here, where am I ?";
		}
		else if (timeCounter > 20 && timeCounter < 27){ 
			_messageBoard = "Damn... Looks like something is after me... ";
		}
		else if (timeCounter > 32 && timeCounter < 38){ 
			_messageBoard = "Holy shit, this thing is so huge ";
		}
		else if (timeCounter > 75 && timeCounter < 80){ 
			_messageBoard = "Run... run... run ! ";
		}
		else if (timeCounter > 130 && timeCounter < 145){ 
			_messageBoard = "Fuck, it is getting angry... It is so fast ! ";
		}
		else {
			_messageBoard = "";
		}
	}
}
