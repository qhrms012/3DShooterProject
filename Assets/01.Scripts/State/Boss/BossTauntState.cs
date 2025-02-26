using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossTauntState : Istate
{

    private StateMachine stateMachine;
    private Animator animator;
    private Boss boss;

    public BossTauntState(StateMachine machine, Animator animator, Boss boss)
    {
        stateMachine = machine;
        this.animator = animator;
        this.boss = boss;
    }

    public void Enter()
    {
        boss.StartCoroutine(Taunt());
    }

    public void Execute(Vector3 playerVector)
    {
        
    }

    public void Exit()
    {
        
    }

    IEnumerator Taunt()
    {
        boss.tauntVec = boss.target.position + boss.lookVec;

        boss.isLook = false;
        boss.agent.isStopped = false;
        boss.bc.enabled = false;
        animator.Play("Taunt");
        boss.agent.SetDestination(boss.tauntVec);
        yield return new WaitForSeconds(1.5f);        
        boss.meleeArea.enabled = true;
        
        yield return new WaitForSeconds(1.5f);
        boss.meleeArea.enabled = false;

        yield return new WaitForSeconds(3f);
        boss.isLook = true;
        boss.bc.enabled = true;
        stateMachine.SetState(new BossThinkState(stateMachine, animator, boss));
    }

}
