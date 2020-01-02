using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Config]
public class BuffSkillBase : SkillBase
{
    [Config]
    public int CFG_AddHp;               //血量增益
    [Config]
    public int CFG_AddMp;               //魔法增益
    [Config]
    public int CFG_AddAttack;           //攻击增益
    [Config]
    public float CFG_AddSpeed;          //移速增益
    [Config]
    public float CFG_AddEndurance;      //耐力增益
    [Config]
    public float CFG_Duration;          //持续时间

    [Config]
    public String FXTemplatePath;

    public override void Initialize()
    {
        base.Initialize();
        canUse = true;
    }

    public override IEnumerator SkillState(Pawn inOwner, Pawn inTarget)
    {
        canUse = false;
        inTarget.AddMaxHp(CFG_AddHp);
        inTarget.AddMaxMp(CFG_AddMp);
        inTarget.AddMaxAttack(CFG_AddAttack);
        inTarget.AddMaxMoveSpeed(CFG_AddSpeed);
        inTarget.AddEnduranceMax(CFG_AddEndurance);

        GameObject fxGo = GameObject.Instantiate(Resources.Load<GameObject>(FXTemplatePath), inTarget.transform);
        fxGo.transform.localPosition = Vector3.zero;
        fxGo.transform.localEulerAngles = Vector3.zero;
        skillTimer = Time.time;
        while (true)
        {
            if (Time.time - skillTimer >= CFG_Duration || inTarget == null || inTarget.hp <= 0)
            {
                break;
            }
            yield return null;
        }
        inTarget.AddMaxHp(-CFG_AddHp);
        inTarget.AddMaxMp(-CFG_AddMp);
        inTarget.AddMaxAttack(-CFG_AddAttack);
        inTarget.AddMaxMoveSpeed(-CFG_AddSpeed);
        inTarget.AddEnduranceMax(-CFG_AddEndurance);

        yield return null;

        GameObject.Destroy(fxGo);
        canUse = true;
    }

    public override string GetDescription()
    {
        String str = "Buff获得以下增益效果：";
        if (CFG_AddHp > 0)
            str += "+Hp上限:" + CFG_AddHp;
        if (CFG_AddMp > 0)
            str += "+Mp上限:" + CFG_AddMp;
        if (CFG_AddAttack > 0)
            str += "+攻击力:" + CFG_AddAttack;
        if (CFG_AddSpeed > 0)
            str += "+移速:" + CFG_AddSpeed;
        if (CFG_AddEndurance > 0)
            str += "+耐力:" + CFG_AddEndurance;
        str += "\\n持续时间：" + CFG_Duration;
        return str;
    }
}