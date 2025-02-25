using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BossMissileShot : Istate
{
    private StateMachine stateMachine;
    private Animator animator;
    private Boss boss;

    public BossMissileShot(StateMachine machine, Animator animator, Boss boss)
    {
        stateMachine = machine;
        this.animator = animator;
        this.boss = boss;
    }


    public void Enter()
    {
        animator.Play("Shot");
        boss.StartCoroutine(MissileShot());
    }

    public void Execute(Vector3 playerVector)
    {
        
    }

    public void Exit()
    {
        
    }

    IEnumerator MissileShot()
    {
        GameObject instantBossMissileA = GameManager.Instance.objectpool.Get(5);
        BossMissile bossMissileA = instantBossMissileA.GetComponent<BossMissile>();

        Vector3 bulletSpawnPositionA = boss.missilePortA.transform.position;
        instantBossMissileA.transform.position = bulletSpawnPositionA;
        bossMissileA.target = boss.target;



        yield return new WaitForSeconds(0.3f);
        GameObject instantBossMissileB = GameManager.Instance.objectpool.Get(5);
        BossMissile bossMissileB = instantBossMissileB.GetComponent<BossMissile>();
        Vector3 bulletSpawnPositionB = boss.missilePortB.transform.position;
        instantBossMissileB.transform.position = bulletSpawnPositionB;

        bossMissileB.target = boss.target;

        yield return new WaitForSeconds(2f);
    }
}
