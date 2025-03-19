using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Boss : Enemy
{
    public Transform missilePortA;
    public Transform missilePortB;

    public Vector3 lookVec;
    public Vector3 tauntVec;
    public bool isLook;



    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        bc = GetComponent<BoxCollider>();
        meshs = GetComponentsInChildren<SkinnedMeshRenderer>();
        agent = GetComponent<NavMeshAgent>();
        childAnimator = GetComponentInChildren<Animator>();
        agent.isStopped = true;

        stateMachine = new StateMachine();

        
    }

    private void Start()
    {
        stateMachine.SetState(new BossThinkState(stateMachine,childAnimator,this));
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
}
