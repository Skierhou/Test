using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Config]
public class PlayerPawn : Pawn
{
    [Config]
    public float CFG_AttackHardValue;   //普攻的击毁硬值

    private int m_Money;

    protected Animator m_Animator;
    protected Animation m_Anim;
    protected WeaponBase m_Weapon;
    protected SkillManager m_SkillMgr;
    protected Rigidbody m_Rigidbody;

    protected bool isAttacking;

    private float rotationSmoothing = 1000;

    private bool canMove = true;

    public int Money { get => m_Money; private set => m_Money = value; }

    protected override void Awake()
    {
        base.Awake();

        m_Animator = GetComponent<Animator>();
        m_Rigidbody = GetComponent<Rigidbody>();
        m_Anim = GetComponent<Animation>();

        m_SkillMgr = new SkillManager();
    }

    protected virtual void Update()
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

        Vector3 stickDirection = new Vector3(x, 0, z);
        if (stickDirection.sqrMagnitude > 1) stickDirection.Normalize();
        float speedOut;

        if (!isAttacking)
            speedOut = stickDirection.sqrMagnitude;
        else
        {
            speedOut = 0.2f;
            m_Rigidbody.MovePosition(transform.position + stickDirection * CFG_Speed * Time.fixedDeltaTime * speedOut);
        }

        if (stickDirection != Vector3.zero)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(stickDirection, Vector3.up), rotationSmoothing * Time.deltaTime);
            if (m_Animator.GetCurrentAnimatorStateInfo(0).IsName("Blend Tree"))
                m_Rigidbody.MovePosition(transform.position + stickDirection * CFG_Speed * Time.fixedDeltaTime);
        }
        m_Animator.SetFloat("Speed", speedOut);
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
    public override void TakeDamage(int inDamage, Vector3 inMovement, Pawn inCauser)
    {
        base.TakeDamage(inDamage, inMovement, inCauser);

        if (hp <= 0)
        {
            m_Animator.SetBool("Die", true);
        }
        else
        {
            //强行播放受击动画
            m_Animator.SetTrigger("Hit");
        }
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
