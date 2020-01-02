using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_EnemyHp : MonoBehaviour
{
    private Slider m_HpSlider;              //血条
    private Slider m_HardValueSlider;       //硬值条

    private AIPawn m_Target;
    private bool bStartRecovery;

    private void Awake()
    {
        m_HpSlider = transform.Find("HpSlider").GetComponent<Slider>();
        m_HardValueSlider = transform.Find("HardValueSlider").GetComponent<Slider>();
    }

    public void Initialize(AIPawn inTarget)
    {
        m_Target = inTarget;
        if (UIManager.Instance.HpUIGird != null)
        {
            transform.SetParent(UIManager.Instance.HpUIGird);
            transform.localScale = Vector3.one;
        }
    }
    private void Update()
    {
        if (m_Target != null)
        {
            Vector2 _pos = Vector2.one;

            Vector3 screenPos = Camera.main.WorldToScreenPoint(m_Target.transform.position);
            screenPos.y += m_Target.CFG_HpUIOffset;

            RectTransformUtility.ScreenPointToLocalPointInRectangle(UIManager.Instance.Canvas.transform as RectTransform
                , screenPos, UIManager.Instance.Canvas.worldCamera, out _pos);
            transform.localPosition = _pos;

            ChangeHp(m_Target.hp, m_Target.HealthMax);
            if(!bStartRecovery)
                ChangeHardValue(m_Target.hardValue, m_Target.CFG_HardValue);
        }
    }

    private void ChangeHp(float inHp, float inMaxHp)
    {
        m_HpSlider.value = inHp / inMaxHp;
    }
    private void ChangeHardValue(float invalue, float inMaxValue)
    {
        m_HardValueSlider.value = invalue / inMaxValue;
    }

    public void RecoveryHardValue()
    {
        bStartRecovery = true;
        StartCoroutine(StartRecovery());
    }
    IEnumerator StartRecovery()
    {
        float value = 0;
        while (true)
        {
            value += Time.deltaTime * 3;
            m_HardValueSlider.value = Mathf.Min(1, value / m_Target.CFG_HardValue);
            if (value >= m_Target.CFG_HardValue)
            {
                break;
            }
            yield return null;
        }
        bStartRecovery = false;
        yield return null;
    }
}
