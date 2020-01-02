using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DragGrid : MonoBehaviour
{
    public KeyCode key;
    public int count;
    public EToolType toolType;

    private Button m_Btn;
    private Image m_ChildImg;
    private Text m_CountTxt;
    private Text m_KeyTxt;

    public Sprite Sprite { get => m_ChildImg.sprite; }

    private void Awake()
    {
        m_Btn = GetComponent<Button>();
        m_ChildImg = transform.Find("Child").GetComponent<Image>();
        m_KeyTxt = transform.Find("KeyTxt").GetComponent<Text>();
        m_CountTxt = transform.Find("CountTxt").GetComponent<Text>();
    }

    public void Initialize(KeyCode inKey,string inKeyName)
    {
        key = inKey;
        m_KeyTxt.text = inKeyName;
        ChildActive(false);
    }

    private void Update()
    {
        if (key != KeyCode.None && Input.GetKeyDown(key) && count > 0 && toolType != EToolType.None)
        {
            ToolsManager.Instance.UseTool(CharacterManager.Instance.Player, CharacterManager.Instance.Player, toolType);
        }
    }

    public void Change(EToolType inToolType, int inCount, string inIconPath)
    {
        toolType = inToolType;
        count = inCount;
        m_ChildImg.sprite = Tools.CreateSprite(inIconPath);
        m_CountTxt.text = count.ToString();
        ChildActive(true);
    }
    public void Change(DragData inData)
    {
        toolType = inData.toolType;
        count = inData.count;
        m_ChildImg.sprite = inData.sprite;
        m_CountTxt.text = count.ToString();
        ChildActive(true);
    }

    public void ChildActive(bool inEnable)
    {
        m_ChildImg.enabled = inEnable;
        m_CountTxt.enabled = inEnable;
    }

    public void StartDrag()
    {
        if (!DragManager.Instance.bDrag)
        {
            if (toolType != EToolType.None && count > 0)
            {
                //Vector2 screenPoint = Camera.main.WorldToScreenPoint(Input.mousePosition);
                //screenPoint.x -= Screen.width / 2;
                //screenPoint.y -= Screen.height / 2;

                //Vector2 offset = screenPoint - ((RectTransform)transform).rect.position;
                DragManager.Instance.SetDrag(((RectTransform)transform).sizeDelta, m_ChildImg.sprite, Vector2.zero, this);
                ChildActive(false);
            }
        }
    }

    public void AddCount(int inCount)
    {
        count += inCount;
        m_CountTxt.text = count.ToString();

        if (count <= 0)
        {
            ChildActive(false);
        }
    }
}
