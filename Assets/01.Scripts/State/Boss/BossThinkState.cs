using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossThinkState : Istate
{
    private StateMachine stateMachine;
    private Animator animator;
    private Boss boss;

    public BossThinkState(StateMachine Machine, Animator animator, Boss boss)
    {
        stateMachine = Machine;
        this.animator = animator;
        this.boss = boss;
    }

    public void Enter()
    {
        boss.StartCoroutine(Think());
    }

    public void Execute(Vector3 playerVector)
    {
        
    }

    public void Exit()
    {
        
    }

    IEnumerator Think()
    {
        yield return new WaitForSeconds(0.1f);

        int ranAction = Random.Range(0, 5);
        switch (ranAction)
        {
            case 0:
            case 1:
                stateMachine.SetState(new BossMissileShot(stateMachine,animator,boss));
                break;

            case 2:
            case 3:
                stateMachine.SetState(new BossRockShotState(stateMachine,animator,boss));
                break;

            case 4:

                stateMachine.SetState(new BossTauntState(stateMachine,animator,boss));
                break;
        }
    }
}
