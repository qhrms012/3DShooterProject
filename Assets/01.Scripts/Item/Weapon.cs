using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public enum Type { Melee, Range};
    public Type type;
    public int damage;
    public float rate;
    public BoxCollider meleeArea;
    public TrailRenderer trailEffect;

    
    public void Use()
    {
        if(type == Type.Melee )
        {
            Swing();
        }
    }

    IEnumerator Swing()
    {


        yield return null;
    }
}
