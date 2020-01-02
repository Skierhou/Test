using System;
using UnityEngine;
using System.Collections.Generic;

public class ToolsManager:Singleton<ToolsManager>
{
    private Dictionary<EToolType, ToolBase> m_ToolDict = new Dictionary<EToolType, ToolBase>();
    private List<ToolBase> m_ToolList = new List<ToolBase>();
    private List<GameObject> m_ToolGo = new List<GameObject>();

    public GamePanel gamePanel;

    public override void Initialize()
    {
        m_ToolDict.Add(EToolType.SmallBloodDrug,new SmallBloodDrug());
        m_ToolDict.Add(EToolType.BigBloodDrug,new BigBloodDrug());
        m_ToolDict.Add(EToolType.SmallMagicDrug,new SmallMagicDrug());
        m_ToolDict.Add(EToolType.BigMagicDrug,new BigMagicDrug());
    }

    public void UseTool(Pawn inOwner,Pawn inTarget,EToolType inToolType)
    {
        ToolBase tool;
        if (!m_ToolDict.TryGetValue(inToolType, out tool) || tool == null)
        {
            Debug.LogWarning("不存在道具类型:" + inToolType);
            return;
        }
        tool.UseSkill(inOwner,inTarget);
        gamePanel.UseTool(inToolType);
    }

    /// <summary>
    /// 玩家获得道具
    /// </summary>
    public void GetTool(EToolType inToolType)
    {
        ToolBase tool = m_ToolDict.TryGetValueByKey(inToolType);
        if (tool != null)
        {
            ++tool.count;
            gamePanel.GetTool(tool.toolType, 1, tool.CFG_IconPath);
            if (!m_ToolList.Contains(tool))
                m_ToolList.Add(tool);
        }
    }

    public GameObject CreateTool(EToolType inToolType,Vector3 inLoc)
    {
        GameObject go = null;
        ToolBase tool = m_ToolDict.TryGetValueByKey(inToolType);
        if (tool != null)
        {
            go = tool.CreateTool(inLoc);
            m_ToolGo.Add(go);
        }
        return go;
    }
}
