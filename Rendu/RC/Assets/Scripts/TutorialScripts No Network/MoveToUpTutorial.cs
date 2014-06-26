using UnityEngine;
using System.Collections;

public class MoveToUpTutorial : MonoBehaviour {

	[SerializeField]
	NavMeshAgent _agent;

	public float _slenderSpeed;
	[SerializeField]
	private float _slenderBaseSpeed;

	[SerializeField]
	Vector3 _targetPosition;

	[SerializeField]
	Transform _target;

	[SerializeField]
	float _top;
	[SerializeField]
	float _left;
	[SerializeField]
	float _width;

	float timeCounter = 0;
	// Use this for initialization
	void Start () {
		_slenderSpeed = _slenderBaseSpeed;
	}

	public void setTarget(Transform _targetTransform){
		this._target = _targetTransform;
	}

	
	// Update is called once per frame
	void Update () {
		_agent.SetDestination(_target.position);
		_agent.speed = _slenderSpeed;
	}
}
