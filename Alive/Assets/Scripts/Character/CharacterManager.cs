using System;
using UnityEngine;
using System.Collections.Generic;

class CharacterManager : Singleton<CharacterManager>
{
    private GameObject m_PlayerGoTemplate;
    private GameObject m_EnemyGoTemplate;

    private List<AIPawn> m_AllAIPawn;
    private PlayerPawn m_Player;

    public override void Initialize()
    {
        m_PlayerGoTemplate = Resources.Load<GameObject>("Player");
        m_EnemyGoTemplate = Resources.Load<GameObject>("Enemy");

        m_AllAIPawn = new List<AIPawn>();
    }

    public AIPawn SpawnEnemy(Vector3 inLoc)
    {
        AIPawn tPawn = GameObject.Instantiate(m_EnemyGoTemplate, inLoc, Quaternion.identity).GetComponent<AIPawn>();
        m_AllAIPawn.Add(tPawn);
        return tPawn;
    }

    public PlayerPawn SpawnPlayer(Vector3 inLoc)
    {
        m_Player = GameObject.Instantiate(m_PlayerGoTemplate, inLoc, Quaternion.identity).GetComponent<PlayerPawn>();
        return m_Player;
    }

    public void TakeDamage()
    {
        for (int i = 0; i < m_AllAIPawn.Count; i++)
        {
            m_AllAIPawn[i].TakeDamage(m_AllAIPawn[i].hp,Vector3.zero, m_AllAIPawn[i]);
        }
    }

    public Pawn Player { get { return m_Player; } }
}