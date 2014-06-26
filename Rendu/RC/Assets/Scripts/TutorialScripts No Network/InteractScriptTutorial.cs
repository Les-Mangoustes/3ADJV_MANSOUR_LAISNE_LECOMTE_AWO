using UnityEngine;
using System.Collections;

public class InteractScriptTutorial : MonoBehaviour {

    [SerializeField]
    private Transform _slender;

    [SerializeField]
    private Transform _player1;

    [SerializeField]
    private Animator p1_animator;

    [SerializeField]
    private Transform _player1_model;

    private CharacterController _player1_controller;

    void Start()
    {
        _player1_controller = _player1.GetComponent<CharacterController>();
    }



    // Update is called once per frame
    void FixedUpdate()
    {
        //check_deplacement();//walkSpeed
        animation_deplacement();
    }


    void animation_deplacement()
    {
        float walkSpeed_p1;

        walkSpeed_p1 = _player1.GetComponent<PlayerCaracScript>().getSpeedWithAlt();
        p1_animator.SetBool("Walking", false);

        //Animations movement
        //server : Only left
        if ((Input.GetKey(KeyCode.Q) && !Input.GetKey(KeyCode.S) && !Input.GetKey(KeyCode.Z) ))
        {
            _player1_controller.Move( Vector3.left *
                walkSpeed_p1 * Time.deltaTime);
            p1_animator.SetBool("Walking", true);
            _player1_model.rotation = Quaternion.Euler(0, 270, 0);
        }
        //server : Only Right
        if ((Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.S) && !Input.GetKey(KeyCode.Z)))
        {
            _player1_controller.Move( Vector3.right *
                walkSpeed_p1 * Time.deltaTime);
            p1_animator.SetBool("Walking", true);
            _player1_model.rotation = Quaternion.Euler(0, 90, 0);
        }
        //serveur Only Up
        if ((Input.GetKey(KeyCode.Z) && !Input.GetKey(KeyCode.Q) && !Input.GetKey(KeyCode.D)))
        {
            _player1_controller.Move( Vector3.forward *
                    walkSpeed_p1 * Time.deltaTime);
            p1_animator.SetBool("Walking", true);
            _player1_model.rotation = Quaternion.Euler(0, 0, 0);
        }
        //server Only down
        if ((Input.GetKey(KeyCode.S) && !Input.GetKey(KeyCode.Q) && !Input.GetKey(KeyCode.D)))
        {
            _player1_controller.Move( -Vector3.forward *
                    walkSpeed_p1 * Time.deltaTime);
            
            p1_animator.SetBool("Walking", true);
            _player1_model.rotation = Quaternion.Euler(0, 180, 0);
        }
        // Animations movement diagonals 
        //server : left Up
        if ((Input.GetKey(KeyCode.Q) && Input.GetKey(KeyCode.Z)))
        {
            Vector3 direction = new Vector3((-Mathf.Sqrt(2))/2,0,Mathf.Sqrt(2)/2);
            _player1_controller.Move(  direction *
                walkSpeed_p1 * Time.deltaTime);
            p1_animator.SetBool("Walking", true);
            _player1_model.rotation = Quaternion.Euler(0, 315, 0);
        }
        //server : right up
        if ((Input.GetKey(KeyCode.Z) && Input.GetKey(KeyCode.D) ))
        {
            Vector3 direction = new Vector3(Mathf.Sqrt(2)/2,0,Mathf.Sqrt(2)/2);
            _player1_controller.Move(  direction *
                walkSpeed_p1 * Time.deltaTime);
            p1_animator.SetBool("Walking", true);
            _player1_model.rotation = Quaternion.Euler(0, 45, 0);
        }
        //server : right down
        if ((Input.GetKey(KeyCode.D) && Input.GetKey(KeyCode.S)))
        {
            Vector3 direction = new Vector3(Mathf.Sqrt(2)/2,0,(-Mathf.Sqrt(2))/2);
            _player1_controller.Move(  direction *
                walkSpeed_p1 * Time.deltaTime);
            p1_animator.SetBool("Walking", true);
            _player1_model.rotation = Quaternion.Euler(0, 135, 0);
        }
        //server : left down
        if ((Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.Q)))
        {
            Vector3 direction = new Vector3((-Mathf.Sqrt(2))/2,0,(-Mathf.Sqrt(2))/2);
            _player1_controller.Move(  direction *
                walkSpeed_p1 * Time.deltaTime);
            p1_animator.SetBool("Walking", true);
            _player1_model.rotation = Quaternion.Euler(0, 225, 0);
        }
    }
}