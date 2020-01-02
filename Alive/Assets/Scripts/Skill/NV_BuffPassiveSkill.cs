using System;
using System.Collections;
using System.Collections.Generic;

[Config]
class BuffPassiveSkill : SkillBase
{
    [Config]
    public int CFG_AddHp;               //血量增益
    [Config]
    public int CFG_AddMp;               //魔法增益
    [Config]
    public float CFG_AddEndurance;      //耐力增益
    [Config]
    public int CFG_AddAttack;           //攻击增益
    [Config]
    public float CFG_AddSpeed;          //移速增益

    public override void Initialize()
    {
        base.Initialize();
    }
    public override void ReleaseSkill(Pawn inOwner, Pawn inTarget)
    {
        canUse = false;
        inTarget.AddMaxHp(CFG_AddHp);
        inTarget.AddMaxMp(CFG_AddMp);
        inTarget.AddEnduranceMax(CFG_AddEndurance);
        inTarget.AddMaxAttack(CFG_AddAttack);
        inTarget.AddMaxMoveSpeed(CFG_AddSpeed);
    }
    public override void OnRemove(Pawn inOwner)
    {
        canUse = true;
        inOwner.AddMaxHp(-CFG_AddHp);
        inOwner.AddMaxMp(-CFG_AddMp);
        inOwner.AddEnduranceMax(-CFG_AddEndurance);
        inOwner.AddMaxAttack(-CFG_AddAttack);
        inOwner.AddMaxMoveSpeed(-CFG_AddSpeed);
    }
    public override string GetDescription()
    {
        String str = "获得以下增益效果：";
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
        return str;
    }
}