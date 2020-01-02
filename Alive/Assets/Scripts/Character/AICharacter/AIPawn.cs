using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[Config]
public class AIPawn : Pawn
{
    private const string UI_ENEMYHP = "UI/EnemyHp";

    [Config]
    public int CFG_DieMoney;        //死亡得到金币
    [Config]
    public float CFG_HardValue;     //怪物硬值
    [Config]
    public float CFG_HardStraightTime;      //硬值时间
    [Config]
    public float CFG_HpUIOffset;            //HpUI偏移

    public float hardValue;
    private bool bHardStraight;

    //自身组件
    private CharacterController m_CC;
    private BoxCollider m_Trigger;

    private AICmdAction m_AICmdAction;
    private GameObject m_Target;
    private UI_EnemyHp m_HpUI;

    private bool bDie;

    private bool bStartAStarFind;
    private Vector3 m_TargetLoc;

    public override void Initialize()
    {
        base.Initialize();
        m_CC = GetComponent<CharacterController>();
        m_Trigger = GetComponent<BoxCollider>();

        GameObject enemyUIGo = GameObjectPool.Instance.Spawn(UI_ENEMYHP,transform.position,Quaternion.identity);
        m_HpUI = enemyUIGo.GetComponent<UI_EnemyHp>();
        m_HpUI.Initialize(this);

        m_Trigger.enabled = false;
        m_Trigger.isTrigger = true;

        hardValue = CFG_HardValue;
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

    public void TakeDamage(int inDamage, Vector3 inMovement, Pawn inCauser, int inHardValue)
    {
        if (bDie) return;

        base.TakeDamage(inDamage, inMovement, inCauser);

        if (!bHardStraight)
        {
            hardValue -= inHardValue;
            hardValue = Mathf.Max(0, hardValue);
            if (hardValue <= 0)
            {
                bHardStraight = true;
                Invoke("ResetHardValue", CFG_HardStraightTime);
            }
        }

        if (hp <= 0)
        {
            Died(inCauser);
        }
    }
    private void ResetHardValue()
    {
        bHardStraight = false;
        hardValue = CFG_HardValue;
        m_HpUI.RecoveryHardValue();
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
