using System;
using System.Collections;
using System.Collections.Generic;

[Config]
public class RecoverySkill : SkillBase
{
    [Config]
    public int CFG_AddHp;               //血量增益
    [Config]
    public int CFG_AddMp;               //魔法增益

    public override void Initialize()
    {
        canUse = true;
    }

    public override void ReleaseSkill(Pawn inOwner, Pawn inTarget)
    {
        inTarget.AddHealth(CFG_AddHp);
        inTarget.AddMp(CFG_AddMp);
    }
}

[Config]
public class RS_SmallBloodDrug : RecoverySkill { }
[Config]
public class RS_SmallMagicDrug : RecoverySkill { }
[Config]
public class RS_BigBloodDrug : RecoverySkill { }
[Config]
public class RS_BigMagicDrug : RecoverySkill { }