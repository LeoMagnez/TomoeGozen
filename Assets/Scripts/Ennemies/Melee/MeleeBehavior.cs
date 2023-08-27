using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MeleeBehavior : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private Transform playerTransform;

    [Header("Attack")]
    bool canAttack;
    public bool isAttacking;
    public int attackCounter;
    public int attackTicker;
    public int attackChance;

    float attackTimer = 3f;

    [Header("Animations")]
    public Animator meleeAnimator;
    public int animIndex;
    bool _hasAnimator;

    [Header("VFX")]
    public GameObject vfxSender;
    public List<GameObject> vfxList = new List<GameObject>();
    private GameObject instantiatedVFX = null;

    // Start is called before the first frame update
    void Start()
    {
        if (agent == null)
        {
            if (!TryGetComponent(out agent))
            {
                Debug.Log(name + "needs NavMesh Agent");
            }
        }

        animIndex = 0;
        _hasAnimator = TryGetComponent(out meleeAnimator);
    }

    // Update is called once per frame
    void Update()
    {
        Move(playerTransform.position);

        transform.LookAt(playerTransform.position);

        if (instantiatedVFX != null)
        {
            instantiatedVFX.transform.position = vfxSender.transform.position;
            instantiatedVFX.transform.rotation = vfxSender.transform.rotation;
        }
    }

    public void Move(Vector3 pos)
    {
     
        float maxDistance = Vector3.Distance(playerTransform.position, transform.position);

        Debug.Log(attackTimer);

        if (!isAttacking && maxDistance > 3)
        {
            agent.SetDestination(pos);
            agent.isStopped = false;
            meleeAnimator.SetBool("MeleeRun", true);
            meleeAnimator.SetBool("MeleeAttack", false);

            attackTimer = Random.Range(1, 3);
        }

        if(!isAttacking && maxDistance <= 3)
        {           

            StartCoroutine(WaitForAttack());
        }
    }

    public IEnumerator WaitForAttack()
    {
        meleeAnimator.SetBool("MeleeRun", false);
        isAttacking = true;

        while(attackTimer > 0)
        {
            attackTimer -= Time.deltaTime;
            yield return null;

        }

        meleeAnimator.SetBool("MeleeAttack", true);

        yield return new WaitForSeconds(2f);
        isAttacking = false;
        meleeAnimator.SetBool("MeleeAttack", false);
        attackTimer = Random.Range(1, 3);
    }

    //ANIMATION EVENTS

    public void ClawAttack()
    {
        instantiatedVFX = Instantiate(vfxList[0].gameObject, vfxSender.transform.position, vfxSender.transform.rotation);
    }
}
