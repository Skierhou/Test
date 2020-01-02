using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public enum EPanelType
{
    MainPanel,
    GamePanel,
    SettingPanel,
    LoadingPanel,
    SkillPanel,
    SkillTipPanel,
    ToolPanel,
}

public class PanelBase:BaseClassMono
{
    //Loading图异步加载的加载进度
    public float LoadingRate;
    //Loading图异步加载的信息
    public string LoadingInfo;

    public void StartLoading()
    {
        LoadingRate = 0;
        StartCoroutine(OnStartLoading());
    }
    protected virtual IEnumerator OnStartLoading()
    {
        while (LoadingRate < 1.0f)
        {
            LoadingRate += Time.deltaTime;
            LoadingRate = LoadingRate >= 1.0f ? 1.0f : LoadingRate;
            yield return null;
        }
        yield return null;
    }
    public virtual void OnPush()
    {

    }
    public virtual void OnPop()
    {

    }
}
