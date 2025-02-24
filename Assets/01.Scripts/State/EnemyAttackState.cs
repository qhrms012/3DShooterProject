using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackState : Istate
{
    
    private StateMachine stateMachine;
    private Animator animator;
    private BoxCollider meleeArea;
    private Rigidbody rb;
    private Enemy enemy;
    public EnemyAttackState(StateMachine machine, Animator animator, BoxCollider meleeArea, Enemy enemy, Rigidbody rb)
    {
        stateMachine = machine;
        this.animator = animator;
        this.meleeArea = meleeArea;
        this.enemy = enemy;
        this.rb = rb;
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
        switch (enemy.enemyType)
        {
            case Enemy.Type.A:   
                yield return new WaitForSeconds(0.2f);
                meleeArea.enabled = true;

                yield return new WaitForSeconds(1f);
                meleeArea.enabled = false;

                yield return new WaitForSeconds(1f);
                break;
            case Enemy.Type.B:
                yield return new WaitForSeconds(0.1f);
                rb.AddForce(enemy.transform.forward * 20, ForceMode.Impulse);
                meleeArea.enabled = true;

                yield return new WaitForSeconds(0.5f);
                rb.velocity = Vector3.zero;
                meleeArea.enabled = false;

                yield return new WaitForSeconds(2f);
                break;

            case Enemy.Type.C:
                yield return new WaitForSeconds(0.5f);
                GameObject enemyBullet = GameManager.Instance.objectpool.Get(4);
                Rigidbody rigidBullet = enemyBullet.GetComponent<Rigidbody>();

                // 총알의 위치를 적의 위치 + y축 5만큼 위로 설정
                Vector3 bulletSpawnPosition = enemy.transform.position + new Vector3(0, 2f, 0);

                enemyBullet.transform.position = bulletSpawnPosition;
                enemyBullet.transform.rotation = enemy.transform.rotation;

                rigidBullet.velocity = enemy.transform.forward * 20;

                yield return new WaitForSeconds(2f);
                break;

        }

        enemy.isAttack = false;
        // 공격이 끝나면 다시 추격 상태로 전환
        stateMachine.SetState(new ChaseState(stateMachine, enemy.GetAnimator(), 
            enemy.GetAgent(), enemy.target));

    }
}
