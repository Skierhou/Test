using System;
using System.Collections.Generic;
using UnityEngine;

public class PlaceTool:MonoBehaviour
{
    public EToolType toolType;
    public string toolPath;

    public void Initialize(EToolType inToolType,string inPath)
    {
        toolType = inToolType;
        toolPath = inPath;
    }

    private void OnTriggerStay(Collider other)
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (other.gameObject == CharacterManager.Instance.Player.gameObject)
            {
                ToolsManager.Instance.GetTool(toolType);
                GameObjectPool.Instance.UnSpawn(gameObject, toolPath);
            }
        }
    }
}
