using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeState : Istate
{
    private StateMachine stateMachine;
    private Animator animator;
    private Player player;
    private Weapon weapon;
    public MeleeState(StateMachine machine, Animator animator, Player player,Weapon weapon)
    {
        stateMachine = machine;
        this.animator = animator;
        this.player = player;
        this.weapon = weapon;
    }
    public void Enter()
    {
        if (player.equipWeapon == null)
        {
            stateMachine.SetState(new IdleState(stateMachine, animator, player)); // IdleState�� ����
            return;
        }

        player.isFireReady = player.equipWeapon.rate < player.fireDelay;

        if (player.isFireReady)
        {
            player.onAttack = true;
            animator.SetBool("IsSwing", true);
            animator.Play("Swing");
            player.StartCoroutine(Swing());
        }
    }


    public void Execute(Vector3 playerVector)
    {
        if (player.equipWeapon == null)
        {
            stateMachine.SetState(new IdleState(stateMachine, animator, player)); // Idle�� ����

            return;
        }

        player.fireDelay += Time.deltaTime;
        if (animator.GetBool("IsSwing"))
        {
            player.SetMoveLock(true);
        }
    }

    public void Exit()
    {
        
    }
    IEnumerator Swing()
    {
        yield return new WaitForSeconds(0.4f);
        weapon.meleeArea.enabled = true;
        weapon.trailEffect.enabled = true;

        yield return new WaitForSeconds(0.1f);
        weapon.meleeArea.enabled = false;

        yield return new WaitForSeconds(0.3f);
        weapon.trailEffect.enabled = false;

        animator.SetBool("IsSwing", false);

        // �ִϸ��̼��� ������ false�� �� ������ ���
        yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f);

        player.SetMoveLock(false); // �ִϸ��̼��� ���� �Ŀ� ������ �� �ֵ��� ����        
        player.fireDelay = 0;
        yield return new WaitForSeconds(0.5f);
        player.onAttack = false;
    }

}
