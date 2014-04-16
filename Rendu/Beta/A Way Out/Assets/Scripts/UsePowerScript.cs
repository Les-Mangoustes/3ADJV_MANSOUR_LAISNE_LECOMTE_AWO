using UnityEngine;
using System.Collections;

public class UsePowerScript : MonoBehaviour
{

    [SerializeField]
    private Transform usable;
    // Use this for initialization
    /*void Start()
    {
        


    }*/

    // Update is called once per frame
    void Update()
    {
        if ((Network.isServer && gameObject.name == "Player") || (Network.isClient && gameObject.name == "Player2"))
        {

            if (Input.GetMouseButtonDown(1))
            {

                Inventory inv = gameObject.GetComponent<Inventory>();
                int currentItemId = inv.getCurrentItemId();
                int currentItemIndex = inv.getCurrentItemIndex();



                switch (currentItemId)
                {

                    //Piège à loup
                    case 1: 
                    case 2: 
                    case 3:
                        {

                            inv.removeItemAt(currentItemIndex);

                            Camera _camera;
                            if (Network.isServer)
                            {
                                Transform trans = gameObject.transform.Find("CameraPlayer1");
                                if (trans == null)
                                    _camera = GameObject.Find("CameraPlayer1").gameObject.camera;
                                else
                                    _camera = trans.gameObject.camera;
                            }
                            else
                            {
                                Transform trans = gameObject.transform.Find("CameraPlayer2");
                                if (trans == null)
                                    _camera = GameObject.Find("CameraPlayer2").gameObject.camera;
                                else
                                    _camera = trans.gameObject.camera;

                            }


                            var ray = _camera.ScreenPointToRay(Input.mousePosition);
                            // Le point d'impact
                            RaycastHit hit;

                            // rayon, point d'impact, longeur max du rayon, layers sur lesquels le rayon peut impacter
                            if (Physics.Raycast(ray, out hit, 20))
                            {
                                //Vector3 position = _camera.ScreenPointToRay.ScreenToViewportPoint(Input.mousePosition);//_camera.ScreenToWorldPoint()
                                Transform trans = (Transform)Network.Instantiate(usable, hit.point, Quaternion.identity, 0);
                                trans.GetComponent<PowerProperties>().setIdPower(currentItemId);
                            }

                        }


                        break;
                    
                    default: ;
                        break;
                }
            }
        }
    }
}