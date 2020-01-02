using System;
using System.Collections.Generic;
using UnityEngine;

public class SoldierPlayerPawn:PlayerPawn
{
    private bool canAttackTwo;
    private bool canAttackThree;

    protected override void Awake()
    {
        base.Awake();

        m_Weapon = new SoldierWeapon(this);
    }

    protected override void OnMouseLeft()
    {
        if (canAttackThree)
        {
            
        }
        else if (canAttackTwo)
        {
            
        }
        else
        {
            
        }
    }
    protected override void OnMouseRight()
    {
        
    }
    protected override void OnRoll()
    {
        
    }

    void OnAttackOne()
    {
        m_Weapon.StartFire((byte)ESoldierWeapon.ESoldier_AttackOne);
    }
    void OnAttackTwo()
    {
        m_Weapon.StartFire((byte)ESoldierWeapon.ESoldier_AttackTwo);
        canAttackThree = true;
    }
    void OnAttackThree()
    {
        m_Weapon.StartFire((byte)ESoldierWeapon.ESoldier_AttackThree);
        canAttackTwo = true;
    }
}