using System;
using UnityEngine;
using System.Collections.Generic;

public class GameObjectBase
{
    private bool canUse;

    public GameObject myGo;

    private Transform m_UnUseParent;
    private Transform m_UseParent;

    public bool CanUse { get => canUse; private set => canUse = value; }

    public GameObjectBase(Transform useParent, Transform unUseParent, GameObject go)
    {
        myGo = go;
        m_UseParent = useParent;
        m_UnUseParent = unUseParent;
    }

    public virtual void OnSpawn()
    {
        canUse = false;
        myGo.transform.SetParent(m_UseParent);
    }

    public virtual void OnUnSpawn()
    {
        canUse = true;
        myGo.transform.SetParent(m_UnUseParent);
    }
}