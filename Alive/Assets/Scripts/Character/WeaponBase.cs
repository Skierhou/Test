using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[Config]
public class WeaponBase :BaseClass
{
    public Pawn m_Owner;
    [Config]
    public List<string> m_AttackAnimList;
    [Config]
    public List<float> m_AttackOutTimeList;
    [Config]
    public List<int> m_AttackDamageList;
    [Config]
    public List<float> m_AttackDistanceList;

    private int m_AttackIndex;
    private float m_AttackTimer;
    private bool bStartAttack;

    public WeaponBase(Pawn inOwner)
    {
        m_Owner = inOwner;
    }

    public void OnUpdate()
    {
        if (bStartAttack)
        {
            if (m_AttackAnimList != null && m_AttackOutTimeList != null
            && m_AttackOutTimeList.Count > m_AttackIndex)
            {
                if (Time.time - m_AttackTimer > m_AttackOutTimeList[m_AttackIndex])
                {
                    m_AttackIndex = 0;
                }
            }
            else
            {
                m_AttackIndex = 0;
            }
        }
    }

    /// <summary>
    /// 攻击具体实现,用于计算攻击造成实际作用
    /// </summary>
    public virtual void StartFire(byte inFireMode)
    {
        
    }

    public void ResetAttack()
    {
        m_AttackIndex = 0;
        m_AttackTimer = Time.time;
    }

    public void StartAttackTimer()
    {
        m_AttackTimer = Time.time;
        bStartAttack = true;
    }

    public string GetAttackAnimName()
    {
        return m_AttackAnimList[m_AttackIndex];
    }

}