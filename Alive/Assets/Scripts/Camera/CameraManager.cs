using System.Collections.Generic;
using UnityEngine;

[Config]
public class CameraManager : BaseClass
{
    [Config]
    public float cameraHigh;
    [Config]
    public float cameraAngle;

    private GameObject m_Target;
    private Camera m_Camera;
    private Vector3 vec;
    private Vector3 distance;
    
    public CameraManager()
    {
        Initatilize();
    }

    private void Initatilize()
    {
        //创建玩家的相机
        m_Camera = new GameObject("PlayerCamera").AddComponent<Camera>();
        m_Camera.tag = "MainCamera";
    }

    public void SetTarget(GameObject inObj)
    {
        m_Target = inObj;
        m_Camera.transform.position = inObj.transform.position - inObj.transform.forward * cameraHigh * Mathf.Cos(cameraAngle)
            + Vector3.up * cameraHigh;
        distance = m_Camera.transform.position - m_Target.transform.position;
        m_Camera.transform.LookAt(m_Target.transform);
    }

    public void LastUpdate()
    {
        if (m_Target != null)
        {
            m_Camera.transform.position = Vector3.SmoothDamp(m_Camera.transform.position, m_Target.transform.position + distance, ref vec, 0);
            m_Camera.transform.LookAt(m_Target.transform);
        }
    }
}