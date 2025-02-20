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

    public Camera followCamera;
    public int ammo;
    public int coin;
    public int health;
    public int hasGrenades;

    public int maxAmmo;
    public int maxCoin;
    public int maxHealth;

    public int maxhasGrenades;
    public GameObject[] weapons;
    public GameObject[] grenades;

    public bool[] hasWeapons;


    public float GetWalkSpeed() => walkSpeed;
    public float GetRunSpeed() => runSpeed;
    public float GetJumpPower() => jumpPower;
    public Rigidbody GetRigidbody() => rb;
    private float playerSpeed;
    private StateMachine stateMachine;
    private bool onRun;
    public bool onJump;
    public bool onAttack;
    private bool isBorder;

    private Animator childAnimator;
    private Rigidbody rb;
    public Vector3 position;

    private GameObject nearObject;
    public Weapon equipWeapon;
    public int weaponIndex;

    public float fireDelay;
    public bool isFireReady;



    private void Awake()
    {
        childAnimator = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody>();
        stateMachine = new StateMachine();

    }
    private void Start()
    {
        playerSpeed = walkSpeed;
        stateMachine.SetState(new IdleState(stateMachine, childAnimator, this));
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
        if (stateMachine.currentState is JumpState) return; // 점프 상태일 때는 변경하지 않음

        if (onAttack) return;
        if (Input.GetKey(KeyCode.LeftShift))
        {
            OnRun();
        }
        else if (position.magnitude != 0 )
        {
            onRun = false;
            stateMachine.SetState(new WalkState(stateMachine, childAnimator, this));
        }



    }
    public void OnTurn()
    {
        transform.LookAt(transform.position + position);

        if (Input.GetMouseButton(0))
        {
            Ray ray = followCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit rayHit;
            if (Physics.Raycast(ray, out rayHit, 100))
            {
                Vector3 nextVec = rayHit.point - transform.position;
                nextVec.y = 0;
                transform.LookAt(transform.position + nextVec);
            }
        }
    }
    public void OnRun()
    {
        onRun = true;
        stateMachine.SetState(new RunState(stateMachine, childAnimator, this));
    }
    public void OnJump()
    {
        stateMachine.SetState(new JumpState(stateMachine, childAnimator, this));
    }
    public void OnAttack()
    {
        Debug.Log($"현재 장착 무기 :{equipWeapon.type}");
        if (equipWeapon.type == Weapon.Type.Melee)
        {
            stateMachine.SetState(new MeleeState(stateMachine, childAnimator, this, equipWeapon));
        }
        else if (equipWeapon.type == Weapon.Type.HandGun)
        {
            stateMachine.SetState(new HandGunShotState(stateMachine, childAnimator, this, equipWeapon));
        }
        else if (equipWeapon.type == Weapon.Type.SubMachine)
        {
            stateMachine.SetState(new SubMachineGunShotState(stateMachine, childAnimator, this, equipWeapon));
        }
        Debug.Log($"새로운 상태 : {stateMachine.currentState?.GetType().Name}"); // 상태 변경 후 확인
    }
    public void OnReload()
    {
        if (equipWeapon == null)
            return;
        if (equipWeapon.type == Weapon.Type.Melee)
            return;
        if (ammo == 0)
            return;
        if (stateMachine.currentState is RunState && stateMachine.currentState is JumpState)
            return;
        stateMachine.SetState(new ReloadState(stateMachine, childAnimator, this, equipWeapon));
    }

    private void FreezeRotation()
    {
        rb.angularVelocity = Vector3.zero;
    }
    private void StopToWall()
    {
        Debug.DrawRay(transform.position, transform.forward * 5, Color.green);
        isBorder = Physics.Raycast(transform.position, transform.forward,
            5, LayerMask.GetMask("Wall"));

    }
    private void FixedUpdate()
    {
        if (!isBorder)
        {
            Vector3 move = position.normalized * playerSpeed * Time.fixedDeltaTime;
            rb.MovePosition(rb.position + move);
        }
        FreezeRotation();
        StopToWall();

    }
    public void OnInteration()
    {
        if (nearObject != null)
        {
            if (nearObject.tag == "Weapon")
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

        stateMachine.SetState(new SwapState(stateMachine, childAnimator, this));

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
            onJump = false; // 착지 시 점프 가능하도록 설정
            stateMachine.SetState(new LandState(stateMachine, childAnimator, this));
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Item")
        {
            Item item = other.GetComponent<Item>();
            switch (item.type)
            {
                case Item.Type.Ammo:
                    ammo += item.value;
                    if (ammo > maxAmmo)
                        ammo = maxAmmo;
                    break;
                case Item.Type.Coin:
                    coin += item.value;
                    if (coin > maxCoin)
                        coin = maxCoin;
                    break;
                case Item.Type.Heart:
                    health += item.value;
                    if (health > maxHealth)
                        health = maxHealth;
                    break;
                case Item.Type.Grenade:
                    grenades[hasGrenades].SetActive(true);
                    hasGrenades += item.value;
                    if (hasGrenades > maxhasGrenades)
                        hasGrenades = maxhasGrenades;
                    break;
            }
            Destroy(other.gameObject);
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Weapon")
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
