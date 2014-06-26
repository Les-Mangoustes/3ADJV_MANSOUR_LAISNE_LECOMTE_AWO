using UnityEngine;
using System.Collections;

public class InteractScript : MonoBehaviour {

    [SerializeField]
    private GameObject _slender;

    [SerializeField]
    private Transform _player1;

    [SerializeField]
    private Transform _player2;

    [SerializeField]
    private Animator p1_animator;

    [SerializeField]
    private Animator p2_animator;

    [SerializeField]
    private Transform _player1_model;

    [SerializeField]
    private Transform _player2_model;

    private bool _clientWantToMoveDown = false;
    private bool _clientWantToMoveUp = false;
    private bool _clientWantToMoveLeft = false;
    private bool _clientWantToMoveRight = false;

    private bool _serverWantToMoveDown = false;
    private bool _serverWantToMoveUp = false;
    private bool _serverWantToMoveLeft = false;
    private bool _serverWantToMoveRight = false;

    private CharacterController _player1_controller;
    private CharacterController _player2_controller;

    private StaticVariableScript settings;

    private float couldown_ref = 3.0f;
    private float couldown =0;

    void Start()
    {
        _player1_controller = _player1.GetComponent<CharacterController>();
        _player2_controller = _player2.GetComponent<CharacterController>();
        settings = gameObject.GetComponent<StaticVariableScript>();
        couldown = couldown_ref;
    }

