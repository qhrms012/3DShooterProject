using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LandState : Istate
{
    private StateMachine stateMachine;
    private Animator animator;
    private Player player;

    public LandState(StateMachine stateMachine, Animator animator, Player player)
    {
        this.stateMachine = stateMachine;
        this.animator = animator;
        this.player = player;
    }

    public void Enter()
    {
        animator.Play("Land"); // ���� �ִϸ��̼� ���
        player.onJump = false;
    }

    public void Execute(Vector3 position)
    {

    }

    public void Exit() 
    { 
        //stateMachine.SetState(new IdleState(stateMachine,animator, player));
    }
}

