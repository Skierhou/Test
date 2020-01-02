using UnityEngine;
using System.Collections.Generic;
using DG.Tweening;

[Config]
public class AICmd_Patrol : AICmd
{
    [Config]
    public float MoveTimeMin;   //移动最小时间
    [Config]
    public float MoveTimeMax;   //移动最大时间

    private Vector3 PawnRotation;
    private float MoveTime;

    private Vector3 LastPosition;
    private float LastPositionTimer;

    private bool bStartPatrol;
        
    public override void Pushed()
    {
        base.Pushed();
        bStartPatrol = false;
        FindNextPatrol();
    }
    public override void Poped()
    {
        base.Poped();
    }

    public override void Update()
    {
        if (m_Owenr == null)
            return;

        if (bStartPatrol)
        {
            if (!m_AIOwner.Animation.IsPlaying("Run1"))
            {
                m_AIOwner.Animation.Play("Run1");
            }
            m_Owenr.GetComponent<Rigidbody>().MovePosition(m_Owenr.transform.position + m_Owenr.transform.forward * Time.deltaTime * m_Owenr.CFG_Speed);

            if (Time.time - LastPositionTimer >= 0.1f)
            {
                RaycastHit hit;
                if (Physics.Raycast(m_Owenr.transform.position + Vector3.up * 0.5f ,m_Owenr.transform.forward , out hit))
                {
                    if (hit.distance <= 0.3f)
                    {
                        AICmd.InitCmd<AICmd_Idle>(m_AIOwner);
                    }
                }
            }

            if (MoveTime > 0)
            {
                MoveTime -= Time.deltaTime;
            }
            else
            {
                AICmd.InitCmd<AICmd_Idle>(m_AIOwner);
            }
        }
    }

    private void FindNextPatrol()
    {
        List<float> angleList = new List<float>();
        for (int i = 0; i < 8; i++)
        {
            angleList.Add(i * 45);
        }

        MoveTime = Random.Range(MoveTimeMin, MoveTimeMax);

        Vector3 orginalEuler = m_Owenr.transform.eulerAngles;
        Vector3 tEul;

        while (true)
        {
            int tRand = Random.Range(0, angleList.Count);
            float tAngle = angleList[tRand];
            tEul = m_Owenr.transform.eulerAngles + Vector3.up * tAngle;
            m_Owenr.transform.eulerAngles = tEul;
            RaycastHit hit;
            if (Physics.Raycast(m_Owenr.transform.position + Vector3.up * 0.5f, m_Owenr.transform.forward, out hit))
            {
                if (hit.distance >= 1f)
                {
                    m_Owenr.transform.eulerAngles = orginalEuler;
                    break;
                }
                else
                {
                    angleList.RemoveAt(tRand);
                    if (angleList.Count == 0)
                        break;
                }
            }
            m_Owenr.transform.eulerAngles = orginalEuler;
        }


        if (!m_AIOwner.Animation.IsPlaying("Fidle0"))
        {
            m_AIOwner.Animation.Play("Fidle0");
        }
        LastPosition = m_Owenr.transform.position;
        m_Owenr.transform.DORotate(tEul, 1).onComplete = () =>
        {
            LastPositionTimer = Time.time;
            bStartPatrol = true;
        };
        bStartPatrol = false;
    }
}