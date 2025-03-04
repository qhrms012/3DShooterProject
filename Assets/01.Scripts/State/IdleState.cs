using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : Istate
{
    private StateMachine stateMachine;
    private Animator animator;
    private Player player;

    public IdleState(StateMachine Machine, Animator animator, Player player)
    {
        stateMachine = Machine;
        this.animator = animator;
        this.player = player;
    }
    public void Enter()
    {
        animator.Play("Idle");
    }

    public void Execute(Vector3 playerVector)
    {
        if(playerVector.magnitude > 0)
        {
            stateMachine.SetState(new WalkState(stateMachine, animator,player));
        }
    }

    public void Exit()
    {
        
    }
}
