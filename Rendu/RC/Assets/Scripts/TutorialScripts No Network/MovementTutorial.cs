using UnityEngine;
using System.Collections;

public class MovementTutorial : MonoBehaviour {

	[SerializeField]
	private float _walkSpeed =5f;
	[SerializeField]
	private float _baseSpeed = 5f;
	[SerializeField]
	private Transform _transform;
	[SerializeField]
	private float _staminaGaugeMax = 10f;
	[SerializeField]
	private float _staminaGauge = 10f;

	public Transform MyTransform {
		get { return _transform; }
		set { _transform = value; }
	}
	private Vector3 _direction;

	[SerializeField]
	private float _runSpeed = 2f;
	
	private int isRunning = 0;
	// Use this for initialization
	void Start () {
		Debug.Log("start movement script");
		_staminaGauge = _staminaGaugeMax;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		check_deplacement();
	}

	void check_deplacement(){
		//deplacements
		_direction = 
			Vector3.forward * Input.GetAxis("RunScript") +
				Vector3.right * Input.GetAxis("RunScript2");
		
		_transform.position += 
			_transform.rotation * _direction.normalized * 
				_walkSpeed * Time.deltaTime;
		
		//Courrir avec shift augmente la vitesse de _runSpeed
		if ( Input.GetKeyDown(KeyCode.LeftShift) && _staminaGauge > 0){
			_walkSpeed += _runSpeed;
			isRunning = 1;
		}
		//Lacher le bouton de course fait revenir le joueur a sa vitesse de base
		else if ( Input.GetKeyUp(KeyCode.LeftShift) && _staminaGauge < _staminaGaugeMax){
			_walkSpeed = _baseSpeed;
			isRunning = 0;
		}
		//Si le joueur essaie de courrir alors que son stamina est vide, son stamina continuera de diminuer
		else if(isRunning == 1 && _staminaGauge <= 0){
			_staminaGauge -= Time.deltaTime;
		}
		//Si le stamina du joueur est vide, il est alors fatigué, et marchera plus lentement le temps de se reposer
		if(_staminaGauge < 0){
			_walkSpeed = _baseSpeed-(_runSpeed/2);
		}
		//Si son stamina remonte et qu'il est reposé, sa vitesse revient a la normale
		if(_staminaGauge >=0 && isRunning == 0 && !Input.GetKeyUp(KeyCode.LeftShift)){
			_walkSpeed = _baseSpeed;
		}
		
		//Si le joueur arrete de courrir, alors son stamina remontera
		if(_staminaGauge >=0 && isRunning == 1){
			_staminaGauge -= Time.deltaTime;
		}
		
		//Si le joueur ne court pas, et que son stamina n'est pas au maximum, il remonte
		else if(_staminaGauge <= _staminaGaugeMax && isRunning ==0){
			_staminaGauge += Time.deltaTime;
			
		}
	}
	
	public void setTransform(Transform _player){
		this._transform = _player;
	}

}
