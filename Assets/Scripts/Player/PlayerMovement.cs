using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;
using UnityEngine.UI;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Mouvement")]
    [SerializeField]
    private InputActionReference movementControls;

    [SerializeField]
    private float playerSpeed = 2.0f;

    [SerializeField]
    private float rotationSpeed = 2f;
    [SerializeField]
    private float jumpHeight = 1.0f;
    [SerializeField]
    private float gravityValue = -9.81f;

    private CharacterController controller;
    private Vector3 playerVelocity;
    private bool groundedPlayer;

    [Header("Attacks")]
    [SerializeField]
    private InputActionReference attackControls;





    [Header("Animations")]
    [SerializeField]
    private Animator playerAnimator;

    private bool _hasAnimator;


    private void OnEnable()
    {
        movementControls.action.Enable();
        attackControls.action.Enable();
    }

    private void OnDisable()
    {
        movementControls.action.Disable();
        attackControls.action.Disable();
    }

    private void Start()
    {
        controller = gameObject.GetComponent<CharacterController>();
        _hasAnimator = TryGetComponent(out playerAnimator);
    }

    void Update()
    {
        _hasAnimator = TryGetComponent(out playerAnimator);
        Move();
        Attack();


    }

    public void Move()
    {
        groundedPlayer = controller.isGrounded;
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }

        Vector2 movement = movementControls.action.ReadValue<Vector2>();
        Vector3 move = new Vector3(movement.x, 0, movement.y);

        controller.Move(move * Time.deltaTime * playerSpeed);

        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);

        if (movement != Vector2.zero)
        {
            float targetAngle = Mathf.Atan2(movement.x, movement.y) * Mathf.Rad2Deg;
            Quaternion rotation = Quaternion.Euler(0f, targetAngle, 0f);
            transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * rotationSpeed);
            playerAnimator.SetBool("isRunning", true);
        }

        else
        {
            playerAnimator.SetBool("isRunning", false);
        }

    }

    public void Attack()
    {
        if (attackControls.action.triggered && attackControls.action.ReadValue<float>() > 0)
        {
            
                playerAnimator.SetBool("isAttacking", true);
        }
        else
        {
            //Permet à l'animation de se finir avant de transitionner
            if (playerAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1 && !playerAnimator.IsInTransition(0)) 
                playerAnimator.SetBool("isAttacking", false);
            
            
        }
        
    }
}

