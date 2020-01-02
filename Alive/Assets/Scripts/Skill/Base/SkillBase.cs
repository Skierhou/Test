using System;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;

[Config]
public abstract class SkillBase : BaseClass
{
    [Config]
    public EReleaseType CFG_ReleaseType;
    [Config]
    public int CFG_Id;
    [Config]
    public string CFG_IconPath;
    [Config]
    public string CFG_Name;


    [Config]
    public int CFG_Damage;          //造成伤害
    [Config]
    public int CFG_DamageCount;     //伤害次数
    [Config]
    public int CFG_DamageInterval;  //伤害间隔

    private Coroutine m_Coroutine;

    protected float skillTimer;

    public bool canUse;

    public SkillBase():base()
    {
        Initialize();
    }

    public virtual void Initialize()
    {
        
    }

    public virtual void ReleaseSkill(Pawn inOwner, Pawn inTarget)
    {
        m_Coroutine = inOwner.StartCoroutine(SkillState(inOwner, inTarget));
    }

    public virtual IEnumerator SkillState(Pawn inOwner, Pawn inTarget)
    {
        yield return null;
    }

    public virtual int AddMoney(int inMoney)
    {
        return 0;
    }

    public virtual String GetDescription()
    {
        return "";
    }

    public virtual void OnRemove(Pawn inOwner)
    {
        if(m_Coroutine != null)
            inOwner.StopCoroutine(m_Coroutine);
        inOwner.ExistSkillList.Remove(this);
    }
}