    void Update()
    {
        if (couldown <= 0)
        {
            attack();
        }
        else
        {
            couldown -= Time.deltaTime;
        }
        if (Network.isClient)
        {
            if (Input.GetKeyDown(KeyCode.S))
            {
                networkView.RPC("ClientMoveDown", RPCMode.Server,true);
            }
            if (Input.GetKeyDown(KeyCode.Z))
            {
                networkView.RPC("ClientMoveUp", RPCMode.Server,true);
            }
            if (Input.GetKeyUp(KeyCode.S))
            {
                networkView.RPC("ClientMoveDown", RPCMode.Server,false);
            }
            if (Input.GetKeyUp(KeyCode.Z))
            {
                networkView.RPC("ClientMoveUp", RPCMode.Server,false);
            }
            if (Input.GetKeyDown(KeyCode.D))
            {
                networkView.RPC("ClientMoveRight", RPCMode.Server,true);
            }
            if (Input.GetKeyDown(KeyCode.Q))
            {
                networkView.RPC("ClientMoveLeft", RPCMode.Server,true);
            }
            if (Input.GetKeyUp(KeyCode.D))
            {
                networkView.RPC("ClientMoveRight", RPCMode.Server,false);
            }
            if (Input.GetKeyUp(KeyCode.Q))
            {
                networkView.RPC("ClientMoveLeft", RPCMode.Server,false);
            }
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //check_deplacement();//walkSpeed
        animation_deplacement();
    }


    void animation_deplacement()
    {
        bool serveur = Network.isServer;
        float walkSpeed_p1;
        float walkSpeed_p2;
        if (serveur)
        {
            networkView.RPC("ServerMoveDown", RPCMode.Others, false);
            networkView.RPC("ServerMoveUp", RPCMode.Others, false);
            networkView.RPC("ServerMoveLeft", RPCMode.Others, false);
            networkView.RPC("ServerMoveRight", RPCMode.Others, false);
        }
        walkSpeed_p1 = _player1.GetComponent<PlayerCaracScript>().getSpeedWithAlt();
        walkSpeed_p2 = _player2.GetComponent<PlayerCaracScript>().getSpeedWithAlt();
        p1_animator.SetBool("Walking", false);
        p2_animator.SetBool("Walking", false);

        //Animations movement
        //server : Only left
        if ((serveur && Input.GetKey(KeyCode.Q) && !Input.GetKey(KeyCode.S) && !Input.GetKey(KeyCode.Z) )|| (_serverWantToMoveLeft && !_serverWantToMoveUp && !_serverWantToMoveDown))
        {
            _player1_controller.Move( Vector3.left *
                walkSpeed_p1 * Time.deltaTime);
            if (serveur)
                networkView.RPC("ServerMoveLeft", RPCMode.Others, true);
            p1_animator.SetBool("Walking", true);
            _player1_model.rotation = Quaternion.Euler(0, 270, 0);
        }
        //server : Only Right
        if ((serveur && Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.S) && !Input.GetKey(KeyCode.Z)) || (_serverWantToMoveRight && !_serverWantToMoveUp && !_serverWantToMoveDown))
        {
            _player1_controller.Move( Vector3.right *
                walkSpeed_p1 * Time.deltaTime);
            if (serveur)
                networkView.RPC("ServerMoveRight", RPCMode.Others, true);
            p1_animator.SetBool("Walking", true);
            _player1_model.rotation = Quaternion.Euler(0, 90, 0);
        }
        //serveur Only Up
        if ((serveur && Input.GetKey(KeyCode.Z) && !Input.GetKey(KeyCode.Q) && !Input.GetKey(KeyCode.D)) || (_serverWantToMoveUp && !_serverWantToMoveLeft && !_serverWantToMoveRight))
        {
            _player1_controller.Move( Vector3.forward *
                    walkSpeed_p1 * Time.deltaTime);
            if (serveur)
                networkView.RPC("ServerMoveUp", RPCMode.Others, true);
            p1_animator.SetBool("Walking", true);
            _player1_model.rotation = Quaternion.Euler(0, 0, 0);
        }
        //server Only down
        if ((serveur && Input.GetKey(KeyCode.S) && !Input.GetKey(KeyCode.Q) && !Input.GetKey(KeyCode.D)) || (_serverWantToMoveDown && !(_serverWantToMoveLeft) && !(_serverWantToMoveRight)))
        {
            _player1_controller.Move( -Vector3.forward *
                    walkSpeed_p1 * Time.deltaTime);
            if (serveur)
                networkView.RPC("ServerMoveDown", RPCMode.Others, true);
            p1_animator.SetBool("Walking", true);
            _player1_model.rotation = Quaternion.Euler(0, 180, 0);
        }
        // Animations movement diagonals 
        //server : left Up
        if ((serveur && Input.GetKey(KeyCode.Q) && Input.GetKey(KeyCode.Z))|| (_serverWantToMoveLeft && _serverWantToMoveUp))
        {
            Vector3 direction = new Vector3((-Mathf.Sqrt(2))/2,0,Mathf.Sqrt(2)/2);
            _player1_controller.Move(  direction *
                walkSpeed_p1 * Time.deltaTime);
            if (serveur)
            {
                networkView.RPC("ServerMoveLeft", RPCMode.Others, true);
                networkView.RPC("ServerMoveUp", RPCMode.Others, true);
            }

            p1_animator.SetBool("Walking", true);
            _player1_model.rotation = Quaternion.Euler(0, 315, 0);
        }
        //server : right up
        if ((serveur && Input.GetKey(KeyCode.Z) && Input.GetKey(KeyCode.D) ) || (_serverWantToMoveUp && _serverWantToMoveRight))
        {
            Vector3 direction = new Vector3(Mathf.Sqrt(2)/2,0,Mathf.Sqrt(2)/2);
            _player1_controller.Move(  direction *
                walkSpeed_p1 * Time.deltaTime);
            if (serveur)
            {
                networkView.RPC("ServerMoveRight", RPCMode.Others, true);
                networkView.RPC("ServerMoveUp", RPCMode.Others, true);
            }
            p1_animator.SetBool("Walking", true);
            _player1_model.rotation = Quaternion.Euler(0, 45, 0);
        }
        //server : right down
        if ((serveur && Input.GetKey(KeyCode.D) && Input.GetKey(KeyCode.S)) || (_serverWantToMoveDown && _serverWantToMoveRight))
        {
            Vector3 direction = new Vector3(Mathf.Sqrt(2)/2,0,(-Mathf.Sqrt(2))/2);
            _player1_controller.Move(  direction *
                walkSpeed_p1 * Time.deltaTime);
            if (serveur)
            {
                networkView.RPC("ServerMoveRight", RPCMode.Others, true);
                networkView.RPC("ServerMoveDown", RPCMode.Others, true);
            }
            p1_animator.SetBool("Walking", true);
            _player1_model.rotation = Quaternion.Euler(0, 135, 0);
        }
        //server : left down
        if ((serveur && Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.Q)) || (_serverWantToMoveLeft && _serverWantToMoveDown))
        {
            Vector3 direction = new Vector3((-Mathf.Sqrt(2))/2,0,(-Mathf.Sqrt(2))/2);
            _player1_controller.Move(  direction *
                walkSpeed_p1 * Time.deltaTime);
            if (serveur)
            {
                networkView.RPC("ServerMoveLeft", RPCMode.Others, true);
                networkView.RPC("ServerMoveDown", RPCMode.Others, true);
            }
            p1_animator.SetBool("Walking", true);
            _player1_model.rotation = Quaternion.Euler(0, 225, 0);
        }

