using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public int maxHealth;
    public int curHealth;
    public Transform target;
    public BoxCollider meleeArea;
    private StateMachine stateMachine;

    private Rigidbody rb;
    private BoxCollider bc;
    private Material mat;
    private NavMeshAgent agent;
    private Animator childAnimator;
    public bool isAttack;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        bc = GetComponent<BoxCollider>();
        mat = GetComponentInChildren<MeshRenderer>().material;
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
        float targetRadius = 1.5f;
        float targetRange = 3f;

        RaycastHit[] rayHits = 
            Physics.SphereCastAll(transform.position,targetRadius,transform.forward,targetRange,
                                                            LayerMask.GetMask("Player"));

        if(rayHits.Length > 0 && !isAttack)
        {
            stateMachine.SetState(new EnemyAttackState(stateMachine, childAnimator, meleeArea, this));
        }
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Melee")
        {
            Weapon weapon = other.GetComponent<Weapon>();
            curHealth -= weapon.damage;
            Vector3 reactVec = transform.position - other.transform.position;

            StartCoroutine(OnDamage(reactVec, false));
        }
        else if(other.tag == "Bullet")
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
        StartCoroutine (OnDamage(reactVec, true));
    }

    IEnumerator OnDamage(Vector3 reactVec, bool isGrenade)
    {
        mat.color = Color.red;
        yield return new WaitForSeconds(0.1f);

        if(curHealth > 0)
        {
            mat.color = Color.white;
        }
        else
        {
            mat.color = Color.gray;
            gameObject.layer = 11;

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
