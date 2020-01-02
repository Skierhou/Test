using System;
using UnityEngine;
using System.Collections.Generic;
public class Door : ElementBase
{
    private BoxCollider m_Collider;

    protected override void Awake()
    {
        base.Awake();
        m_Collider = GetComponent<BoxCollider>();
    }

    public void OpenDoor()
    {
        m_Collider.isTrigger = true;
    }
    public void CloseDoor()
    {
        m_Collider.isTrigger = false;
    }
}