using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStart : MonoBehaviour
{
    CameraManager cameraManager;
    PlayerPawn player;

    void Awake()
    {
        //地图初始化
        MapManager.Instance.CreateRoom(10,20);
        //相机初始化
        cameraManager = new CameraManager();
    }
    private void Start()
    {
        //玩家创建
        player = CharacterManager.Instance.SpawnPlayer(MapManager.Instance.GetPlayerStartPoint());
        cameraManager.SetTarget(player.gameObject);
        MapManager.Instance.OpenDoor();

        UIManager.Instance.PushPanel(EPanelType.MainPanel);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            CharacterManager.Instance.TakeDamage();
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            MapManager.Instance.CloseDoor();
        }
    }
    private void LateUpdate()
    {
        cameraManager.LastUpdate();
    }
}
