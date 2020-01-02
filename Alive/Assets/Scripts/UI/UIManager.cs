using System;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;

[Config]
public class UIManager:MonoSingleton<UIManager>
{
    [Config]
    public List<string> CFG_PanelsPathList;

    private Stack<PanelBase> panels = new Stack<PanelBase>();
    private Dictionary<EPanelType, PanelBase> panelsDict = new Dictionary<EPanelType, PanelBase>();

    private void Awake()
    {
        this.ReadConfig();
    }

    public void PushPanel(EPanelType inPanelType)
    {
        PanelBase panel;
        if (panelsDict.TryGetValue(inPanelType, out panel) && panel != null)
        {
            panel.transform.SetAsLastSibling();
            panel.OnPush();
        }
        else
        {
            panel = CreatePanel(inPanelType);
        }
        if(panel != null)
            panels.Push(panel);
    }

    private PanelBase CreatePanel(EPanelType inPanelType)
    {
        PanelBase panel;
        panel = GameObject.Instantiate(Resources.Load<GameObject>(CFG_PanelsPathList[(int)inPanelType]), transform).GetComponent<PanelBase>();
        panel.transform.localPosition = Vector3.one;
        panel.transform.localScale = Vector3.one;
        panelsDict.Add(inPanelType, panel);

        return panel;
    }

    public void PushPanelAsync(EPanelType inPanelType)
    {
        PanelBase panel;
        PanelBase loadingPanel;

        if (!panelsDict.TryGetValue(inPanelType, out panel) || panel != null)
        {
            panel = CreatePanel(inPanelType);
        }
        panel.StartLoading();

        if (!panelsDict.TryGetValue(EPanelType.LoadingPanel, out loadingPanel) || loadingPanel != null)
        {
            loadingPanel = CreatePanel(EPanelType.LoadingPanel);
        }
        loadingPanel.transform.SetAsLastSibling();
        loadingPanel.OnPush();
        panels.Push(loadingPanel);
        StartCoroutine(StartAsyncLoading(panel, (LoadingPanel)loadingPanel));
    }
    IEnumerator StartAsyncLoading(PanelBase inPanel, LoadingPanel inLoadingPanel)
    {
        float loadingRate = 0;
        while (true)
        {
            loadingRate = inPanel.LoadingRate >= 1.00f ? 1.00f : inPanel.LoadingRate;
            inLoadingPanel.SetLoadingValue(inPanel.LoadingRate);
            inLoadingPanel.SetLoadingInfo(inPanel.LoadingInfo);

            if (loadingRate >= 1)
            {
                PopPanel();
                inPanel.transform.SetAsLastSibling();
                inPanel.OnPush();
                panels.Push(inPanel);
                break;
            }
            yield return null;
        }
        yield return null;
    }

    public void PopPanel()
    {
        if (panels.Count > 0)
        {
            PanelBase panel = panels.Peek();
            panel.OnPop();
            panels.Pop();
        }
    }
}
