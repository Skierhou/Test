using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Config]
public class PlayerPawn : Pawn
{
    private int m_Money;

    protected Animator m_Animator;
    protected Animation m_Anim;
    protected WeaponBase m_Weapon;
    protected SkillManager m_SkillMgr;
    protected Rigidbody m_Rigidbody;

    private bool canMove;

    public int Money { get => m_Money; private set => m_Money = value; }

    protected override void Awake()
    {
        base.Awake();

        m_Animator = GetComponent<Animator>();
        m_Rigidbody = GetComponent<Rigidbody>();
        m_Anim = GetComponent<Animation>();

        m_SkillMgr = new SkillManager();
    }

    private void Update()
    {
        if(m_Weapon != null)
            m_Weapon.OnUpdate();

        PlayerInput();
    }

    private void FixedUpdate()
    {
        PlayerMove();
    }

    /// <summary>
    /// 玩家移动
    /// </summary>
    protected virtual void PlayerMove()
    {
        if (!canMove) return;

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        if (x != 0 || z != 0)
        {
            m_Rigidbody.WakeUp();
            if (!m_Anim.IsPlaying("Run1"))
            {
                m_Anim.Play("Run1");
            }
            transform.forward = new Vector3(x, 0, z).normalized;
            m_Rigidbody.MovePosition(transform.position + new Vector3(x, 0, z).normalized * CFG_Speed * Time.fixedDeltaTime);
        }
        else
        {
            if (!m_Anim.IsPlaying("Fidle0"))
            {
                m_Anim.Play("Fidle0");
            }
            m_Rigidbody.Sleep();
        }
    }

    /// <summary>
    /// 玩家按键输入
    /// </summary>
    protected virtual void PlayerInput()
    {
        if (Input.GetMouseButtonDown(0))
            OnMouseLeft();
        else if (Input.GetMouseButtonDown(1))
            OnMouseRight();

        if (Input.GetKeyDown(KeyCode.Space))
            OnRoll();
    }

    protected virtual void OnMouseLeft()
    {
    }
    protected virtual void OnMouseRight()
    {
    }
    protected virtual void OnRoll()
    {
    }

    public void AddMoney(int inMoney)
    {
        Money += inMoney;
        Money = Money < 0 ? 0 : Money;
    }
}
