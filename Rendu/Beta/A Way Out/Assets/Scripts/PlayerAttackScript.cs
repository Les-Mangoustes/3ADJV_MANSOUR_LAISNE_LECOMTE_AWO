using UnityEngine;
using System.Collections;

public class PlayerAttackScript : MonoBehaviour {
	
	[SerializeField]
	private Camera _camera;

	// Update is called once per frame
	void Update () { 
		if (Input.GetMouseButtonDown(0)){

			// Le rayon
			var ray = _camera.ScreenPointToRay(Input.mousePosition);
			// Le point d'impact
			RaycastHit hit;
			
			// rayon, point d'impact, longeur max du rayon, layers sur lesquels le rayon peut impacter
			if (Physics.Raycast(ray, out hit, Mathf.Infinity, 1 << 8)){
				// hit.point = position du point d'impact
				GameObject.FindGameObjectWithTag("Target").transform.parent = hit.transform;
				//Put the target in the center of the player
				GameObject.FindGameObjectWithTag("Target").transform.localPosition = new Vector3(0,0,0);
			}
		} 
	}
}