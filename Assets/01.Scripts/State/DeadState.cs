using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadState : Istate
{

    private StateMachine stateMachine;
    private Animator animator;
    private GameObject gameObject;
    private MonoBehaviour monoBehaviour;

    public DeadState(StateMachine machine, Animator animator, GameObject gameObject, MonoBehaviour monoBehaviour)
    {
        stateMachine = machine;
        this.animator = animator;
        this.gameObject = gameObject;
        this.monoBehaviour = monoBehaviour;
    }

    public void Enter()
    {
        animator.Play("Die");
        monoBehaviour.StartCoroutine(Die());
    }

    public void Execute(Vector3 playerVector)
    {
    }

    public void Exit()
    {
    }

    private IEnumerator Die()
    {
        // �ִϸ��̼� ���̸�ŭ ���
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);

        // ���� ������Ʈ ��Ȱ��ȭ
        gameObject.SetActive(false);
    }

}
