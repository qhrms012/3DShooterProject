using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : MonoBehaviour
{
    public GameObject meshObj;
    public GameObject effectObj;
    public Rigidbody rb;

    
    public void Throw()
    {
        ResetGrenade();  // √ ±‚»≠
        StartCoroutine(Explosion());  
    }

    IEnumerator Explosion()
    {
        yield return new WaitForSeconds(3f);

        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        meshObj.SetActive(false);
        effectObj.SetActive(true);  

        RaycastHit[] rayHits = Physics.SphereCastAll(transform.position, 15, Vector3.up,
            0, LayerMask.GetMask("Enemy"));

        foreach (RaycastHit hitObj in rayHits)
        {
            hitObj.transform.GetComponent<Enemy>().HitByGrenade(transform.position);
        }

        StartCoroutine(DisableCoroutine(gameObject, 2));
    }

    private IEnumerator DisableCoroutine(GameObject obj, float delay)
    {
        yield return new WaitForSeconds(delay);
        obj.SetActive(false);
    }

    
    public void ResetGrenade()
    {
        meshObj.SetActive(true);
        effectObj.SetActive(false);  
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }
}

