using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public class GamePanel:PanelBase
{
    //人物属性栏
    private Image m_HeadImg;
    private Slider m_HpSlider;
    private Text m_HpTxt;
    private Slider m_MpSlider;
    private Text m_MpTxt;
    private Slider m_EnduranceSlider;
    private Text m_EnduranceTxt;

    //技能状态栏
    private Transform m_SkillStateGrid;

    //道具栏
    private Transform m_ToolGrid;
    private List<DragGrid> m_DragGridList;

    //设置框

    //游戏信息
    private PlayerPawn player;

    private Transform hpUIGrid;
    

    protected override void Awake()
    {
        base.Awake();

        m_SkillStateGrid = transform.Find("SkillStateGrid");
        m_ToolGrid = transform.Find("Tool/ToolGrid");
        m_HeadImg = transform.Find("Head/HeadImg").GetComponent<Image>();
        m_HpSlider = transform.Find("Head/HpBg/HpSlider").GetComponent<Slider>();
        m_MpSlider = transform.Find("Head/MpBg/MpSlider").GetComponent<Slider>();
        m_EnduranceSlider = transform.Find("Head/EnduranceBg/EnduranceSlider").GetComponent<Slider>();
        m_HpTxt = m_HpSlider.transform.GetComponentInChildren<Text>();
        m_MpTxt = m_MpSlider.transform.GetComponentInChildren<Text>();
        m_EnduranceTxt = m_EnduranceSlider.transform.GetComponentInChildren<Text>();
        hpUIGrid = transform.Find("HpUIGrid");

        UIManager.Instance.HpUIGird = hpUIGrid;
    }

    public override void OnPush()
    {
        base.OnPush();
        ToolsManager.Instance.gamePanel = this;

        m_DragGridList = m_ToolGrid.GetComponentsInChildren<DragGrid>().ToList();
        for (int i = 0; i < m_DragGridList.Count; i++)
        {
            int key = i + 49;
            m_DragGridList[i].Initialize((KeyCode)key, (i + 1).ToString());
        }
    }

    private void Update()
    {
        UpdateHeadInfo();
    }

    /// <summary>
    /// ToolManager通知UI改变
    /// </summary>
    public bool GetTool(EToolType inToolType, int inCount, string inIconPath)
    {
        if (inToolType == EToolType.None) return false;

        DragGrid dragGrid = m_DragGridList.Find((grid)=> grid.toolType == inToolType);
        if (dragGrid != null)
        {
            dragGrid.AddCount(inCount);
        }
        else
        {
            dragGrid = m_DragGridList.Find((grid) => grid.toolType == EToolType.None);
            if (dragGrid != null)
                dragGrid.Change(inToolType, inCount, inIconPath);
            else
            {
                //获得道具失败
                return false;
            }
        }
        return true;
    }
    public void UseTool(EToolType inToolType)
    {
        DragGrid dragGrid = m_DragGridList.Find((grid) => grid.toolType == inToolType);
        if (dragGrid != null)
        {
            dragGrid.AddCount(-1);
        }
    }

    public void UpdateHeadInfo()
    {
        if(player == null)
            player = CharacterManager.Instance.Player.GetComponent<PlayerPawn>();

        m_HpTxt.text = player.hp + "/" + player.HealthMax;
        m_HpSlider.value = player.hp / player.HealthMax;
        m_MpTxt.text = player.mp + "/" + player.MagicMax;
        m_MpSlider.value = player.mp / player.MagicMax;
        m_EnduranceTxt.text = player.endurance + "/" + player.EnduranceMax;
        m_EnduranceSlider.value = player.endurance / player.EnduranceMax;
    }
    public override void OnPop()
    {
        base.OnPop();
    }
}
