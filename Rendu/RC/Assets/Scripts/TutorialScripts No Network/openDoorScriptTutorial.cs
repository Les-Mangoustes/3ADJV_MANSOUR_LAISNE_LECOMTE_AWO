using UnityEngine;
using System.Collections;

public class openDoorScriptTutorial : MonoBehaviour {

    private float smooth = 2.0f;
    private float DoorOpenAngle = 90.0f;

    private Vector3 defaultRot ;
    private Vector3 openRot;

    private Transform trans;
    
    private bool isReallyOn = true;

    private bool open = false;

    void Start()
    {
        trans = gameObject.transform;
        defaultRot = trans.eulerAngles;
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
                if (trans.eulerAngles.y < 90.0f)
                    trans.eulerAngles = Vector3.Slerp(trans.eulerAngles, openRot, Time.deltaTime * smooth);
            }
            else if (trans.eulerAngles.y > 1.0f)//Vector3.Angle(trans.eulerAngles,defaultRot) > 1.0f)
            {
                trans.eulerAngles = Vector3.Slerp(trans.eulerAngles, defaultRot, Time.deltaTime * smooth);
            }

	}

    void OnTriggerExit(Collider other)
    {
        isReallyOn = false;
        if (!isReallyOn)
            open = false;
    }
}
