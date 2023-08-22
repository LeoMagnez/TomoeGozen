using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

public class ArcherBehavior : MonoBehaviour
{
    [Header("Animations")]
    public Animator archerAnimController;
    public int animIndex;

    bool _hasAnimator;

    [Header("VFX")]
    public GameObject vfxSender;
    public List<GameObject> vfxList = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        animIndex = 0;
        _hasAnimator = TryGetComponent(out archerAnimController);
    }

    // Update is called once per frame
    void Update()
    {
        AnimationTests();
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
        GameObject temp = Instantiate(vfxList[0].gameObject, vfxSender.transform.position, vfxSender.transform.rotation);
    }

    public void ArrowShotRelease()
    {
        GameObject temp = Instantiate(vfxList[1].gameObject, vfxSender.transform.position, vfxSender.transform.rotation);
    }

}
