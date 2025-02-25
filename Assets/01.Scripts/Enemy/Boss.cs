using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Boss : Enemy
{
    public Transform missilePortA;
    public Transform missilePortB;

    private Vector3 lookVec;
    private Vector3 tauntVec;
    public bool isLook;



    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        bc = GetComponent<BoxCollider>();
        meshs = GetComponentsInChildren<MeshRenderer>();
        agent = GetComponent<NavMeshAgent>();
        childAnimator = GetComponentInChildren<Animator>();

        stateMachine = new StateMachine();

        StartCoroutine(Think());
    }


    void Update()
    {
        if (isLook)
        {
            float h = Input.GetAxisRaw("Horizontal");
            float v = Input.GetAxisRaw("Vertical");
            lookVec = new Vector3(h, 0, v) * 5f;
            transform.LookAt(target.position + lookVec);
        }
    }

    IEnumerator Think()
    {
        yield return new WaitForSeconds(0.1f);

        int ranAction = Random.Range(0, 5);
        switch (ranAction)
        {
            case 0:                
            case 1:
                StartCoroutine(MissileShot());
                break;

            case 2:
            case 3:
                StartCoroutine(RockShot());
                break;

            case 4:

                StartCoroutine(Taunt());
                break;
        }
    }

    IEnumerator MissileShot()
    {
        yield return new WaitForSeconds(2.5f);
        stateMachine.SetState(new BossMissileShot(stateMachine,childAnimator,this));



        StartCoroutine(Think());
    }

    IEnumerator RockShot()
    {

        yield return new WaitForSeconds(3f);
        StartCoroutine(Think());
    }

    IEnumerator Taunt()
    {
        yield return new WaitForSeconds(3f);
        StartCoroutine(Think());
    }
}
