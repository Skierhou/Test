using System;
using UnityEngine;
using System.Collections.Generic;

[Config]
public class AICmd_Idle : AICmd
{
    [Config]
    public float IdleTime;

    private float IdleTimer;

    public override void Pushed()
    {
        base.Pushed();
        IdleTimer = Time.time;
        m_AIOwner.Animation.Play("Fidle0");
    }
    public override void Poped()
    {
        base.Poped();
    }

    public override void Update()
    {
        base.Update();
        if (Time.time - IdleTimer >= IdleTime)
        {
            AICmd.InitCmd<AICmd_Patrol>(m_AIOwner);
        }
    }

}