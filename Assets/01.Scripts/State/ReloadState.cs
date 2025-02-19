using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReloadState : Istate
{
    private StateMachine stateMachine;
    private Animator animator;
    private Player player;
    private Weapon weapon;
    public ReloadState(StateMachine machine, Animator animator, Player player, Weapon weapon)
    {
        stateMachine = machine;
        this.animator = animator;
        this.player = player;
        this.weapon = weapon;
    }
    public void Enter()
    {
        animator.Play("Reload");
        int reAmmo = player.ammo < weapon.maxAmmo ? player.ammo : weapon.maxAmmo;
        weapon.curAmmo = reAmmo;
        player.ammo -= reAmmo;

    }

    public void Execute(Vector3 playerVector)
    {
        
    }

    public void Exit()
    {
        
    }
}
