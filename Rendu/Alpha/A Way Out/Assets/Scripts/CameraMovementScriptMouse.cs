using UnityEngine;
using System.Collections;

public class CameraMovementScriptMouse : MonoBehaviour {
	[SerializeField]
	private float mDelta = 10; // Pixels. The width border at the edge in which the movement work
	
	[SerializeField]
	private float mSpeed = 3.0f; // Scale. Speed of the movement
	
	[SerializeField]
	private Transform joueur;
	
	
	// Use this for initialization
	void Start () {
		transform.parent = joueur;
	}
	
	/*public void setTransform(Transform _player){
		this.joueur = _player;
		Debug.Log("Mouse instantiate network");
	}*/
	
	// Update is called once per frame
	void Update () {
		// Check if on the right edge
		if ( Input.mousePosition.x >= Screen.width - mDelta ){
			// Move the camera
			transform.position += Vector3.right * Time.deltaTime * mSpeed;
			
			/*if(transform.parent == joueur){
				transform.parent = null;
			}*/
		}
		if ( Input.mousePosition.x <= 0 + mDelta ){
			// Move the camera
			transform.position += Vector3.left * Time.deltaTime * mSpeed;
			
			if(transform.parent == joueur){
				transform.parent = null;
			}
		}
		if ( Input.mousePosition.y >= Screen.height - mDelta ){
			// Move the camera
			transform.position += Vector3.forward * Time.deltaTime * mSpeed;
			
			if(transform.parent == joueur){
				transform.parent = null;
			}
		}
		if ( Input.mousePosition.y <= 0 + mDelta ){
			// Move the camera
			transform.position -= Vector3.forward * Time.deltaTime * mSpeed;
			
			if(transform.parent == joueur){
				transform.parent = null;
			}
		}
		
		if ( Input.GetKeyDown(KeyCode.E)){
			transform.parent = joueur;
			float yPos = transform.position.y;
			transform.position = new Vector3(joueur.position.x, yPos, joueur.position.z);
			
		}
	}
}

