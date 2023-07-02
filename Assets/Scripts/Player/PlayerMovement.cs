using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public Rigidbody rb;
    public float moveSpeed = 5f;

    public PlayerInputActions playerControls;

    Vector3 moveDirection = Vector3.zero;

    private InputAction move;
    private InputAction fire;


    private void Awake()
    {
        playerControls = new PlayerInputActions();
    }
    private void OnEnable()
    {
        rb = GetComponent<Rigidbody>();
        move = playerControls.Player.Move;
        move.Enable();


    }

    private void OnDisable()
    {
        move.Disable();


    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        moveDirection = move.ReadValue<Vector3>();


    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector3(moveDirection.x * moveSpeed,0f, moveDirection.z * moveSpeed);
    }
}
