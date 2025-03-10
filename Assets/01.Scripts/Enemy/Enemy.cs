using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public enum Type { A, B, C, D };
    public Type enemyType;
    public int maxHealth;
    public int curHealth;
    public int score;
    public GameObject[] coins;
    public Transform target;
    public BoxCollider meleeArea;
    public StateMachine stateMachine;


    public Rigidbody rb;
    public BoxCollider bc;
    public MeshRenderer[] meshs;
    public NavMeshAgent agent;
    public Animator childAnimator;
    public bool isAttack;
    private bool isDead;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        bc = GetComponent<BoxCollider>();
        meshs = GetComponentsInChildren<MeshRenderer>();
        agent = GetComponent<NavMeshAgent>();
        childAnimator = GetComponentInChildren<Animator>();

        stateMachine = new StateMachine();


    }
    private void Start()
    {
        stateMachine.SetState(new ChaseState(stateMachine, childAnimator, agent, target));
    }
    private void Update()
    {
        stateMachine.Update(target.position);
    }

    private void FixedUpdate()
    {
        FreezeRotation();
        TargetTing();
    }
    private void FreezeRotation()
    {
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }
    private void TargetTing()
    {
        if (isDead)
            return;
        float targetRadius = 0;
        float targetRange = 0;

        switch (enemyType)
        {
            case Type.A:
                targetRadius = 1.5f;
                targetRange = 3f;
                break;
            case Type.B:
                targetRadius = 1f;
                targetRange = 12f;
                break;
            case Type.C:
                targetRadius = 0.5f;
                targetRange = 25f;
                break;
        }
        RaycastHit[] rayHits =
            Physics.SphereCastAll(transform.position, targetRadius, transform.forward, targetRange,
                                                            LayerMask.GetMask("Player"));

        if (rayHits.Length > 0 && !isAttack)
        {
            stateMachine.SetState(new EnemyAttackState(stateMachine, childAnimator, meleeArea, this, rb));
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Melee")
        {
            Weapon weapon = other.GetComponent<Weapon>();
            curHealth -= weapon.damage;
            Vector3 reactVec = transform.position - other.transform.position;

            StartCoroutine(OnDamage(reactVec, false));
        }
        else if (other.tag == "Bullet")
        {
            Bullet bullet = other.GetComponent<Bullet>();
            curHealth -= bullet.damage;
            Vector3 reactVec = transform.position - other.transform.position;
            other.gameObject.SetActive(false);

            StartCoroutine(OnDamage(reactVec, false));
        }
    }
    public void HitByGrenade(Vector3 explosionPos)
    {
        curHealth -= 100;
        Vector3 reactVec = transform.position - explosionPos;
        StartCoroutine(OnDamage(reactVec, true));
    }

    IEnumerator OnDamage(Vector3 reactVec, bool isGrenade)
    {
        foreach (MeshRenderer mesh in meshs)
            mesh.material.color = Color.red;

        yield return new WaitForSeconds(0.1f);

        if (curHealth > 0)
        {
            foreach (MeshRenderer mesh in meshs)
                mesh.material.color = Color.white;
        }
        else
        {
            foreach (MeshRenderer mesh in meshs)
                mesh.material.color = Color.gray;

            gameObject.layer = 11;
            isDead = true;
            agent.enabled = true;

            if (isGrenade)
            {
                reactVec = reactVec.normalized;
                reactVec += Vector3.up * 3;
                rb.freezeRotation = false;
                rb.AddForce(reactVec * 5, ForceMode.Impulse);
                rb.AddTorque(reactVec * 15, ForceMode.Impulse);
            }
            else
            {
                reactVec = reactVec.normalized;
                reactVec += Vector3.up;
                rb.AddForce(reactVec * 5, ForceMode.Impulse);
            }
            stateMachine.SetState(new DeadState(stateMachine, childAnimator, gameObject, GameManager.Instance));
            Player player = target.GetComponent<Player>();
            player.score += score;
            int ranCoin = Random.Range(0, 3);
            Instantiate(coins[ranCoin], transform.position, Quaternion.identity);
            switch (enemyType)
            {
                case Type.A:
                    GameManager.Instance.enemyCntA--;
                    break;
                case Type.B:
                    GameManager.Instance.enemyCntB--;
                    break;
                case Type.C:
                    GameManager.Instance.enemyCntC--;
                    break;
                case Type.D:
                    GameManager.Instance.enemyCntD--;
                    break;
            }
            Destroy(gameObject, 4);
        }
    }

    public NavMeshAgent GetAgent()
    {
        return agent;
    }

    public Animator GetAnimator()
    {
        return childAnimator;
    }

}
