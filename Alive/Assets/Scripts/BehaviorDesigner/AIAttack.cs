using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Basic.UnityAnimation
{
    [TaskCategory("Basic/AIPawn")]
    [TaskDescription("Plays animation without any blending. Returns Success.")]
    public class AIAttack : Action
    {
        private bool haveAttack;
        [Tooltip("AttackAnim")]
        public SharedString AttackAnim;

        private Animation m_Anim;
        private AIPawn m_AIPawn;

        public override void OnStart()
        {
            if (m_Anim == null)
                m_Anim = GetComponent<Animation>();
            if (m_AIPawn == null)
                m_AIPawn = GetComponent<AIPawn>();

            if(m_Anim != null)
                Attack();
        }

        public override TaskStatus OnUpdate()
        {
            return TaskStatus.Success;
        }

        private void Attack()
        {
            if (m_Anim.IsPlaying((string)AttackAnim.GetValue()))
                return;

            if (m_Anim.clip.wrapMode == WrapMode.Loop)
            {
                if (!m_Anim.IsPlaying((string)AttackAnim.GetValue()))
                    m_Anim.Play((string)AttackAnim.GetValue());
            }
            else
            {
                m_Anim.PlayQueued((string)AttackAnim.GetValue(), QueueMode.CompleteOthers);
            }
        }
    }
}