using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpState : Istate
{
    private StateMachine stateMachine;
    private Animator animator;
    private Player player;
    private Rigidbody rb;
    public JumpState(StateMachine machine, Animator animator, Player player)
    {
        stateMachine = machine;
        this.animator = animator;
        this.player = player;
        this.rb = player.GetRigidbody();        
    }
    public void Enter()
    {
        animator.Play("Jump");
        if (!player.onJump)
        {
            player.onJump = true;
            rb.AddForce(Vector3.up * player.GetJumpPower(), ForceMode.Impulse);
        }
    }

    public void Execute(Vector3 playerVector)
    {
        if (player.IsGrounded()) // ∂•ø° ¥Í¿∏∏È IdleState∑Œ ∫Ø∞Ê
        {
            stateMachine.SetState(new IdleState(stateMachine, animator, player));
        }
    }

    public void Exit()
    {
        player.onJump = false;
    }

}
