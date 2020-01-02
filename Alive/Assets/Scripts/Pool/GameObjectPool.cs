using System;
using UnityEngine;
using System.Linq;
using System.Collections.Generic;

public enum GOType
{
    
}

public class GameObjectPool : Singleton<GameObjectPool>
{
    public Dictionary<string, List<GameObjectBase>> myPool = new Dictionary<string, List<GameObjectBase>>();

    private Transform unUseParent;
    private Transform useParent;

    public override void Initialize()
    {
        unUseParent = new GameObject("UnUseParent").transform;
        unUseParent.gameObject.SetActive(false);
        unUseParent.position = Vector3.zero;

        useParent = new GameObject("UseParent").transform;
        useParent.position = Vector3.zero;
    }

    public GameObject Spawn(string inPath, Vector3 inLoc, Quaternion inQuaternion)
    {
        if (string.IsNullOrEmpty(inPath)) return null;

        GameObject goTemplate = Resources.Load<GameObject>(inPath);
        if (goTemplate == null) return null;

        GameObjectBase objectBase = null;
        List<GameObjectBase> goList;

        if (myPool.TryGetValue(inPath, out goList) && goList != null)
        {
            objectBase = goList.Find((go) => go.CanUse);
            if (objectBase == null)
            {
                objectBase = new GameObjectBase(useParent, unUseParent, GameObject.Instantiate(goTemplate, inLoc, inQuaternion));
            }
        }
        else
        {
            goList = new List<GameObjectBase>();
            objectBase = new GameObjectBase(useParent, unUseParent, GameObject.Instantiate(goTemplate, inLoc, inQuaternion));
            goList.Add(objectBase);
            myPool.Add(inPath, goList);
        }

        if (objectBase != null)
        {
            objectBase.myGo.transform.position = inLoc;
            objectBase.OnSpawn();
        }
        return objectBase.myGo;
    }

    public void UnSpawn(GameObject inGo,string inPath)
    {
        List<GameObjectBase> goList = null;
        if (myPool.TryGetValue(inPath, out goList) && goList != null)
        {
            GameObjectBase objectBase = goList.Find((go)=>go.myGo == inGo);
            if(objectBase != null)
                objectBase.OnUnSpawn();
        }
    }
}