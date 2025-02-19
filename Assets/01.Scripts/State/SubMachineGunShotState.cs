using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubMachineGunShotState : Istate
{
    private StateMachine stateMachine;
    private Animator animator;
    private Player player;
    private Weapon weapon;
    public SubMachineGunShotState(StateMachine machine, Animator animator, Player player, Weapon weapon)
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
        if (player.isFireReady)
        {
            animator.Play("Shot");
            player.StartCoroutine(SubMachineShot());
        }
    }

    public void Execute(Vector3 playerVector)
    {
        player.fireDelay += Time.deltaTime;
    }

    public void Exit()
    {
        
    }
    IEnumerator SubMachineShot()
    {

        GameObject subMachineBullet = GameManager.Instance.objectpool.Get(1);
        Rigidbody bulletRigid = subMachineBullet.GetComponent<Rigidbody>();
        Transform spawnBulletPos = weapon.bulletPos;

        subMachineBullet.transform.position = spawnBulletPos.position;
        subMachineBullet.transform.rotation = spawnBulletPos.rotation;

        // �Ѿ��� ������ �������� ���� �߰�
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

    }
}
