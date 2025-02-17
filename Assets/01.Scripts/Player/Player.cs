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
    [SerializeField]
    private float jumpPower;
    
    public GameObject[] weapons;
    
    public bool[] hasWeapons;

    public float GetWalkSpeed() => walkSpeed;
    public float GetRunSpeed() => runSpeed;
    public float GetJumpPower() => jumpPower;
    public Rigidbody GetRigidbody() => rb;
    private float playerSpeed;
    private StateMachine stateMachine;
    private bool onRun;
    public bool onJump;

    private Animator childAnimator;
    private Rigidbody rb;
    public Vector3 position;

    private GameObject nearObject;
    public GameObject equipWeapon;
    public int weaponIndex;
    
    

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
        if (stateMachine.currentState is JumpState) return; // ���� ������ ���� �������� ����

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
    public void OnTurn()
    {
        transform.LookAt(transform.position + position);
    }
    public void OnRun()
    {
        onRun = true;
        stateMachine.SetState(new RunState(stateMachine, childAnimator,this));
    }
    public void OnJump()
    {
        stateMachine.SetState(new JumpState(stateMachine, childAnimator, this));      
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
    public void OnInteration()
    {
        if(nearObject != null)
        {
            if(nearObject.tag == "Weapon")
            {
                Item item = nearObject.GetComponent<Item>();
                int weaponIndex = item.value;
                hasWeapons[weaponIndex] = true;

                Destroy(nearObject);
            }
        }
    }
    public void OnSwap1()
    {
        weaponIndex = 0;
        
        stateMachine.SetState(new SwapState(stateMachine, childAnimator,this));

    }
    public void OnSwap2()
    {
        weaponIndex = 1;
        
        stateMachine.SetState(new SwapState(stateMachine, childAnimator, this));
    }
    public void OnSwap3()
    {
        weaponIndex = 2;
        
        stateMachine.SetState(new SwapState(stateMachine, childAnimator, this));
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Floor"))
        {
            onJump = false; // ���� �� ���� �����ϵ��� ����
            stateMachine.SetState(new LandState(stateMachine, childAnimator, this));
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if(other.tag == "Weapon")
        {
            nearObject = other.gameObject;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Weapon")
        {
            nearObject = null;
        }
    }


    public void SetSpeed(float speed)
    {
        playerSpeed = speed;
    }
    public void SetMoveLock(bool moveLock)
    {
        position = Vector3.zero;
    }
}
