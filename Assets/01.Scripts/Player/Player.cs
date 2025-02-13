using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{

    [SerializeField]
    private float walkSpeed;
    [SerializeField]
    private float runSpeed;

    public float GetWalkSpeed() => walkSpeed;
    public float GetRunSpeed() => runSpeed;

    private float playerSpeed;
    private StateMachine stateMachine;
    private bool onRun;

    private Animator childAnimator;
    private Rigidbody rb;
    public Vector3 position;

    private void Awake()
    {
        childAnimator = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody>();
        stateMachine = new StateMachine();

    }
    private void Start()
    {
        playerSpeed = walkSpeed;
        stateMachine.SetState(new IdleState(stateMachine, childAnimator,this));
    }
    private void Update()
    {
        OnTurn();
        stateMachine.Update(position);
        UpdateMovementState();
    }
    public void OnMove(InputValue value)
    {
        Vector2 inputVector = value.Get<Vector2>();
        position = new Vector3(inputVector.x, 0, inputVector.y);
    }
    private void UpdateMovementState()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            OnRun();
        }
        else if (position.magnitude != 0)
        {
            onRun = false;
            stateMachine.SetState(new WalkState(stateMachine, childAnimator, this));
        }
    }
    private void OnTurn()
    {
        transform.LookAt(transform.position + position);
    }
    public void OnRun()
    {
        onRun = true;
        stateMachine.SetState(new RunState(stateMachine, childAnimator,this));
    }
    public void OnAttack()
    {
        Debug.Log("attack");
    }
    private void FixedUpdate()
    {
        Vector3 move = position.normalized * playerSpeed * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + move);

    }
    public void SetSpeed(float speed)
    {
        playerSpeed = speed;
    }
}
