using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int damage;

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Floor")
        {
            gameObject.SetActive(false);
        }
        else if(collision.gameObject.tag == "Wall")
        {
            gameObject.SetActive(false);
        }
    }
}
