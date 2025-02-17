using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwapState : Istate
{
    private StateMachine stateMachine;
    private Animator animator;
    private Player player;
    private Rigidbody rb;
    public SwapState(StateMachine machine, Animator animator, Player player)
    {
        stateMachine = machine;
        this.animator = animator;
        this.player = player;
        this.rb = player.GetRigidbody();
    }
    public void Enter()
    {
        animator.Play("Swap");        
        if (player.equipWeapon != null)
        {
            player.equipWeapon.SetActive(false);
        }
        player.equipWeapon = player.weapons[player.weaponIndex];
        player.equipWeapon.SetActive(true);
    }

    public void Execute(Vector3 playerVector)
    {
        if (playerVector.magnitude > 0)
        {
            Vector3 move = playerVector.normalized * Time.fixedDeltaTime;
            rb.MovePosition(rb.position + move);
        }
    }

    public void Exit()
    {
        
    }
}
