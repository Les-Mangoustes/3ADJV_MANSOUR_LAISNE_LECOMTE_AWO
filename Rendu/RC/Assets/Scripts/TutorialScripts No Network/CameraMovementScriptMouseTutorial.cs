﻿using UnityEngine;
using System.Collections;

public class CameraMovementScriptMouseTutorial : MonoBehaviour {
	[SerializeField]
	private float mDelta = 10; // Pixels. The width border at the edge in which the movement work
	
	[SerializeField]
	private float mSpeed = 3.0f; // Scale. Speed of the movement

    private Transform joueur;
	
	// Use this for initialization
	void Start () {
        joueur = transform.parent;
		//transform.parent = joueur;
	}
	
	// Update is called once per frame
	void Update () {
		// Check if on the right edge
		if ( Input.mousePosition.x >= Screen.width - mDelta ){
			transform.position += Vector3.right * Time.deltaTime * mSpeed;
			
		}
		if ( Input.mousePosition.x <= 0 + mDelta ){
			transform.position += Vector3.left * Time.deltaTime * mSpeed;
			
			if(transform.parent == joueur){
				transform.parent = null;
			}
		}
		if ( Input.mousePosition.y >= Screen.height - mDelta ){
			transform.position += Vector3.forward * Time.deltaTime * mSpeed;
			
			if(transform.parent == joueur){
				transform.parent = null;
			}
		}
		if ( Input.mousePosition.y <= 0 + mDelta ){
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

