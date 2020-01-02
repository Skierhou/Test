using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainPanel:PanelBase
{
    private Button m_StartBtn;

    protected override void Awake()
    {
        base.Awake();

        m_StartBtn = transform.Find("StartBtn").GetComponent<Button>();

        m_StartBtn.onClick.AddListener(OnStartBtnClick);
    }

    public override void OnPush()
    {
        gameObject.SetActive(true);
    }
    public override void OnPop()
    {
        gameObject.SetActive(false);
    }

    private void OnStartBtnClick()
    {
        UIManager.Instance.PopPanel();
        UIManager.Instance.PushPanelAsync(EPanelType.GamePanel);
    }
}
