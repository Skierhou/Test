using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Config]
public class Pawn : BaseClassMono
{
    //配置的默认数值
    [Config]
    public int CFG_Id;
    [Config]
    public int CFG_HpMax;
    [Config]
    public int CFG_MpMax;
    [Config]
    public int CFG_Attack;
    [Config]
    public int CFG_AttackInterval;
    [Config]
    public int CFG_Speed;
    [Config]
    public float CFG_AttackRange;
    [Config]
    public float CFG_Endurance;

    //临时数值
    public int hp;
    public int mp;
    public float endurance;

    //数值的上限(额外加成的数值)
    public float moveSpeedLimit;
    public int hpLimit;
    public int mpLimit;
    public int attackLimit;
    public float enduranceLimit;

    //外部使用的属性
    public int HealthMax { get { return hpLimit + CFG_HpMax; } }
    public int MagicMax { get { return mpLimit + CFG_MpMax; } }
    public float MoveSpeed { get { return CFG_Speed + moveSpeedLimit; } }
    public int Attack { get { return CFG_Attack + attackLimit; } }
    public float EnduranceMax { get { return enduranceLimit + CFG_Endurance; } }

    //拥有的技能
    public List<SkillBase> PossessSkillList = new List<SkillBase>();
    //自身当前被释放的技能效果
    public List<SkillBase> ExistSkillList = new List<SkillBase>();

    protected override void Awake()
    {
        base.Awake();
        Initialize();
    }

    public virtual void Initialize()
    {
        hp = CFG_HpMax + hpLimit;
        hpLimit = 0;
        moveSpeedLimit = 0;
        mpLimit = 0;
    }

    public virtual void TakeDamage(int inDamage, Vector3 inMovement, Pawn inCauser)
    {
        if (inDamage < 0)
            return;

        hp -= inDamage;
        transform.position += inMovement;
    }

    /// <summary>
    /// 回蓝
    /// </summary>
    public void AddMp(int addMp)
    {
        int tAddMp = Mathf.Min(CFG_MpMax + mpLimit - mp, addMp);
        mp += tAddMp;

        if (mp < 0)
            mp = 0;
    }
    /// <summary>
    /// 增加Mp上限
    /// </summary>
    public void AddMaxMp(int addMp)
    {
        mpLimit += addMp;
        mp = addMp >= 0 ? mp + addMp : mp;
        if (MagicMax < mp)
            mp = MagicMax;
    }
    /// <summary>
    /// 回血
    /// </summary>
    public void AddHealth(int addHp)
    {
        int tAddHp = Mathf.Min(CFG_HpMax + hpLimit - hp, addHp);
        hp += tAddHp;

        if (hp < 0)
            hp = 0;
    }
    /// <summary>
    /// 增加Hp上限
    /// </summary>
    public void AddMaxHp(int addHp)
    {
        hpLimit += addHp;
        hp = addHp >= 0 ? hp + addHp : hp;
        if (HealthMax < hp)
            hp = HealthMax;
    }
    /// <summary>
    /// 加攻
    /// </summary>
    public void AddMaxAttack(int addAttack)
    {
        attackLimit += addAttack;
        if (Attack < 0)
            attackLimit = - CFG_Attack;
    }
    /// <summary>
    /// 加移速
    /// </summary>
    public void AddMaxMoveSpeed(float addSpeed)
    {
        moveSpeedLimit += addSpeed;
        if (MoveSpeed < 0)
            moveSpeedLimit = - CFG_Speed;
    }
    /// <summary>
    /// 加耐力上限
    /// </summary>
    public void AddEnduranceMax(float addEndurance)
    {
        enduranceLimit += addEndurance;

        endurance = addEndurance >= 0 ? endurance + addEndurance : endurance;
        if (endurance < 0)
            endurance = 0;
        if (EnduranceMax < 0)
            enduranceLimit = -enduranceLimit;
    }

}
