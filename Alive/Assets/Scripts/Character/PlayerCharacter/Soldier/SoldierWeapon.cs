using System;
using System.Collections.Generic;
using UnityEngine;


enum ESoldierWeapon
{
    ESoldier_AttackOne,
    ESoldier_AttackTwo,
    ESoldier_AttackThree,
    ESoldier_AttackFour,
};

[Config]
public class SoldierWeapon : WeaponBase
{
    private ESoldierWeapon m_CurMode;

    public SoldierWeapon(Pawn inOwner) : base(inOwner)
    {

    }

    public override void StartFire(byte inFireMode)
    {
        float attackDistance = 0;
        int attackDamage = 0;
        switch ((ESoldierWeapon)inFireMode)
        {
            case ESoldierWeapon.ESoldier_AttackOne:
                attackDistance = m_AttackDistanceList[(int)ESoldierWeapon.ESoldier_AttackOne];
                attackDamage = m_AttackDamageList[(int)ESoldierWeapon.ESoldier_AttackOne];
                Debug.Log("ESoldier_AttackOne");
                break;
            case ESoldierWeapon.ESoldier_AttackTwo:
                attackDistance = m_AttackDistanceList[(int)ESoldierWeapon.ESoldier_AttackTwo];
                attackDamage = m_AttackDamageList[(int)ESoldierWeapon.ESoldier_AttackTwo];
                Debug.Log("ESoldier_AttackTwo");
                break;
            case ESoldierWeapon.ESoldier_AttackThree:
                attackDistance = m_AttackDistanceList[(int)ESoldierWeapon.ESoldier_AttackThree];
                attackDamage = m_AttackDamageList[(int)ESoldierWeapon.ESoldier_AttackThree];
                Debug.Log("ESoldier_AttackThree");
                break;
            default:
                break;
        }
        Collider[] colliders = Physics.OverlapSphere(m_Owner.transform.position, attackDistance);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (Vector3.Dot(m_Owner.transform.forward, colliders[i].transform.position - m_Owner.transform.position) > 0)
            {
                AIPawn aiPawn = colliders[i].gameObject.GetComponent<AIPawn>();
                if (aiPawn != null)
                {
                    aiPawn.TakeDamage(attackDamage, Vector3.zero, m_Owner);
                }
            }
        }
    }
}
