using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwapState : Istate
{
    private StateMachine stateMachine;
    private Animator animator;
    private Player player;

    public SwapState(StateMachine machine, Animator animator, Player player)
    {
        stateMachine = machine;
        this.animator = animator;
        this.player = player;
    }
    public void Enter()
    {
        player.SetMoveLock(true);
        if (player.weapons[player.weaponIndex].activeSelf == true || 
            !player.hasWeapons[player.weaponIndex])
            return;       
        animator.Play("Swap");
        animator.GetComponent<Animator>().SetBool("IsSwapping", true);
        
        if (player.equipWeapon != null)
        {
            player.equipWeapon.SetActive(false);
        }
        player.equipWeapon = player.weapons[player.weaponIndex];
        player.equipWeapon.SetActive(true);
    }

    public void Execute(Vector3 playerVector)
    {
        animator.SetBool("IsSwapping", false);
        player.SetMoveLock(false);
        stateMachine.SetState(new IdleState(stateMachine, animator, player));       
    }

    public void Exit()
    {
        
    }
}
