using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseClassMono : MonoBehaviour
{
    protected virtual void Awake()
    {
        this.ReadConfig();
    }
}

public class BaseClass
{
    public BaseClass()
    {
        this.ReadConfig();
    }
}
