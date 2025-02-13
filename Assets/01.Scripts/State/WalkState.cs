using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkState : Istate
{
    private StateMachine stateMachine;
    private Animator animator;

    public WalkState(StateMachine machine, Animator animator)
    {
        stateMachine = machine;
        this.animator = animator;
    }
    public void Enter()
    {
        animator.Play("Walk");
    }

    public void Execute(Vector3 playerVector)
    {
        if (playerVector.magnitude == 0)
        {
            stateMachine.SetState(new IdleState(stateMachine, animator));
        }
    }

    public void Exit()
    {
        
    }
}
