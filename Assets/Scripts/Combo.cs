using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Combo : MonoBehaviour
{
    private int comboCounter = 0;
    private float attackPower = 1f;


    private void Start()
    {
        ResetCombo();
    }

    public void IncrementCombo()
    {
        comboCounter++;
        attackPower *=comboCounter;
        Debug.Log($"Attack Power: {attackPower}");
    }

    public void ResetCombo()
    {
        comboCounter = 0;
        attackPower = 1f;
        Debug.Log("Combo Reset. Attack Power reset to base.");
    }

    public float GetCurrentAttackPower()
    {
        return attackPower;
    }
}
