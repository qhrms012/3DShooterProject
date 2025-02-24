using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackState : Istate
{
    
    private StateMachine stateMachine;
    private Animator animator;
    private BoxCollider meleeArea;
    private Enemy enemy;
    public EnemyAttackState(StateMachine machine, Animator animator, BoxCollider meleeArea, Enemy enemy)
    {
        stateMachine = machine;
        this.animator = animator;
        this.meleeArea = meleeArea;
        this.enemy = enemy;
    }

    public void Enter()
    {
        animator.Play("Attack");
        enemy.StartCoroutine(Attack());
    }

    public void Execute(Vector3 playerVector)
    {

    }

    public void Exit()
    {
        
    }

    IEnumerator Attack()
    {
        enemy.isAttack = true;
        meleeArea.enabled = true;
        yield return new WaitForSeconds(0.2f);

        yield return new WaitForSeconds(1f);
        meleeArea.enabled = false;
        enemy.isAttack = false;

        // 공격이 끝나면 다시 추격 상태로 전환
        stateMachine.SetState(new ChaseState(stateMachine, enemy.GetAnimator(), 
            enemy.GetAgent(), enemy.target));

    }
}
