using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_SkillCell:BaseClassMono
{
    private string m_Path;
    private Image m_Img;
    private Button m_Btn;
    private Text m_CountTxt;
    private Text m_KeyTxt;

    private SkillBase m_SkillBase;

    private Action<UI_SkillCell> m_BtnClickEvent;

    public bool isTool;
    public bool isReleaseSkill;
    public int count;
    private float timer;

    protected override void Awake()
    {
        base.Awake();
        m_Img = GetComponent<Image>();
        m_Btn = GetComponent<Button>();
        m_CountTxt = transform.Find("CountTxt").GetComponent<Text>();
        m_KeyTxt = transform.Find("KeyTxt").GetComponent<Text>();

        m_Btn.onClick.AddListener(OnBtnClick);
    }

    public void Initaialize(string inPath,SkillBase inSkill,Sprite inSprite,Action<UI_SkillCell> inBtnAction,string inKey,int inCount
        ,bool inTool = false,bool inReleaseSkill = false)
    {
        m_SkillBase = inSkill;
        m_Img.sprite = inSprite;
        m_BtnClickEvent = inBtnAction;
        isTool = inTool;
        isReleaseSkill = inReleaseSkill;
        m_KeyTxt.text = inKey;
        m_Path = inPath;

        m_CountTxt.gameObject.SetActive(isTool);
        m_KeyTxt.gameObject.SetActive(!isReleaseSkill);
        timer -= Time.deltaTime;
    }
    private void Update()
    {
        //当成技能状态使用时
        if (gameObject.activeInHierarchy && isReleaseSkill)
        {
            timer -= Time.deltaTime;
            m_CountTxt.text = ((int)timer).ToString();
            if (timer <= 0)
            {
                GameObjectPool.Instance.UnSpawn(gameObject,m_Path);
            }
        }
    }

    public void SetCount(int inCount)
    {
        count = inCount;
        m_CountTxt.text = count.ToString();
    }

    private void OnBtnClick()
    {
        if(m_BtnClickEvent != null)
            m_BtnClickEvent(this);
    }
}