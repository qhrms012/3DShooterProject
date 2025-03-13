using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleFireState : Istate
{
    private StateMachine stateMachine;
    private Animator animator;
    private Player player;
    private Weapon weapon;

    public SingleFireState(StateMachine machine, Animator animator, Player player, Weapon weapon)
    {
        stateMachine = machine;
        this.animator = animator;
        this.player = player;
        this.weapon = weapon;
    }
    public void Enter()
    {
        if (weapon.curAmmo <= 0)
        {
            stateMachine.SetState(new IdleState(stateMachine, animator, player)); // IdleState로 변경
            return;
        }
        player.isFireReady = player.equipWeapon.rate < player.fireDelay;
        if (player.isFireReady && weapon.curAmmo > 0)
        {
            player.StopCoroutine(SingleShot());
            weapon.curAmmo--;           
            player.StartCoroutine(SingleShot());
        }
    }

    public void Execute(Vector3 playerVector)
    {
        player.fireDelay += Time.deltaTime;
    }

    public void Exit()
    {
        
    }

    IEnumerator SingleShot()
    {
        animator.Play("SingleShot");
        GameObject subMachineBullet = GameManager.Instance.objectpool.Get(1);
        Rigidbody bulletRigid = subMachineBullet.GetComponent<Rigidbody>();
        Transform spawnBulletPos = weapon.bulletPos;

        subMachineBullet.transform.position = spawnBulletPos.position;
        subMachineBullet.transform.rotation = spawnBulletPos.rotation;

        // 총알이 앞으로 나가도록 힘을 추가
        bulletRigid.velocity = spawnBulletPos.forward * 50f;
        yield return null;

        GameObject bulletCase = GameManager.Instance.objectpool.Get(2);
        Rigidbody caseRigid = bulletCase.GetComponent<Rigidbody>();
        Transform caseBulletPos = weapon.bulletCasePos;

        bulletCase.transform.position = caseBulletPos.position;
        bulletCase.transform.rotation = caseBulletPos.rotation;

        Vector3 caseVec = weapon.bulletCasePos.forward * Random.Range(-3, 2)
            + Vector3.up * Random.Range(2, 3);
        caseRigid.AddForce(caseVec, ForceMode.Impulse);
        caseRigid.AddTorque(Vector3.up * 10, ForceMode.Impulse);

        yield return new WaitForSeconds(0.5f);

        stateMachine.SetState(new IdleState(stateMachine, animator, player));


    }
}
