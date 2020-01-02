using System;
using System.Collections.Generic;
using UnityEngine;

[Config]
class SpecialMove : BaseClass
{
    //是否能被打断
    private bool m_CanBeOverload;
    //当前动画名
    protected string[] m_AnimNameList;
    protected int m_Index;
    //随机播放动画
    protected bool m_RandomPlay;
    protected bool m_UseAnimation;

    protected AICmdAction m_AIOwner;
    protected Animation m_Animation;
    protected Animator m_Animator;

    public bool CanBeOverload { get => m_CanBeOverload;  }

    public static SpecialMove InitSpecialMove(AICmdAction inAIOwner, string[] inAnimNameList, bool inUseAnimation = true
        ,bool inCanBeOverload = false,bool inRandomPlay = false)
    {
        SpecialMove SpecialMove = new SpecialMove
        {
            m_AIOwner = inAIOwner,
            m_AnimNameList = inAnimNameList,
            m_RandomPlay = inRandomPlay,
            m_CanBeOverload = inCanBeOverload,
            m_Animation = inAIOwner.Animation,
            m_Animator = inAIOwner.Animator,
            m_UseAnimation = inUseAnimation,
            m_Index = 0,
        };

        inAIOwner.PushSpecialMove(SpecialMove);
        return SpecialMove;
    }

    public virtual void OnPush()
    {
        
    }
    public virtual void OnPop()
    {
        
    }

    public virtual void Update()
    {
        if (m_AnimNameList.Length > m_Index)
        {
            if (!String.IsNullOrEmpty(m_AnimNameList[m_Index]))
            {
                if (m_UseAnimation)
                {
                    UseAnimation();
                }
                else
                {
                    UseAnimator();
                }
            }
            else
            {
                m_Index++;
            }
        }
        else
        {
            m_AIOwner.PopSpecialMove();
        }
    }
    /// <summary>
    /// 动画
    /// </summary>
    protected virtual void UseAnimation()
    {
        if (m_Animation != null)
        {
            if (m_Animation.IsPlaying(m_AnimNameList[m_Index]))
                return;

            m_Index++;
            if (m_AnimNameList.Length > m_Index && m_Animation.GetClip(m_AnimNameList[m_Index]) != null)
            {
                m_Animation.Play(m_AnimNameList[m_Index]);
            }
            else
            {
                m_Index++;
            }
        }
        else
        {
            m_AIOwner.PopSpecialMove();
        }
    }

    /// <summary>
    /// 动画状态机
    /// </summary>
    protected virtual void UseAnimator()
    {
        if (m_Animator != null)
        {
            if (!m_Animator.GetCurrentAnimatorStateInfo(0).IsName(m_AnimNameList[m_Index]))
            {
                m_Animator.SetTrigger(m_AnimNameList[m_Index]);
            }
        }
        else
        {
            m_AIOwner.PopSpecialMove();
        }
    }
}