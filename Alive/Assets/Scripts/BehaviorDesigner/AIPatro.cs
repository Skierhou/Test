using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Basic.UnityAnimation
{
    [TaskCategory("Basic/AIPawn")]
    [TaskDescription("Plays animation without any blending. Returns Success.")]
    public class AIPatro : Action
    {
        private bool bStartPatrol;
        private bool bStartIdle;
        private float moveTime;
        private float idleTimer;
        [Tooltip("IdleTime")]
        public SharedFloat IdleTime;
        [Tooltip("IdleAnim")]
        public SharedString IdleAnim;
        [Tooltip("MoveTimeMin")]
        public SharedFloat MoveTimeMin;
        [Tooltip("MoveTimeMax")]
        public SharedFloat MoveTimeMax;

        private Animation m_Anim;
        private AIPawn m_AIPawn;

        public override void OnStart()
        {
            if (m_Anim == null)
                m_Anim = GetComponent<Animation>();
            if (m_AIPawn == null)
                m_AIPawn = GetComponent<AIPawn>();

            if (!bStartPatrol && !bStartIdle)
            {
                Vector3 tVec;
                while (true)
                {
                    tVec = MapManager.Instance.currentRoom.FindSpaceLind();
                    if (Vector3.Distance(transform.position, tVec) >= 2)
                    {
                        break;
                    }
                }
                m_AIPawn.StartAStarFind(tVec);
                moveTime = Random.Range((float)MoveTimeMin.GetValue(), (float)MoveTimeMax.GetValue());
                bStartPatrol = true;
            }
        }

        public override TaskStatus OnUpdate()
        {
            if (bStartPatrol)
            {
                if (moveTime > 0)
                {
                    moveTime -= Time.deltaTime;
                }
                else
                {
                    //进入Idle
                    GetComponent<AIPawn>().StopAStarFind();
                    idleTimer = Time.time;
                    bStartIdle = true;
                    bStartPatrol = false;
                }
            }
            else
            {
                if (bStartIdle)
                {
                    if (Time.time - idleTimer >= (float)IdleTime.GetValue())
                    {
                        bStartIdle = false;
                        bStartPatrol = false;
                    }
                    StartIdle();
                }
            }
            return TaskStatus.Success;
        }

        private void FindNextPatrol()
        {
            List<float> angleList = new List<float>();
            for (int i = 0; i < 8; i++)
            {
                angleList.Add(i * 45);
            }

            moveTime = Random.Range((float)MoveTimeMin.GetValue(), (float)MoveTimeMax.GetValue());

            Vector3 orginalEuler = transform.eulerAngles;
            Vector3 tEul;

            while (true)
            {
                int tRand = Random.Range(0, angleList.Count);
                float tAngle = angleList[tRand];
                tEul = transform.eulerAngles + Vector3.up * tAngle;
                transform.eulerAngles = tEul;
                RaycastHit hit;
                if (Physics.Raycast(transform.position + Vector3.up * 0.5f, transform.forward, out hit))
                {
                    if (hit.distance >= 1f)
                    {
                        transform.eulerAngles = orginalEuler;
                        break;
                    }
                    else
                    {
                        angleList.RemoveAt(tRand);
                        if (angleList.Count == 0)
                            break;
                    }
                }
                transform.eulerAngles = orginalEuler;
            }

            transform.DORotate(tEul, 1).onComplete = () =>
            {
                bStartPatrol = true;
            };
            bStartPatrol = false;
        }

        private void StartIdle()
        {
            if (m_Anim.clip.wrapMode == WrapMode.Loop)
            {
                if(!m_Anim.IsPlaying((string)IdleAnim.GetValue()))
                    m_Anim.Play((string)IdleAnim.GetValue());
            }
            else
            {
                m_Anim.PlayQueued((string)IdleAnim.GetValue(), QueueMode.CompleteOthers);
            }
        }

        public override void OnReset()
        {

        }
    }
}