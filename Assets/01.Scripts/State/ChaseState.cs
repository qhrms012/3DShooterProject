using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ChaseState : Istate
{
    private StateMachine stateMachine;
    private Animator animator;
    private NavMeshAgent agent;
    private Transform target;

    public ChaseState(StateMachine machine, Animator animator, NavMeshAgent agent, Transform target)
    {
        stateMachine = machine;
        this.animator = animator;
        this.agent = agent;
        this.target = target;
    }
    public void Enter()
    {      
        animator.Play("Walk");
    }

    public void Execute(Vector3 playerVector)
    {
        if (agent.enabled) 
            agent.SetDestination(target.position);
    }

    public void Exit()
    {
        
    }
}
