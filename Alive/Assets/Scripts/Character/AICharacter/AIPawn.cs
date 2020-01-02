using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[Config]
public class AIPawn : Pawn
{
    [Config]
    public int CFG_DieMoney;    //死亡得到金币
    [Config]
    public float CFG_HardValue;     //怪物硬值

    public float hardValue;

    //自身组件
    private CharacterController m_CC;
    private BoxCollider m_Trigger;

    private AICmdAction m_AICmdAction;
    private GameObject m_Target;

    private bool bDie;

    private bool bStartAStarFind;
    private Vector3 m_TargetLoc;

    public override void Initialize()
    {
        base.Initialize();
        m_CC = GetComponent<CharacterController>();
        m_Trigger = GetComponent<BoxCollider>();

        m_Trigger.enabled = false;
        m_Trigger.isTrigger = true;
    }

    private void Start()
    {
        StartCoroutine(AStarFind());
    }


    private void Update()
    {

    }

    public void StartAStarFind(Vector3 inTargetLoc)
    {
        m_TargetLoc = inTargetLoc;
        bStartAStarFind = true;
    }
    public void StopAStarFind()
    {
        bStartAStarFind = false;
    }
    IEnumerator AStarFind()
    {
        while (!bDie)
        {
            if (bStartAStarFind)
            {
                if (Vector3.Distance(m_TargetLoc, transform.position) > Time.deltaTime * CFG_Speed)
                {
                    List<Vector3> pathList = MapManager.Instance.currentRoom.AStarFind(transform.position, m_TargetLoc, out int g);

                    if (pathList.Count > 1)
                    {
                        //防止方向乱抖
                        if(Vector3.Distance(pathList[1],transform.position) >= 0.1f)
                            transform.LookAt(pathList[1]);
                        m_CC.Move(transform.forward * CFG_Speed * Time.deltaTime);
                    }
                }
                else
                {
                    transform.position = m_TargetLoc;
                }
            }
            yield return null;
        }
    }

    public override void TakeDamage(int inDamage, Vector3 inMovement, Pawn inCauser)
    {
        base.TakeDamage(inDamage, inMovement, inCauser);

        if (hp <= 0)
        {
            Died(inCauser);
        }
    }

    public virtual void Died(Pawn inCauser)
    {
        //是玩家则加钱
        if (inCauser is PlayerPawn)
        {
            PlayerPawn playerPawn = (PlayerPawn)inCauser;
            int addMoney = CFG_DieMoney;
            for (int i = 0; i < playerPawn.ExistSkillList.Count; i++)
            {
                addMoney += playerPawn.ExistSkillList[i].AddMoney(CFG_DieMoney);
            }
            playerPawn.AddMoney(addMoney);
        }
        bDie = true;
        m_Trigger.enabled = true;
    }


    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject == CharacterManager.Instance.Player.gameObject)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                //随机道具
                ToolsManager.Instance.CreateTool((EToolType)Random.Range(1, (int)EToolType.Max_Count), transform.position);

                GameObject.Destroy(gameObject,0.5f);
            }
        }
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
        if(m_AICmdAction != null)
            m_AICmdAction.OnDestroy();
    }
}
