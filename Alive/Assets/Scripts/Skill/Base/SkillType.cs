
public enum EReleaseType
{
    Acitve,             //主动
    AttackPassive,      //普攻被动
    BuffPassive,        //Buff被动
}

/// <summary>
/// 技能类型：NV前缀是直对数值做修改，PS前缀为被动，AS前缀为主动
/// </summary>
public enum ESkillType
{
    None,
    SmallBloodDrug,     //小血药
    BigBloodDrug,       //大血药
    SmallMagicDrug,     //小蓝药
    BigMagicDrug,       //大蓝药
}
