using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public enum Type
    {
        Ammo,
        Coin,
        Grenade,
        Heart,
        Weapon
    }
    public Type type;
    public int value;
    private Rigidbody rb;
    private SphereCollider sp;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        sp = GetComponent<SphereCollider>();
    }

    private void Update()
    {
        transform.Rotate(Vector3.up * 15 * Time.deltaTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Floor")
        {
            rb.isKinematic = true;
            sp.enabled = false;
        }
    }
}
