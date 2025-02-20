using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeState : Istate
{
    private StateMachine stateMachine;
    private Animator animator;
    private Player player;
    private Weapon weapon;
    public GrenadeState(StateMachine machine, Animator animator, Player player, Weapon weapon)
    {
        stateMachine = machine;
        this.animator = animator;
        this.player = player;
        this.weapon = weapon;
    }
    public void Enter()
    {
        animator.Play("Throw");
        GameObject poolGrenade = GameManager.Instance.objectpool.Get(3);
        // �÷��̾� ���ʿ� ��ȯ�ǵ��� ��ġ ����
        Vector3 spawnPosition = player.transform.position + player.transform.forward * 1.5f + Vector3.up * 1.0f;
        poolGrenade.transform.position = spawnPosition;

        Rigidbody rigidGrenade = poolGrenade.GetComponent<Rigidbody>();

        // �÷��̾�������� ���� �ֱ�
        rigidGrenade.AddForce(player.transform.forward * 10 + Vector3.up * 10, ForceMode.Impulse);
        rigidGrenade.AddTorque(Vector3.back * 10, ForceMode.Impulse);

        player.hasGrenades--;
        player.grenades[player.hasGrenades].SetActive(false);


    }

    public void Execute(Vector3 playerVector)
    {
        
    }

    public void Exit()
    {
        
    }
}
