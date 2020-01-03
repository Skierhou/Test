using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Config]
public class SoldierPlayerPawn : PlayerPawn
{
    [Config]
    public List<float> CFG_AttackTimes;

    private bool canAttackTwo = false;
    private bool canAttackThree = false;
    private bool canAttackFour = false;

    protected override void Awake()
    {
        base.Awake();
    }
    private void Start()
    {
        m_Weapon = new SoldierWeapon(this);
    }

    protected override void OnMouseLeft()
    {
        AnimatorStateInfo stateInfo = m_Animator.GetCurrentAnimatorStateInfo(0);
        if (stateInfo.IsName("Blend Tree"))
        {
            isAttacking = true;
            m_Animator.SetTrigger("Attack");
            m_Animator.SetBool("IsAttack",true);
        }
        else if (stateInfo.IsName("AttackOne"))
        {
            canAttackTwo = true;
        }
        else if (stateInfo.IsName("AttackTwo"))
        {
            canAttackThree = true;
        }
        else if (stateInfo.IsName("AttackThree"))
        {
            canAttackFour = true;
        }

    }

    protected override void Update()
    {
        base.Update();
    }

    protected override void OnMouseRight()
    {
        
    }
    protected override void OnRoll()
    {
        
    }

    void OnBeginFire(int inValue)
    {
        m_Weapon.StartFire((byte)(inValue - 1));
    }
    void OnEndFire(int inValue)
    {
        isAttacking = false;
        m_Animator.SetBool("IsAttack",false);
        switch (inValue)
        {
            case 1:
                if (canAttackTwo)
                {
                    m_Animator.SetTrigger("Attack");
                    canAttackTwo = false;
                    isAttacking = true;
                    m_Animator.SetBool("IsAttack",true);
                }
                break;
            case 2:
                if (canAttackThree)
                {
                    m_Animator.SetTrigger("Attack");
                    canAttackThree = false;
                    isAttacking = true;
                    m_Animator.SetBool("IsAttack",true);
                }
                break;
            case 3:
                if (canAttackFour)
                {
                    m_Animator.SetTrigger("Attack");
                    canAttackFour = false;
                    isAttacking = true;
                    m_Animator.SetBool("IsAttack",true);
                }
                break;
            default:
                break;
        }
    }
}