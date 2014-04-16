using UnityEngine;
using System.Collections;

public class openDoorScript : MonoBehaviour {

    private float smooth = 2.0f;
    private float DoorOpenAngle = 90.0f;

    private Vector3 defaultRot ;
    private Vector3 openRot;

    private bool isReallyOn = true;

    private bool open = false;

    void Start()
    {
        defaultRot = gameObject.transform.eulerAngles;
        openRot = new Vector3(defaultRot.x, defaultRot.y + DoorOpenAngle, defaultRot.z);
    }

	// Use this for initialization
    void OnTriggerEnter(Collider other)
    {
        isReallyOn = true;
        if(isReallyOn)
            open = true;
    }
	
	// Update is called once per frame
	void FixedUpdate () {
            if (open)
            {
                if (gameObject.transform.eulerAngles.y < 90.0f)
                    gameObject.transform.eulerAngles = Vector3.Slerp(gameObject.transform.eulerAngles, openRot, Time.deltaTime * smooth);
            }
            else if (Vector3.Angle(gameObject.transform.eulerAngles,defaultRot) > 1.0f)
            {
                gameObject.transform.eulerAngles = Vector3.Slerp(gameObject.transform.eulerAngles, defaultRot, Time.deltaTime * smooth);
            }

	}

    void OnTriggerExit(Collider other)
    {
        isReallyOn = false;
        if (!isReallyOn)
            open = false;
    }
}
