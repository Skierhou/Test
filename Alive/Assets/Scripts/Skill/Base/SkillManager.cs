using System;
using System.Collections.Generic;

[Config]
public class SkillManager:Singleton<SkillManager>
{
    private Dictionary<ESkillType, Type> m_AllSkillDict;

    private Dictionary<ESkillType, List<SkillBase>> m_SkillDict = new Dictionary<ESkillType, List<SkillBase>>();

    public override void Initialize()
    {
        m_AllSkillDict = new Dictionary<ESkillType, Type>();
        m_AllSkillDict.Add(ESkillType.SmallBloodDrug,typeof(RS_SmallBloodDrug));
        m_AllSkillDict.Add(ESkillType.BigBloodDrug,typeof(RS_BigBloodDrug));
        m_AllSkillDict.Add(ESkillType.SmallMagicDrug, typeof(RS_SmallMagicDrug));
        m_AllSkillDict.Add(ESkillType.BigMagicDrug, typeof(RS_BigMagicDrug));
    }

    public void ReleaseSkill(Pawn inOwner,Pawn inTarget,ESkillType inSkillType)
    {
        List<SkillBase> list = null;
        if (!m_SkillDict.TryGetValue(inSkillType, out list) || list == null)
        {
            list = new List<SkillBase>();
            m_SkillDict.Add(inSkillType, list);
        }

        SkillBase canUseSkill = list.Find((skill) => skill.canUse);

        if (canUseSkill == null)
        {
            Type type;
            if (m_AllSkillDict.TryGetValue(inSkillType, out type))
            {
                canUseSkill = (SkillBase)Activator.CreateInstance(type);
                if (canUseSkill != null)
                    list.Add(canUseSkill);
            }
        }

        if (canUseSkill != null)
        {
            canUseSkill.ReleaseSkill(inOwner, inTarget);
        }
    }
}