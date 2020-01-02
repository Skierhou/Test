using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace BehaviorDesigner.Runtime.Tasks.Basic.UnityAnimation
{
    [TaskCategory("Basic/AIPawn")]
    [TaskDescription("Plays animation without any blending. Returns Success.")]
    public class TraceTarget : Action
    {
        AIPawn m_AIPawn;
        Animation m_Anim;

        public override void OnStart()
        {
            if(m_AIPawn == null)
                m_AIPawn = GetComponent<AIPawn>();
            if (m_Anim == null)
                m_Anim = GetComponent<Animation>();
        }

        public override TaskStatus OnUpdate()
        {
            Vector3 tLoc = CharacterManager.Instance.Player.transform.position;
            if (Vector3.Distance(tLoc, transform.position) >= m_AIPawn.CFG_AttackRange)
            {
                m_AIPawn.StartAStarFind(tLoc);
                return TaskStatus.Success;
            }
            else if(Vector3.Distance(tLoc, transform.position) <= m_AIPawn.CFG_AttackRange - 0.5f)
            {
                m_AIPawn.StopAStarFind();
                return TaskStatus.Failure;
            }
            return TaskStatus.Success;
        }
        public override void OnEnd()
        {
            
        }

        public override void OnReset()
        {
            
        }
    }
}