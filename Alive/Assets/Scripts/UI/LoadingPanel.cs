using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingPanel:PanelBase
{
    private Slider m_ProgressSlider;
    private Text m_ProgressTxt;
    private Text m_LoadTxt;
    private Text m_InfoTxt;

    protected override void Awake()
    {
        base.Awake();
        m_ProgressSlider = GetComponentInChildren<Slider>();
        m_ProgressTxt = transform.Find("Slider/Text").GetComponent<Text>();
        m_LoadTxt = transform.Find("LoadText").GetComponent<Text>();
        m_InfoTxt = transform.Find("InfoText").GetComponent<Text>();
    }

    public override void OnPush()
    {
        gameObject.SetActive(true);
        m_ProgressSlider.value = 0;
        StartCoroutine(UpdateLoadingTxt());
    }
    public override void OnPop()
    {
        StopAllCoroutines();
        gameObject.SetActive(false);
    }

    IEnumerator UpdateLoadingTxt()
    {
        string loading = "加载中";
        string dotStr = "";
        int count = 0;
        while (true)
        {
            m_LoadTxt.text = loading + dotStr;
            count++;
            count %= 4;
            dotStr = "";
            for (int i = 0; i < count; i++)
            {
                dotStr += ".";
            }
            yield return new WaitForSeconds(0.5f);
        }
    }

    public void SetLoadingValue(float inLoading)
    {
        m_ProgressSlider.value = inLoading;
        m_ProgressTxt.text = (inLoading * 100).ToString("f0") + "%";
    }

    public void SetLoadingInfo(string inInfo)
    {
        m_InfoTxt.text = inInfo;
    }
}