using UnityEngine;
using System.Collections;

public class InteractScript : MonoBehaviour {

    [SerializeField]
    private Transform _slender;

    [SerializeField]
    private Transform _player1;

    [SerializeField]
    private Animator p1_animator;

    [SerializeField]
    private Animator p2_animator;

    [SerializeField]
    private Transform _player1_model;

    [SerializeField]
    private Transform _player2_model;

    [SerializeField]
    private Transform _player2;

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
    //private bool _setTargetOnPlayer1 = false;

    void Start()
    {
        _player1_controller = _player1.GetComponent<CharacterController>();
        _player2_controller = _player2.GetComponent<CharacterController>();
    }

    void Update()
    {
        attack();
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
        check_deplacement();//walkSpeed
        animation_deplacement();
    }


    void animation_deplacement()
    {
        bool serveur = Network.isServer;
        p1_animator.SetBool("Walking", false);
        p2_animator.SetBool("Walking", false);

        //Animations movement
        if (serveur && Input.GetKey(KeyCode.Q) && !Input.GetKey(KeyCode.S) && !Input.GetKey(KeyCode.Z))
        {
            p1_animator.SetBool("Walking", true);
            _player1_model.rotation = Quaternion.Euler(0, 270, 0);
        }
        if (serveur && Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.S) && !Input.GetKey(KeyCode.Z))
        {
            p1_animator.SetBool("Walking", true);
            _player1_model.rotation = Quaternion.Euler(0, 90, 0);
        }
        if (serveur && Input.GetKey(KeyCode.Z) && !Input.GetKey(KeyCode.Q) && !Input.GetKey(KeyCode.D))
        {
            p1_animator.SetBool("Walking", true);
            _player1_model.rotation = Quaternion.Euler(0, 0, 0);
        }
        if (serveur && Input.GetKey(KeyCode.S) && !Input.GetKey(KeyCode.Q) && !Input.GetKey(KeyCode.D))
        {
            p1_animator.SetBool("Walking", true);
            _player1_model.rotation = Quaternion.Euler(0, 180, 0);
        }
        // Animations movement diagonals 
        if (serveur && Input.GetKey(KeyCode.Q) && Input.GetKey(KeyCode.Z))
        {
            p1_animator.SetBool("Walking", true);
            _player1_model.rotation = Quaternion.Euler(0, 315, 0);
        }
        if (serveur && Input.GetKey(KeyCode.Z) && Input.GetKey(KeyCode.D))
        {
            p1_animator.SetBool("Walking", true);
            _player1_model.rotation = Quaternion.Euler(0, 45, 0);
        }
        if (serveur && Input.GetKey(KeyCode.D) && Input.GetKey(KeyCode.S))
        {
            p1_animator.SetBool("Walking", true);
            _player1_model.rotation = Quaternion.Euler(0, 135, 0);
        }
        if (serveur && Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.Q))
        {
            p1_animator.SetBool("Walking", true);
            _player1_model.rotation = Quaternion.Euler(0, 225, 0);
        }

        if (_clientWantToMoveDown && !(_clientWantToMoveLeft) && !(_clientWantToMoveRight))
        {
            _player2_model.rotation = Quaternion.Euler(0, 180, 0);
            p2_animator.SetBool("Walking", true);
        }
        if (_clientWantToMoveUp && !_clientWantToMoveLeft && !_clientWantToMoveRight)
        {
            _player2_model.rotation = Quaternion.Euler(0, 0, 0);
            p2_animator.SetBool("Walking", true);
        }
        if (_clientWantToMoveRight && !_clientWantToMoveUp && !_clientWantToMoveDown)
        {
            _player2_model.rotation = Quaternion.Euler(0, 90, 0);
            p2_animator.SetBool("Walking", true);
        }
        if (_clientWantToMoveLeft && !_clientWantToMoveUp && !_clientWantToMoveDown)
        {
            _player2_model.rotation = Quaternion.Euler(0, 270, 0);
            p2_animator.SetBool("Walking", true);
        }
        //animation client diagonals
        if (_clientWantToMoveLeft && _clientWantToMoveUp)
        {
            _player2_model.rotation = Quaternion.Euler(0, 315, 0);
            p2_animator.SetBool("Walking", true);
        }
        if (_clientWantToMoveUp && _clientWantToMoveRight)
        {
            _player2_model.rotation = Quaternion.Euler(0, 45, 0);
            p2_animator.SetBool("Walking", true);
        }
        if (_clientWantToMoveRight && _clientWantToMoveDown)
        {
            _player2_model.rotation = Quaternion.Euler(0, 135, 0);
            p2_animator.SetBool("Walking", true);
        }
        if (_clientWantToMoveDown && _clientWantToMoveLeft)
        {
            _player2_model.rotation = Quaternion.Euler(0, 225, 0);
            p2_animator.SetBool("Walking", true);
        }
    }

    void check_deplacement()
    {
        float walkSpeed_p1;
        float walkSpeed_p2;
        bool serveur = Network.isServer;
        if (serveur)
        {
            networkView.RPC("ServerMoveDown", RPCMode.Others, false);
            networkView.RPC("ServerMoveUp", RPCMode.Others, false);
            networkView.RPC("ServerMoveLeft", RPCMode.Others, false);
            networkView.RPC("ServerMoveRight", RPCMode.Others, false);
        }
        walkSpeed_p1 = _player1.GetComponent<PlayerCaracScript>().getSpeed();
        walkSpeed_p2 = _player2.GetComponent<PlayerCaracScript>().getSpeed();

        if ((serveur && Input.GetKey(KeyCode.S)) || _serverWantToMoveDown)
        {
            _player1_controller.Move( -Vector3.forward *
                walkSpeed_p1 * Time.deltaTime);
            if (serveur)
                networkView.RPC("ServerMoveDown", RPCMode.Others, true);
        }
        if (serveur && Input.GetKey(KeyCode.Z))
        {
            _player1_controller.Move( Vector3.forward *
                walkSpeed_p1 * Time.deltaTime);
            if (serveur)
                networkView.RPC("ServerMoveUp", RPCMode.Others, true);
        }
        if (serveur && Input.GetKey(KeyCode.D))
        {
            _player1_controller.Move( Vector3.right *
                walkSpeed_p1 * Time.deltaTime);
            if (serveur)
                networkView.RPC("ServerMoveRight", RPCMode.Others, true);
        }
        if (serveur && Input.GetKey(KeyCode.Q))
        {
            _player1_controller.Move( Vector3.left *
                walkSpeed_p1 * Time.deltaTime);
            if (serveur)
                networkView.RPC("ServerMoveLeft", RPCMode.Others, true);
        }

        if (_clientWantToMoveDown)
        {
            _player2_controller.Move( -Vector3.forward *
                walkSpeed_p2 * Time.deltaTime);
        }
        if (_clientWantToMoveUp)
        {
            _player2_controller.Move( Vector3.forward *
                walkSpeed_p2 * Time.deltaTime);
        }
        if (_clientWantToMoveRight)
        {
            _player2_controller.Move( Vector3.right *
                walkSpeed_p2 * Time.deltaTime);
        }
        if (_clientWantToMoveLeft)
        {
            _player2_controller.Move( Vector3.left *
                walkSpeed_p2 * Time.deltaTime);
        }

        if (_serverWantToMoveDown)
        {
            _player1_controller.Move(-Vector3.forward *
                walkSpeed_p1 * Time.deltaTime);
        }
        if (_serverWantToMoveUp)
        {
            _player1_controller.Move(Vector3.forward *
                walkSpeed_p1 * Time.deltaTime);
        }
        if (_serverWantToMoveRight)
        {
            _player1_controller.Move(Vector3.right *
                walkSpeed_p1 * Time.deltaTime);
        }
        if (_serverWantToMoveLeft)
        {
            _player1_controller.Move(Vector3.left *
                walkSpeed_p1 * Time.deltaTime);
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
            Camera _camera;
            Transform trans = _player1.Find("CameraPlayer1");
            if (trans == null)
                _camera = GameObject.Find("CameraPlayer1").gameObject.camera;
            else
                _camera = trans.gameObject.camera;
            if (Input.GetMouseButtonDown(0))
            {
                //Debug.Log("clic for attack");
                // Le rayon
                var ray = _camera.ScreenPointToRay(Input.mousePosition);
                // Le point d'impact
                RaycastHit hit;

                // rayon, point d'impact, longeur max du rayon, layers sur lesquels le rayon peut impacter
                if (Physics.Raycast(ray, out hit, Mathf.Infinity, 1 << 8))
                {
                    // hit.point = position du point d'impact
                    if (hit.transform != _player1.transform)
                        changeSlenderTarget(false);
                    if(hit.transform.name.Contains("minions"))
                    {
                        minionsClass minions = hit.transform.gameObject.GetComponent<SpwanMinionsScript>().getMinions(hit.transform.name);
                        Network.Destroy(hit.transform.gameObject);
                        _player1.GetComponent<PlayerCaracScript>().setTemporaryAlteration(minions.buffValue, minions.buffTime, false);
                    }

                }
            }
        }
        else
        {
            Transform _transformHit;
            Camera _camera;
            Transform trans = _player2.Find("CameraPlayer2");
            if (trans == null)
                _camera = GameObject.Find("CameraPlayer2").gameObject.camera;
            else
                _camera = trans.gameObject.camera;
            if (Input.GetMouseButtonDown(0))
            {

                // Le rayon
                var ray = _camera.ScreenPointToRay(Input.mousePosition);
                // Le point d'impact
                RaycastHit hit;

                // rayon, point d'impact, longeur max du rayon, layers sur lesquels le rayon peut impacter
                if (Physics.Raycast(ray, out hit, Mathf.Infinity, 1 << 8))
                {
                    // hit.point = position du point d'impact
                    _transformHit = hit.transform;
                    if (_transformHit != _player2.transform)
                        networkView.RPC("changeSlenderTarget", RPCMode.Server,true);
                    if (_transformHit.name.Contains("Minions"))
                    {
                        minionsClass minions = hit.transform.gameObject.GetComponent<SpwanMinionsScript>().getMinions(hit.transform.name);
                        Network.Destroy(hit.transform.gameObject);
                        _player2.GetComponent<PlayerCaracScript>().setTemporaryAlteration(minions.buffValue, minions.buffTime, false);
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
    }
}
