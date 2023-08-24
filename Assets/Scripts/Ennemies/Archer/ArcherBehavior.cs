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

    public int attackCounter;

    bool canAttack;
    public int attackChance;

    bool randomAttackTicker;

    [Header("Animations")]
    public Animator archerAnimController;
    public int animIndex;

    bool _hasAnimator;

    [Header("VFX")]
    public GameObject vfxSender;
    public List<GameObject> vfxList = new List<GameObject>();

    private GameObject instantiatedVFX = null;
    private GameObject arrowVolley = null;
    bool arrowVolleyFolllow = false;
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
        

        Vector3 dir = (playerTransform.position - transform.position).normalized;

        //dir = Quaternion.AngleAxis(45, Vector3.up) * dir;

        Move(transform.position - (dir * runAwayDistance) /*playerTransform.position*/);

        if (instantiatedVFX != null)
        {
            instantiatedVFX.transform.position = vfxSender.transform.position;
            instantiatedVFX.transform.rotation = vfxSender.transform.rotation;
        }

        if(arrowVolley != null && arrowVolleyFolllow)
        {
            arrowVolley.transform.position = playerTransform.position;
        }

        if (!isAttacking)
        {
            archerAnimController.SetBool("ArcherDraw", false);
            archerAnimController.SetBool("ArcherShoot", false);
            archerAnimController.SetBool("ArcherDrawVolley", false);
            archerAnimController.SetBool("ArcherShootVolley", false);
        }

        if (canAttack)
        {
            attackChance = Random.Range(0, 10);
        }

    }

    public void Move(Vector3 pos)
    {
        float maxDistance = Vector3.Distance(playerTransform.position, transform.position);

        randomAttackTicker = true;

        if (randomAttackTicker && !isAttacking)
        {
            attackCounter = Random.Range(0, 100);

        }

        if (!isAttacking && maxDistance <= 10)
        {
            agent.SetDestination(pos);
            agent.isStopped = false;
            archerAnimController.SetBool("ArcherRun", true);
            archerAnimController.SetBool("ArcherDraw", false);
            archerAnimController.SetBool("ArcherShoot", false);
            archerAnimController.SetBool("ArcherDrawVolley", false);
            archerAnimController.SetBool("ArcherShootVolley", false);


        }
        else if(isAttacking)
        {
            //randomAttackTicker = false;
            Attack(); 
        }

        if (!isAttacking && maxDistance >= 15)
        {
            archerAnimController.SetBool("ArcherRun", false);
            archerAnimController.SetBool("ArcherDraw", false);
            archerAnimController.SetBool("ArcherShoot", false);
            archerAnimController.SetBool("ArcherDrawVolley", false);
            archerAnimController.SetBool("ArcherShootVolley", false);
            transform.LookAt(playerTransform);
            canAttack = true;

            if(attackChance >= 5)
            {
                StartCoroutine(WaitBeforeAttack());
                isAttacking = true;
                canAttack = false;
            }
        }

    }

    public void Attack()
    {

        
        agent.isStopped = true;
        archerAnimController.SetBool("ArcherRun", false);

        if(attackCounter <= 70)
        {
            archerAnimController.SetBool("ArcherDraw", true);
        }
        else if(attackCounter >= 71)
        {
            archerAnimController.SetBool("ArcherDrawVolley", true);
        }
        
        transform.LookAt(playerTransform);
        

    }



    //Animation Events

    public void ArrowShotBuildUp()
    {
        instantiatedVFX = Instantiate(vfxList[0].gameObject, vfxSender.transform.position, vfxSender.transform.rotation);

        if(attackCounter <= 70)
        {
            StartCoroutine(ArrowShotDelay());
        }
        else if(attackCounter >= 71)
        {
            arrowVolley = Instantiate(vfxList[3].gameObject, playerTransform.position, playerTransform.rotation);
            arrowVolleyFolllow = true;
            StartCoroutine(ArrowVolleyDelay());
        }
        
    }

    public void ArrowShotRelease()
    {
        
        GameObject temp = Instantiate(vfxList[1].gameObject, vfxSender.transform.position, vfxSender.transform.rotation);
        archerAnimController.SetBool("ArcherDraw", false);
        isAttacking = false;
    }

    public void ArrowVolleyRelease()
    {
        GameObject temp = Instantiate(vfxList[2].gameObject, vfxSender.transform.position, vfxSender.transform.rotation);
        archerAnimController.SetBool("ArcherDrawVolley", false);
        isAttacking = false;
    }

    public IEnumerator ArrowShotDelay()
    {
        yield return new WaitForSeconds(2f);
        archerAnimController.SetBool("ArcherShoot", true);
        
    }

    public IEnumerator ArrowVolleyDelay()
    {
        yield return new WaitForSeconds(2f);
        StartCoroutine(ArrowVolleyFollow());
        archerAnimController.SetBool("ArcherShootVolley", true);
    }

    private IEnumerator ArrowVolleyFollow()
    {
        yield return new WaitForSeconds(0.7f);
        arrowVolleyFolllow = false;
    }

    public IEnumerator WaitBeforeAttack()
    {
        yield return new WaitForSeconds(3f);
        canAttack = true;
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
