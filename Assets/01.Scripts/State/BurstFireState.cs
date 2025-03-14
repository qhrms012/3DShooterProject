using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurstFireState : Istate
{
    private StateMachine stateMachine;
    private Animator animator;
    private Player player;
    private Weapon weapon;

    public BurstFireState(StateMachine machine, Animator animator, Player player, Weapon weapon)
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
            stateMachine.SetState(new IdleState(stateMachine, animator, player)); // IdleState�� ����
            return;
        }
        player.isFireReady = player.equipWeapon.rate < player.fireDelay;
        if (player.isFireReady && weapon.curAmmo > 0)
        {
            player.StopCoroutine(BurstShot());
            weapon.curAmmo -= 3;           
            player.StartCoroutine(BurstShot());
        }
    }

    public void Execute(Vector3 playerVector)
    {
        player.fireDelay += Time.deltaTime;
    }

    public void Exit()
    {
        
    }

    IEnumerator BurstShot()
    {
        animator.Play("BurstShot");

        Transform spawnBulletPos = weapon.bulletPos;
        Transform caseBulletPos = weapon.bulletCasePos;

        for (int i = 0; i < 3; i++) // 3�� �ݺ�
        {
            // �Ѿ� ����
            GameObject subMachineBullet = GameManager.Instance.objectpool.Get(1);
            Rigidbody bulletRigid = subMachineBullet.GetComponent<Rigidbody>();

            subMachineBullet.transform.position = spawnBulletPos.position;
            subMachineBullet.transform.rotation = spawnBulletPos.rotation;

            // �Ѿ��� ������ �������� ���� �߰�
            bulletRigid.velocity = spawnBulletPos.forward * 50f;

            // ź�� ����
            GameObject bulletCase = GameManager.Instance.objectpool.Get(2);
            Rigidbody caseRigid = bulletCase.GetComponent<Rigidbody>();

            bulletCase.transform.position = caseBulletPos.position;
            bulletCase.transform.rotation = caseBulletPos.rotation;

            Vector3 caseVec = caseBulletPos.forward * Random.Range(-3, 2)
                + Vector3.up * Random.Range(2, 3);
            caseRigid.AddForce(caseVec, ForceMode.Impulse);
            caseRigid.AddTorque(Vector3.up * 10, ForceMode.Impulse);

            yield return new WaitForSeconds(0.1f); // 0.1�� �������� �߻�
        }

        yield return new WaitForSeconds(0.5f);
        stateMachine.SetState(new IdleState(stateMachine, animator, player));
    }
}
