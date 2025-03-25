using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
    public int score;
    public int hasGrenades;
    public GameObject grenadeObj;
    private SkinnedMeshRenderer[] meshs;

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
    private bool isDamage;
    private bool isShop;
    public bool onAuto;

    private Animator childAnimator;
    private Rigidbody rb;
    public Vector3 position;

    private GameObject nearObject;
    public Weapon equipWeapon;
    public int weaponIndex;

    public float fireDelay;
    public bool isFireReady;
    public bool isDead;
    public FireModeSwitcher fireModeSwitcher;

    private float fireRate = 0.2f; // �⺻ �߻� �ӵ� (0.2�ʿ� �� ��)
    private float nextFireTime = 0f; // ���� �߻簡 ������ �ð�

    private void Awake()
    {
        fireModeSwitcher = new FireModeSwitcher();
        childAnimator = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody>();
        stateMachine = new StateMachine();
        meshs = GetComponentsInChildren<SkinnedMeshRenderer>();

    }
    private void Start()
    {
        playerSpeed = walkSpeed;
        stateMachine.SetState(new IdleState(stateMachine, childAnimator, this));
    }
    private void Update()
    {
        OnPush();
        OnTurn();
        stateMachine.Update(position);
        UpdateMovementState();

        // �ڵ� �߻� ����� ��� �߻� ���� ����
        if (onAuto)
        {
            HandleAutoFire();
        }
    }
    public void OnMove(InputValue value)
    {
        Vector2 inputVector = value.Get<Vector2>();
        position = new Vector3(inputVector.x, 0, inputVector.y);
    }
    private void UpdateMovementState()
    {
        if (stateMachine.currentState is JumpState) return; // ���� ������ ���� �������� ����

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
    public void OnFireMode()
    {
        fireModeSwitcher.SwitchFireMode();
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
        if (isShop)
            return;
        Debug.Log($"���� ���� ���� :{equipWeapon.type}");
        //if (equipWeapon.type == Weapon.Type.Melee)
        //{
        //    stateMachine.SetState(new MeleeState(stateMachine, childAnimator, this, equipWeapon));
        //}
        //else if (equipWeapon.type == Weapon.Type.HandGun)
        //{
        //    stateMachine.SetState(new HandGunShotState(stateMachine, childAnimator, this, equipWeapon));
        //}
        //else if (equipWeapon.type == Weapon.Type.SubMachine)
        //{
        //    stateMachine.SetState(new SubMachineGunShotState(stateMachine, childAnimator, this, equipWeapon));
        //}
        Debug.Log($"���ο� ���� : {stateMachine.currentState?.GetType().Name}"); // ���� ���� �� Ȯ��
        
    }
    // �ڵ� �߻� ��� ���� �޼��� �߰�
    private void HandleAutoFire()
    {
        if (Time.time < nextFireTime) return; // �߻� ���� �ð��� �ƴ� ��� ����

        nextFireTime = Time.time + fireRate; // ���� �߻� ���� �ð� ����
        AudioManager.Instance.PlaySfx(AudioManager.SFX.Shot);
        if (fireModeSwitcher.currentMode == FireMode.Single)
        {
            fireRate = 0.5f; // �ܹ� ���� õõ�� �߻�
            stateMachine.SetState(new SingleFireState(stateMachine, childAnimator, this, equipWeapon));
        }
        else if (fireModeSwitcher.currentMode == FireMode.Auto)
        {
            fireRate = 0.1f; // �ڵ� ���� ������ �߻�
            stateMachine.SetState(new AutoFireState(stateMachine, childAnimator, this, equipWeapon));
        }
        else if (fireModeSwitcher.currentMode == FireMode.Burst)
        {
            fireRate = 0.5f; // ���� ���� �߰� �ӵ�
            StartCoroutine(BurstFireCoroutine()); // ����� ���������� �� �� ���� �ϹǷ� �ڷ�ƾ ���
        }
    }
    private IEnumerator BurstFireCoroutine()
    {
        int burstCount = 3; // 3����
        for (int i = 0; i < burstCount; i++)
        {
            if (!onAuto) break; // ���߿� ��ư�� ���� ����
            stateMachine.SetState(new BurstFireState(stateMachine, childAnimator, this, equipWeapon));
            yield return new WaitForSeconds(0.1f); // ���� ����
        }
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
        AudioManager.Instance.PlaySfx(AudioManager.SFX.Reload);
        stateMachine.SetState(new ReloadState(stateMachine, childAnimator, this, equipWeapon));
    }
    public void OnGrenade()
    {
        if (hasGrenades == 0)
            return;

        Ray ray = followCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit rayHit;
        if (Physics.Raycast(ray, out rayHit, 100))
        {
            Vector3 nextVec = rayHit.point - transform.position;
            nextVec.y = 0;
            
            stateMachine.SetState(new GrenadeState(stateMachine,childAnimator, this, equipWeapon));
        }

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
    public void OnPush()
    {
        if (Input.GetMouseButton(0)) 
        {
            onAuto = true;
        }
        else
        {
            onAuto= false;
        }
            
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
            else if (nearObject.tag == "Shop")
            {
                Shop shop = nearObject.GetComponent<Shop>();
                isShop = true;
                shop.Enter(this);



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
            onJump = false; // ���� �� ���� �����ϵ��� ����
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
        else if(other.tag == "EnemyBullet")
        {
            if (!isDamage)
            {
                Bullet enemyBullet = other.GetComponent<Bullet>();
                health -= enemyBullet.damage;

                bool isBossAtk = other.name == "BossMeleeArea";

                StartCoroutine(OnDamage(isBossAtk));
            }
            if (other.GetComponent<Rigidbody>() != null)
                other.gameObject.SetActive(false);
        }
    }
    IEnumerator OnDamage(bool isBossAtk)
    {
        isDamage = true;
        AudioManager.Instance.PlaySfx(AudioManager.SFX.Hit);
        foreach(SkinnedMeshRenderer mesh in meshs)
        {
            mesh.material.color = Color.red;
        }

        if(isBossAtk)
            rb.AddForce(transform.forward * -25, ForceMode.Impulse);
        yield return new WaitForSeconds(1f);

        isDamage = false;

        foreach (SkinnedMeshRenderer mesh in meshs)
        {
            mesh.material.color = Color.white;
        }
        if(isBossAtk)
            rb.velocity = Vector3.zero;
        
        if(health <= 0 && !isDead)
        {
            isDead = true;
            stateMachine.SetState(new DeadState(stateMachine, childAnimator,gameObject,this));
            GameManager.Instance.GameOver();
        }

    }
    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Weapon" || other.tag == "Shop")
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
        else if (other.tag == "Shop")
        {
            Shop shop = nearObject.GetComponent<Shop>();
            shop.Exit();
            isShop = false;
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
