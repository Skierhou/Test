using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// AI的总行为管理
/// </summary>
public class AICmdAction
{
    // AICmd
    private Stack<AICmd> m_AICmdStack = new Stack<AICmd>();

    private AICmd m_CurrentAICmd;
    private AIPawn m_Owner;
    private Coroutine m_Coroutine;

    //SpecialMove
    private Animation m_Animation;
    private Animator m_Animator;

    private Stack<SpecialMove> m_SpecialMoveStack = new Stack<SpecialMove>();

    private SpecialMove m_CurrentSpecialMove;

    //属性封装
    public AICmd CurrentAICmd { get => m_CurrentAICmd; private set => m_CurrentAICmd = value; }
    public AIPawn Owner { get => m_Owner; private set => m_Owner = value; }
    public Animation Animation { get => m_Animation; private set => m_Animation = value; }
    public Animator Animator { get => m_Animator; private set => m_Animator = value; }

    public AICmdAction(AIPawn inOwner)
    {
        if (inOwner == null)
            Debug.LogWarning("AICmdAction传入Pawn为空！");

        Owner = inOwner;
        m_Animation = inOwner.GetComponent<Animation>();
        m_Animator = inOwner.GetComponent<Animator>();
        m_Coroutine = Owner.StartCoroutine(Start());
    }

    private IEnumerator Start()
    {
        while (true)
        {
            if (CurrentAICmd != null)
            {
                CurrentAICmd.Update();
            }
            if (m_CurrentSpecialMove != null)
            {
                m_CurrentSpecialMove.Update();
            }
            else
            {
                if (m_SpecialMoveStack.Count >= 1)
                {
                    m_CurrentSpecialMove = m_SpecialMoveStack.Pop();
                }
            }
            yield return 0;
        }
    }

    internal void PushSpecialMove(SpecialMove inSpecialMove)
    {
        if (inSpecialMove != null)
        {
            if (m_CurrentSpecialMove != null && !m_CurrentSpecialMove.CanBeOverload)
            {
                m_SpecialMoveStack.Push(inSpecialMove);
            }
            else
            {
                m_CurrentSpecialMove.OnPop();
                m_CurrentSpecialMove = inSpecialMove;
                m_CurrentSpecialMove.OnPush();
            }
        }
    }
    public void PopSpecialMove()
    {
        m_CurrentSpecialMove = null;
        if (m_SpecialMoveStack.Count >= 1)
        {
            m_CurrentSpecialMove = m_SpecialMoveStack.Pop();
        }
    }

    public void PopAICmd()
    {
        if (m_AICmdStack.Count >= 1)
        {
            CurrentAICmd.Poped();
            m_AICmdStack.Pop();
            if (m_AICmdStack.Count >= 1)
            {
                CurrentAICmd = m_AICmdStack.Peek();
            }
            else
            {
                CurrentAICmd = null;
            }
        }
    }

    public void PushAICmd(AICmd inAICmd)
    {
        if (inAICmd != null)
        {
            PopAICmd();
            m_AICmdStack.Push(inAICmd);
            CurrentAICmd = inAICmd;
            CurrentAICmd.Pushed();
        }
    }

    public void OnDestroy()
    {
        while (m_AICmdStack.Count >= 1)
        {
            CurrentAICmd = m_AICmdStack.Peek();
            CurrentAICmd.Poped();
            m_AICmdStack.Pop();
        }
        m_AICmdStack.Clear();
        CurrentAICmd = null;
    }
}