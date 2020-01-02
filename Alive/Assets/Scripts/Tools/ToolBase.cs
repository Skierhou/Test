using System;
using UnityEngine;
using System.Collections.Generic;

[Config]
public class ToolBase:BaseClass
{
    [Config]
    public int CFG_Id;                  //这个Id的配置需要配合EToolType枚举
    [Config]
    public int CFG_SkillId;             //对应的技能ID
    [Config]
    public string CFG_PlaceGoPath;      //场地内放置道具的物体路径
    [Config]
    public string CFG_IconPath;          //图标

    public EToolType toolType;
    public ESkillType skillType;

    public ToolBase() : base()
    {
        toolType = (EToolType)CFG_Id;
        skillType = (ESkillType)CFG_SkillId;
    }

    //临时数据
    public int count;

    public void UseSkill(Pawn inOwner, Pawn inTarget)
    {
        if (count <= 0) return;

        SkillManager.Instance.ReleaseSkill(inOwner, inTarget, skillType);
        count--;
    }

    public GameObject CreateTool(Vector3 inLoc)
    {
        GameObject go = GameObjectPool.Instance.Spawn(CFG_PlaceGoPath, inLoc, Quaternion.identity);
        if (go != null)
        {
            PlaceTool placeTool = go.GetComponent<PlaceTool>();
            if (placeTool != null)
                placeTool.Initialize(toolType, CFG_PlaceGoPath);
        }
        return go;
    }
}