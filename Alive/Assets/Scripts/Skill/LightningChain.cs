using System;
using System.Collections;
using UnityEngine;
using System.Collections.Generic;

[Config]
public class LightningChain : SkillBase
{
    [Config]
    public float CFG_FindNextRadius;

    [Config]
    public string CFG_LightingFXPath;

    public override void Initialize()
    {
        base.Initialize();
    }
    public override IEnumerator SkillState(Pawn inOwner, Pawn inTarget)
    {
        Pawn tNextTarget = inTarget;
        Pawn tPawn = inTarget;
        List<GameObject> vimitList = new List<GameObject>();

        while (CFG_DamageCount > 0 && tNextTarget != null)
        {
            tNextTarget.TakeDamage(CFG_Damage, Vector3.zero, inOwner);
            vimitList.Add(tNextTarget.gameObject);

            CFG_DamageCount--;
            if (CFG_DamageCount <= 0)
                break;

            GameObject[] gos = GameObject.FindGameObjectsWithTag("Enemy");
            float dis1;
            float dis2 = float.MaxValue;
            for (int i = 0; i < gos.Length; i++)
            {
                dis1 = Vector3.Distance(gos[i].transform.position, tPawn.transform.position);
                if (!vimitList.Contains(gos[i]) && dis1 < CFG_FindNextRadius
                    && dis1 < dis2)
                {
                    tNextTarget = gos[i].GetComponent<Pawn>();
                }
            }
            yield return new WaitForSeconds(0.5f); 
        }
        yield return null;
    }
}
