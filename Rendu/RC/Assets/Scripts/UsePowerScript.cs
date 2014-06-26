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
                    case 4:
                        {
                            inv.removeItemAt(currentItemIndex);

                            Camera _camera = (Camera)GameObject.Find("MainCamera").transform.camera;

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
                    case 5:
                        {
                            if (Input.GetMouseButtonDown(1))
                            {
                                Camera _camera = (Camera)GameObject.Find("MainCamera").transform.camera;
                                // Le rayon
                                var ray = _camera.ScreenPointToRay(Input.mousePosition);
                                // Le point d'impact
                                RaycastHit hit;

                                // rayon, point d'impact, longeur max du rayon, layers sur lesquels le rayon peut impacter
                                if (Physics.Raycast(ray, out hit, Mathf.Infinity, 1 << 8))
                                {
                                   

                                    /* mettre un slow sur le joueur touché */


                                }
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