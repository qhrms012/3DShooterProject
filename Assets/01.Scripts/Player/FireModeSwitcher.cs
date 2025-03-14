using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FireMode { Single, Burst, Auto}
public class FireModeSwitcher : MonoBehaviour
{
    public FireMode currentMode = FireMode.Single;
    
    public  void SwitchFireMode()
    {
        currentMode = (FireMode)(((int)currentMode + 1) % System.Enum.GetValues(typeof(FireMode)).Length);
        GameManager.Instance.UpdateWeaponUI();
        Debug.Log("현재 모드: " + currentMode);
    }
}


