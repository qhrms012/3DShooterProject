using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunState : Istate
{
    private StateMachine stateMachine;
    private Animator animator;
    private Player player;

    public RunState(StateMachine machine, Animator animator, Player player)
    {
        stateMachine = machine;
        this.animator = animator;
        this.player = player;
    }
    public void Enter()
    {
        animator.Play("Run");
        player.SetSpeed(player.GetRunSpeed());
    }

    public void Execute(Vector3 playerVector)
    {
        if (playerVector.magnitude == 0)
        {
            stateMachine.SetState(new IdleState(stateMachine, animator, player));
        }
    }

    public void Exit()
    {
        
    }
}
