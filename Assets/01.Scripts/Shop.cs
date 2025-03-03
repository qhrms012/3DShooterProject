using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour
{
    public RectTransform uiGroup;
    public Animator childAnimator;

    Player enterPlayer;

    public void Enter(Player player)
    {
        enterPlayer = player;
        uiGroup.anchoredPosition = Vector3.zero;
        Debug.Log("µé¾î¿È");
    }


    public void Exit()
    {
        childAnimator.Play("Hello");
        uiGroup.anchoredPosition = Vector3.down * 1000;

    }
}
