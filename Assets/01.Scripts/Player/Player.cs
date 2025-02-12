using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float playerSpeed;
    private StateMachine stateMachine;
    private Animator animator;
    private Rigidbody rb;
    public Vector3 position;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        stateMachine = new StateMachine();

    }
    private void Start()
    {
        stateMachine.SetState(new IdleState(stateMachine, animator));
    }

    public void OnMove(InputValue value)
    {
        Vector2 inputVector = value.Get<Vector2>();
        position = new Vector3(inputVector.x, 0, inputVector.y);
    }
    public void OnAttack()
    {
        Debug.Log("attack");
    }
    private void Update()
    {
        stateMachine.Update(position);
    }
    private void FixedUpdate()
    {
        Vector3 move = position.normalized * playerSpeed * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + move);

    }
}
