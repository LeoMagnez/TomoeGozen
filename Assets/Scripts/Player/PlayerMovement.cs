using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;
using UnityEngine.Rendering;
using UnityEngine.UI;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Mouvement")]
    [SerializeField]
    private InputActionReference movementControls;



    [SerializeField]
    private float playerSpeed = 15.0f;

    [SerializeField]
    private float rotationSpeed = 20f;

    [Header("Dash")]
    [SerializeField]
    private InputActionReference dashControls;
    [SerializeField]
    private float dashSpeed = 50.0f;

    [SerializeField]
    private int dashCounter;

    [SerializeField]
    private float resetDashCooldown = 0f;

    [SerializeField]
    private float dashCooldown = 0f;

    [SerializeField]
    private GameObject dashTrail;

    private float jumpHeight = 1.0f;

    private float gravityValue = -9.81f;

    private CharacterController controller;
    private Vector3 playerVelocity;
    private bool groundedPlayer;

    [Header("Attacks")]
    [SerializeField]
    private InputActionReference attackControls;

    [SerializeField]
    private float elapsedAttackTime = 0.5f;

    [SerializeField]
    private int attackCounter;

    private bool isAttacking;

    [SerializeField] private bool lockAnimation;


    [Header("Animations")]
    [SerializeField]
    private Animator playerAnimator;

    private bool _hasAnimator;

    [Header("VFX")]
    public List<Slash> slashes = new List<Slash>();



    private void OnEnable()
    {
        movementControls.action.Enable();
        attackControls.action.Enable();
        dashControls.action.Enable();
    }

    private void OnDisable()
    {
        movementControls.action.Disable();
        attackControls.action.Disable();
        dashControls.action.Disable();
    }

    private void Start()
    {
        lockAnimation = false;
        controller = gameObject.GetComponent<CharacterController>();
        _hasAnimator = TryGetComponent(out playerAnimator);
        attackCounter = 0;
        dashCounter = 0;
    }

    void Update()
    {
        _hasAnimator = TryGetComponent(out playerAnimator);
        Move();
        Attack();
        Dash();

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

    public void Dash()
    {
        if (dashControls.action.triggered && dashControls.action.ReadValue<float>() > 0 && dashCooldown <= 0f)
        {
            dashCounter += 1;
            
            playerAnimator.SetBool("isDashing", true);
        }

        if(dashCounter >= 1)
        {
            resetDashCooldown += Time.deltaTime;
        }

        if (dashCounter >= 2)
        {
            dashCooldown += Time.deltaTime;

            if (dashCooldown >= 2)
            {
                dashCounter = 0;
                dashCooldown = 0;
                resetDashCooldown = 0;
            }
        }

        if(resetDashCooldown >= 2f)
        {
            dashCounter = 0;
            resetDashCooldown = 0;
            dashCooldown = 0f;
        }
    }

    public void Attack()
    {


        if (attackControls.action.triggered && attackControls.action.ReadValue<float>() > 0)
        {
            Debug.Log(attackCounter);

            isAttacking = true;


            switch (attackCounter)
            {
                case 0:
                    if (!lockAnimation)
                    {
                        playerAnimator.SetBool("isAttacking", true);
                        //StartCoroutine(SlashAttack(0));
                        attackCounter += 1;
                        elapsedAttackTime = 0.5f;
                        
                    }

                    break;
                case 1:

                    if (!lockAnimation)
                    {
                        playerAnimator.SetBool("Attack2", true);
                        //StartCoroutine(SlashAttack(1));
                        attackCounter += 1;
                        elapsedAttackTime = 0.5f;
                    }
                    break;

                case 2:

                    if (!lockAnimation)
                    {
                        playerAnimator.SetBool("Attack3", true);
                        //StartCoroutine(SlashAttack(2));
                        //attackCounter += 1;
                        elapsedAttackTime = 0.4f;
                        attackCounter = 0;
                    }

                    break;

                default:
                    attackCounter = 0;
                    isAttacking = false;
                    lockAnimation = false;
                    break;

            }


        }
        /*else
        {
            //Permet à l'animation de se finir avant de transitionner
            if (playerAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1 && !playerAnimator.IsInTransition(0))
            {
                playerAnimator.SetBool("isAttacking", false);
                playerAnimator.SetBool("Attack2", false);
                playerAnimator.SetBool("Attack3", false);
            } 
                

            
            
        }*/

        if (isAttacking)
        {
            elapsedAttackTime -= Time.deltaTime;
        }

        if (elapsedAttackTime <= 0)
        {
            attackCounter = 0;
            isAttacking = false;
            elapsedAttackTime = 0.5f;
            playerAnimator.SetBool("isAttacking", false);
            lockAnimation = false;
            playerAnimator.SetBool("Attack2", false);
            playerAnimator.SetBool("Attack3", false);
        }

    }

    //ANIMATION EVENTS//

    public void LockAnim()
    {
        lockAnimation = true;
    }

    public void UnlockAnim()
    {
        lockAnimation = false;
    }

    public void LockSpeed()
    {
        playerSpeed = 0f;
        rotationSpeed = 0f;
    }

    public void UnlockSpeed()
    {
        playerSpeed = 15.0f;
        rotationSpeed = 20.0f;
    }

    public void Dashing()
    {

        playerSpeed = dashSpeed;
        dashTrail.GetComponent<TrailRenderer>().emitting = true;
        rotationSpeed = 0f;
        StartCoroutine(WaitForEndOfDash());
    }

    public IEnumerator WaitForEndOfDash()
    {
        yield return new WaitForSeconds(0.15f);
        playerAnimator.SetBool("isDashing", false);
        playerSpeed = 15.0f;
        rotationSpeed = 20.0f;
        dashTrail.GetComponent<TrailRenderer>().emitting = false;
    }

    public void Slash(int attackNumber)
    {
        switch (attackNumber)
        {
            case 0:
                StartCoroutine(SlashAttack(0));
                break;
            case 1:
                StartCoroutine(SlashAttack(1));
                break;
            case 2:
                StartCoroutine(SlashAttack(2));
                break;

        }
    }
    //VFX

    public IEnumerator SlashAttack(int slashobj)
    {
        slashes[slashobj].slashObject.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        slashes[slashobj].slashObject.SetActive(false);
    }

}

//VFX CLASS

[System.Serializable]
public class Slash
{
    public GameObject slashObject;
}

