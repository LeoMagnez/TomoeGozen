using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.AI;

public class ArcherBehavior : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float archerVelocity = 15.0f;
    [SerializeField] private NavMeshAgent agent = null;
    [SerializeField] private Transform playerTransform;

    private float runAwayDistance = 5f;

    [Header("Attack")]
    public float baseAttackDamage = 10;

    public bool isAttacking;

    [Header("Animations")]
    public Animator archerAnimController;
    public int animIndex;

    bool _hasAnimator;

    [Header("VFX")]
    public GameObject vfxSender;
    public List<GameObject> vfxList = new List<GameObject>();

    private GameObject instantiatedVFX = null;
    // Start is called before the first frame update
    void Start()
    {
        if(agent == null)
        {
            if(!TryGetComponent(out agent))
            {
                Debug.Log(name + "needs NavMesh Agent");
            }
        }
        animIndex = 0;
        _hasAnimator = TryGetComponent(out archerAnimController);
    }

    // Update is called once per frame
    void Update()
    {
        AnimationTests();
        

        Vector3 dir = (playerTransform.position - transform.position).normalized;

        //dir = Quaternion.AngleAxis(45, Vector3.up) * dir;

        Move(transform.position - (dir * runAwayDistance) /*playerTransform.position*/);

        if (instantiatedVFX != null)
        {
            instantiatedVFX.transform.position = vfxSender.transform.position;
            instantiatedVFX.transform.rotation = vfxSender.transform.rotation;
        }

    }

    public void Move(Vector3 pos)
    {
        float maxDistance = Vector3.Distance(playerTransform.position, transform.position);

        

        if (!isAttacking && maxDistance <= 10)
        {
            agent.SetDestination(pos);
            agent.isStopped = false;
            archerAnimController.SetBool("ArcherRun", true);
            archerAnimController.SetBool("ArcherDraw", false);
            archerAnimController.SetBool("ArcherShoot", false);
        }
        else if(isAttacking)
        {

            Attack(); 
        }

    }

    public void Attack()
    {
        agent.isStopped = true;
        archerAnimController.SetBool("ArcherRun", false);
        archerAnimController.SetBool("ArcherDraw", true);
        transform.LookAt(playerTransform);
        

    }

    public void AnimationTests()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            animIndex++;
            if(animIndex > 3)
            {
                animIndex = 0;
            }
        }

        switch (animIndex)
        {
            case 0:
                archerAnimController.SetBool("ArcherRun", false);
                archerAnimController.SetBool("ArcherDraw", false);
                archerAnimController.SetBool("ArcherShoot", false);
                break;

            case 1:
                archerAnimController.SetBool("ArcherRun", true);
                archerAnimController.SetBool("ArcherDraw", false);
                archerAnimController.SetBool("ArcherShoot", false);
                break;

            case 2:
                archerAnimController.SetBool("ArcherRun", false);
                archerAnimController.SetBool("ArcherDraw", true);
                archerAnimController.SetBool("ArcherShoot", false);
                break;

            case 3:
                archerAnimController.SetBool("ArcherRun", false);
                archerAnimController.SetBool("ArcherDraw", false);
                archerAnimController.SetBool("ArcherShoot", true);
                break;

            default:
                archerAnimController.SetBool("ArcherRun", false);
                archerAnimController.SetBool("ArcherDraw", false);
                archerAnimController.SetBool("ArcherShoot", false);
                break;
        }
    }



    //Animation Events

    public void ArrowShotBuildUp()
    {
        instantiatedVFX = Instantiate(vfxList[0].gameObject, vfxSender.transform.position, vfxSender.transform.rotation);
        StartCoroutine(ArrowShotDelay());
    }

    public void ArrowShotRelease()
    {
        
        GameObject temp = Instantiate(vfxList[1].gameObject, vfxSender.transform.position, vfxSender.transform.rotation);
        archerAnimController.SetBool("ArcherDraw", false);
        isAttacking = false;
    }

    public IEnumerator ArrowShotDelay()
    {
        yield return new WaitForSeconds(2f);
        archerAnimController.SetBool("ArcherShoot", true);
        
    }


#if UNITY_EDITOR

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        Vector3 direction = (playerTransform.position - transform.position).normalized;

        float magnitude = direction.magnitude;

        Gizmos.DrawLine(transform.position, transform.position + direction);
    }

#endif
}
