using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossRockShotState : Istate
{
    private StateMachine stateMachine;
    private Animator animator;
    private Boss boss;

    public BossRockShotState(StateMachine machine, Animator animator, Boss boss)
    {
        stateMachine = machine;
        this.animator = animator;
        this.boss = boss;
    }

    public void Enter()
    {
        boss.StartCoroutine(RockShot());
    }

    public void Execute(Vector3 playerVector)
    {
        
    }

    public void Exit()
    {
        
    }

    IEnumerator RockShot()
    {
        boss.isLook = false;
        animator.Play("BigShot");
        GameObject bossRock = GameManager.Instance.objectpool.Get(6);
        Vector3 bossRockSpawnPosition = boss.transform.position;
        bossRock.transform.position = bossRockSpawnPosition;
        yield return new WaitForSeconds(3f);

        stateMachine.SetState(new BossThinkState(stateMachine,animator,boss));
    }
}