        //Client
        //only down
        if (_clientWantToMoveDown && !(_clientWantToMoveLeft) && !(_clientWantToMoveRight))
        {
            _player2_controller.Move( -Vector3.forward *
                walkSpeed_p2 * Time.deltaTime);
            _player2_model.rotation = Quaternion.Euler(0, 180, 0);
            p2_animator.SetBool("Walking", true);
        }
        //only up
        if (_clientWantToMoveUp && !_clientWantToMoveLeft && !_clientWantToMoveRight)
        {
            _player2_controller.Move( Vector3.forward *
                walkSpeed_p2 * Time.deltaTime);
            _player2_model.rotation = Quaternion.Euler(0, 0, 0);
            p2_animator.SetBool("Walking", true);
        }
        //only right
        if (_clientWantToMoveRight && !_clientWantToMoveUp && !_clientWantToMoveDown)
        {
            _player2_controller.Move( Vector3.right *
                walkSpeed_p2 * Time.deltaTime);
            _player2_model.rotation = Quaternion.Euler(0, 90, 0);
            p2_animator.SetBool("Walking", true);
        }
        //only left
        if (_clientWantToMoveLeft && !_clientWantToMoveUp && !_clientWantToMoveDown)
        {
            _player2_controller.Move( Vector3.left *
                walkSpeed_p2 * Time.deltaTime);
            _player2_model.rotation = Quaternion.Euler(0, 270, 0);
            p2_animator.SetBool("Walking", true);
        }
        //animation client diagonals
        //left up
        if (_clientWantToMoveLeft && _clientWantToMoveUp)
        {
            Vector3 direction = new Vector3((-Mathf.Sqrt(2))/2,0,Mathf.Sqrt(2)/2);
            _player2_controller.Move( direction *
                 walkSpeed_p2 * Time.deltaTime);
            _player2_model.rotation = Quaternion.Euler(0, 315, 0);
            p2_animator.SetBool("Walking", true);
        }
        //right up
        if (_clientWantToMoveUp && _clientWantToMoveRight)
        {
            Vector3 direction = new Vector3(Mathf.Sqrt(2)/2, 0, Mathf.Sqrt(2)/2);
            _player2_controller.Move( direction *
                walkSpeed_p2 * Time.deltaTime);
            _player2_model.rotation = Quaternion.Euler(0, 45, 0);
            p2_animator.SetBool("Walking", true);
        }
        //right down
        if (_clientWantToMoveRight && _clientWantToMoveDown)
        {
            Vector3 direction = new Vector3(Mathf.Sqrt(2)/2, 0, (-Mathf.Sqrt(2))/2);
            _player2_controller.Move( direction *
                walkSpeed_p2 * Time.deltaTime);
            _player2_model.rotation = Quaternion.Euler(0, 135, 0);
            p2_animator.SetBool("Walking", true);
        }
        //left down
        if (_clientWantToMoveDown && _clientWantToMoveLeft)
        {
            Vector3 direction = new Vector3((-Mathf.Sqrt(2))/2, 0, (-Mathf.Sqrt(2))/2);
            _player2_controller.Move( direction *
                walkSpeed_p2 * Time.deltaTime);
            _player2_model.rotation = Quaternion.Euler(0, 225, 0);
            p2_animator.SetBool("Walking", true);
        }
    }

    [RPC]
    public void ClientMoveUp(bool b)
    {
        _clientWantToMoveUp = b;
        if (Network.isServer)
            networkView.RPC("ClientMoveUp", RPCMode.Others,b);
    }

    [RPC]
    public void ClientMoveDown(bool b)
    {
        _clientWantToMoveDown = b;
        if (Network.isServer)
            networkView.RPC("ClientMoveDown", RPCMode.Others,b);
    }

    [RPC]
    public void ClientMoveRight(bool b)
    {
        _clientWantToMoveRight = b;
        if (Network.isServer)
            networkView.RPC("ClientMoveRight", RPCMode.Others,b);
    }

    [RPC]
    public void ClientMoveLeft(bool b)
    {
        _clientWantToMoveLeft = b;
        if (Network.isServer)
            networkView.RPC("ClientMoveLeft", RPCMode.Others,b);
    }

    [RPC]
    public void ServerMoveUp(bool b)
    {
        _serverWantToMoveUp = b;
        if (Network.isServer)
            networkView.RPC("ServerMoveUp", RPCMode.Others, b);
    }

    [RPC]
    public void ServerMoveDown(bool b)
    {
        _serverWantToMoveDown = b;
        if (Network.isServer)
            networkView.RPC("ServerMoveDown", RPCMode.Others, b);
    }

    [RPC]
    public void ServerMoveRight(bool b)
    {
        _serverWantToMoveRight = b;
        if (Network.isServer)
            networkView.RPC("ServerMoveRight", RPCMode.Others, b);
    }

    [RPC]
    public void ServerMoveLeft(bool b)
    {
        _serverWantToMoveLeft = b;
        if (Network.isServer)
            networkView.RPC("ServerMoveLeft", RPCMode.Others, b);
    }

    public void attack()
    {
        if (Network.isServer)
        {
            Camera _camera = (Camera)GameObject.Find("MainCamera").transform.camera;
            if (Input.GetMouseButtonDown(0))
            {
                var ray = _camera.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit, Mathf.Infinity))
                {
                    if (hit.transform.name.Contains("Minions"))
                    {
                        this.gameObject.GetComponent<SpwanMinionsScript>().destroyMinions(hit.transform.name, _player1.gameObject);
                        couldown = couldown_ref;
                    }
                    if (settings.GameMode != "escape")
                    {
                        if (hit.transform == _player2.transform)
                        {
                            networkView.RPC("changeSlenderTarget", RPCMode.Server, false);
                        }
                    }
                    else
                    {
                        if (hit.transform.tag == "cylinderWithKey")
                        {
                            this.GetComponent<GetKeyScript>().getKey();
                            NetworkView view = hit.transform.FindChild("Cylinder").networkView;
                            view.RPC("keyDropped", RPCMode.All, view.viewID);
                            Debug.Log(_slender.GetType());
                            GameObject slender = ((GameObject)Network.Instantiate(_slender, checkPosition.addRandomPos(), Quaternion.identity, 0));
                            slender.GetComponent<IAScript>().enabled = true;
                            slender.networkView.RPC("setTargetByPlayer", RPCMode.All, _player1.networkView.viewID);
                            couldown = couldown_ref;
                        }
                        if (hit.transform == _player2.transform)
                        {
                            this.GetComponent<GetKeyScript>().dropKey();
                            GameObject.FindWithTag("cylinder").GetComponent<SpawnKeyScript>().respawnKey(); ;
                            couldown = couldown_ref;
                        }
                        if (hit.transform.tag == "exit")
                        {
                            if (this.GetComponent<GetKeyScript>().haveThreeKey())
                            {
                                this.GetComponent<endEscapeScript>().haveWin();
                            }
                            else
                            {
                                this.GetComponent<notEnoughKeyScript>().enabled = true;
                            }
                        }
                    }
                }
            }
        }
        else
        {
            Transform _transformHit;
            Camera _camera = (Camera)GameObject.Find("MainCamera").transform.camera;
            if (Input.GetMouseButtonDown(0))
            {

                var ray = _camera.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit, Mathf.Infinity))
                {
                    Debug.Log("hit : " + hit.transform.name);
                    _transformHit = hit.transform;
                    if (_transformHit.name.Contains("Minions"))
                    {
                        this.gameObject.GetComponent<SpwanMinionsScript>().destroyMinions(hit.transform.name, _player2.gameObject);
                        couldown = couldown_ref;
                    }
                    if (settings.GameMode != "escape")
                    {
                        if (_transformHit == _player1.transform)
                        {
                            networkView.RPC("changeSlenderTarget", RPCMode.Server, true);
                        }
                    }
                    else
                    {
                        if (_transformHit.tag == "cylinderWithKey")
                        {
                            this.GetComponent<GetKeyScript>().getKey();
                            NetworkView view = _transformHit.FindChild("Cylinder").networkView;
                            view.RPC("keyDropped", RPCMode.All, view.viewID);
                            Debug.Log(_slender.GetType());
                            GameObject slender = ((GameObject)Network.Instantiate(_slender, checkPosition.addRandomPos(), Quaternion.identity, 0));
                            slender.GetComponent<IAScript>().enabled = true;
                            slender.networkView.RPC("setTargetByPlayer", RPCMode.All, _player2.networkView.viewID);
                            couldown = couldown_ref;
                        }
                        if (_transformHit == _player1.transform)
                        {
                            this.GetComponent<GetKeyScript>().dropKey();
                            NetworkView view = GameObject.FindWithTag("cylinder").networkView;
                            view.RPC("respawnKey", RPCMode.Server, null);
                            couldown = couldown_ref;
                        }
                        if (_transformHit.tag == "exit")
                        {
                            if (this.GetComponent<GetKeyScript>().haveThreeKey())
                            {
                                this.GetComponent<endEscapeScript>().haveWin();
                            }
                            else
                            {
                                this.GetComponent<notEnoughKeyScript>().enabled = true;
                            }
                        }
                    }
                }
            }
        }
    }

    //quand le joueur 2 attack le joueur 1 (execution cote serveur)
    public void setTargetOnPlayer1(bool _isTarget)
    {
        Light light = _player1.GetComponentInChildren<Light>();
        if (_isTarget){
            light.color = Color.red;
            light.intensity = 0.81f;
        }
        else
        {
            light.color = Color.white;
            light.intensity = 0.61f;
        }
    }

    //quand le joueur 1 attack (execution coté client)
    [RPC]
    public void setTargetOnPlayer2(bool _isTarget)
    {
        Light light = _player2.GetComponentInChildren<Light>();
        if (_isTarget)
        {
            light.color = Color.red;
            light.intensity = 0.81f;
        }
        else
        {
            light.color = Color.white;
            light.intensity = 0.61f;
        }
    }

    //execution coté serveur
    [RPC]
    public void changeSlenderTarget(bool _isFromClient)
    {
        IAScript ia = GameObject.Find(_slender.name + "(Clone)").GetComponent<IAScript>();
        if(_isFromClient){
            ia.setTarget(_player1);
            setTargetOnPlayer1(true);
            networkView.RPC("setTargetOnPlayer2", RPCMode.Others,false);
        }
        else
        {
            ia.setTarget(_player2);
            networkView.RPC("setTargetOnPlayer2", RPCMode.Others,true);
            setTargetOnPlayer1(false);
        }
        couldown = couldown_ref;
    }
}