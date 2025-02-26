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
        // 애니메이션 길이만큼 대기
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);

        // 게임 오브젝트 비활성화
        gameObject.SetActive(false);
    }

}
