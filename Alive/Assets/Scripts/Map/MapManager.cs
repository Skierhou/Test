using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager:MonoSingleton<MapManager>
{
    private GameObject WallGoTemplate;
    private GameObject DoorGoTemplate;
    private GameObject[] DestroyedGoTemplates;
    private GameObject[] ObstacleGoTemplates;
    private GameObject[] GroundGoTemplates;

    private Vector3 StartLoc;

    public Room currentRoom;
    public int enemyCount;

    List<Room> RoomList = new List<Room>();

    private void Awake()
    {
        Initialize();
    }

    public void Initialize()
    {
        WallGoTemplate = Resources.Load<GameObject>("WallGoTemplate");
        DoorGoTemplate = Resources.Load<GameObject>("DoorGoTemplate");
        GroundGoTemplates = new GameObject[] {
            Resources.Load<GameObject>("GroundGoTemplate1"),
            Resources.Load<GameObject>("GroundGoTemplate2"),
            Resources.Load<GameObject>("GroundGoTemplate3"),
        };
        DestroyedGoTemplates = new GameObject[]
        {
            Resources.Load<GameObject>("DestroyedGoTemplate1"),
        };
        ObstacleGoTemplates = new GameObject[]
        {
            Resources.Load<GameObject>("ObstacleGoTemplate1"),
            Resources.Load<GameObject>("ObstacleGoTemplate2"),
        };
    }
    public void CreateRoom(int inRoomCount, int inRoomWidth)
    {
        int[,] intMap = new int[inRoomCount * 2, inRoomCount * 2];

        int count = inRoomCount;

        int startX = inRoomCount;
        int startY = inRoomCount;
        //设置起始位置
        intMap[startX, startY] = 1;
        inRoomCount--;
        CreateMap(intMap, ref inRoomCount, startX, startY);

        List<Vector2> tPointArray = new List<Vector2>();
        int minX = int.MaxValue;
        int minY = int.MaxValue;
        Vector2 minPoint = new Vector2(int.MaxValue, int.MaxValue);
        Vector2 maxPoint = new Vector2(int.MinValue, int.MinValue);

        for (int i = 0; i < count * 2; i++)
        {
            for (int j = 0; j < count * 2; j++)
            {
                if (intMap[i, j] == 1)
                {
                    if (i < minX)
                        minX = i;
                    if (j < minY)
                        minY = j;
                    tPointArray.Add(new Vector2(i, j));
                }
            }
        }
        for (int i = 0; i < tPointArray.Count; i++)
        {
            float x = tPointArray[i].x - minX;
            float y = tPointArray[i].y - minY;
            tPointArray[i] = new Vector2(x, y);
        }
        //下面循环为了找到该地图中相离最远的两个点
        minPoint = tPointArray[tPointArray.Count - 1];
        maxPoint = tPointArray[tPointArray.Count - 1];
        for (int i = 0; i < tPointArray.Count; i++)
        {
            if (Vector2.Distance(tPointArray[i],minPoint) > Vector3.Distance(minPoint,maxPoint))
            {
                maxPoint = tPointArray[i];
            }
        }
        CheckIsEndPoint(tPointArray, ref maxPoint);

        for (int i = 0; i < tPointArray.Count; i++)
        {
            Room tRoom = new GameObject().AddComponent<Room>();
            RoomList.Add(tRoom);
            tRoom.transform.SetParent(transform);
            tRoom.Initialize(inRoomWidth, new Vector3(tPointArray[i].x * inRoomWidth * 2, 0, tPointArray[i].y * inRoomWidth * 2), 1
                , WallGoTemplate, DoorGoTemplate, DestroyedGoTemplates, ObstacleGoTemplates, GroundGoTemplates);
            if (minPoint == tPointArray[i])
            {
                tRoom.gameObject.name = "FirstRoom";
                StartLoc = tRoom.transform.position + (Vector3.forward + Vector3.right) * inRoomWidth / 2;
                tRoom.CreateFirstRoom(tPointArray, tPointArray[i]);
            }
            else if (maxPoint == tPointArray[i])
            {
                tRoom.gameObject.name = "EndRoom";
                tRoom.CreateEndRoom(tPointArray, tPointArray[i]);
            }
            else
            {
                tRoom.CreateLoopRoom(tPointArray, tPointArray[i]);
            }
        }
    }

    public Vector3 GetPlayerStartPoint()
    {
        return StartLoc;
    }

    private void CheckIsEndPoint(List<Vector2> inPointList,ref Vector2 endPoint)
    {
        int x = (int)endPoint.x;
        int y = (int)endPoint.y;
        if (HasOtherRoom(inPointList, (int)endPoint.x + 1, (int)endPoint.y))
            endPoint = new Vector2(x + 1, y);
        else if (HasOtherRoom(inPointList, (int)endPoint.x - 1, (int)endPoint.y) && x - 1 >= 0)
            endPoint = new Vector2(x - 1, y);
        else if (HasOtherRoom(inPointList, (int)endPoint.x, (int)endPoint.y + 1))
            endPoint = new Vector2(x, y + 1);
        else if (HasOtherRoom(inPointList, (int)endPoint.x, (int)endPoint.y - 1) && y - 1 >= 0)
            endPoint = new Vector2(x, y - 1);
    }
    private bool HasOtherRoom(List<Vector2> inPointList, int inX,int inY)
    {
        int tCount = 0;
        bool tHasPoint = false;

        for (int i = 0; i < inPointList.Count; i++)
        {
            if (inPointList[i].x == inX && inPointList[i].y == inY)
                tHasPoint = true;
            if (inPointList[i].x == (inX + 1) && inPointList[i].y == inY)
                tCount++;
            if (inPointList[i].x == (inX - 1) && inPointList[i].y == inY)
                tCount++;
            if (inPointList[i].x == inX && (inPointList[i].y + 1) == inY)
                tCount++;
            if (inPointList[i].x == inX && (inPointList[i].y - 1) == inY)
                tCount++;
        }
        return tCount == 1 && tHasPoint;
    }

    private void CreateMap(int[,] inIntMap, ref int inRoomCount, int inX, int inY)
    {
        List<Vector2> tPointList = new List<Vector2>();
        int count = 0;
        while (tPointList.Count <= 2 && inRoomCount > 0)
        {
            if (UnityEngine.Random.value > 0.5f && inRoomCount > 0)
            {
                //左
                if (inIntMap[inX - 1, inY] == 0)
                {
                    inRoomCount--;
                    inIntMap[inX - 1, inY] = 1;
                    tPointList.Add(new Vector2(inX - 1, inY));
                }
            }
            if (UnityEngine.Random.value > 0.5f && inRoomCount > 0)
            {
                //右
                if (inIntMap[inX + 1, inY] == 0)
                {
                    inRoomCount--;
                    inIntMap[inX + 1, inY] = 1;
                    tPointList.Add(new Vector2(inX + 1, inY));
                }
            }
            if (UnityEngine.Random.value > 0.5f && inRoomCount > 0)
            {
                //上
                if (inIntMap[inX, inY + 1] == 0)
                {
                    inRoomCount--;
                    inIntMap[inX, inY + 1] = 1;
                    tPointList.Add(new Vector2(inX, inY + 1));
                }
            }
            if (UnityEngine.Random.value > 0.5f && inRoomCount > 0)
            {
                //下
                if (inIntMap[inX, inY - 1] == 0)
                {
                    inRoomCount--;
                    inIntMap[inX, inY - 1] = 1;
                    tPointList.Add(new Vector2(inX, inY - 1));
                }
            }
            count++;
            if (inRoomCount <= 0 || count >= 2)
                break;
        }
        for (int i = 0; i < tPointList.Count; i++)
        {
            CreateMap(inIntMap, ref inRoomCount, (int)tPointList[i].x, (int)tPointList[i].y);
        }
    }

    public void OpenDoor()
    {
        for (int i = 0; i < RoomList.Count; i++)
        {
            RoomList[i].OpenDoor();
        }
    }

    public void CloseDoor()
    {
        for (int i = 0; i < RoomList.Count; i++)
        {
            RoomList[i].CloseDoor();
        }
    }

    /// <summary>
    /// 监听房间是否能关闭
    /// </summary>
    public void MonitorRoomCanClose(Room inRoom,int inEnemyCount)
    {
        StartCoroutine(StartMonitor(inRoom, inEnemyCount));
    }

    private IEnumerator StartMonitor(Room inRoom, int inEnemyCount)
    {
        enemyCount = inEnemyCount;
        while (enemyCount > 0)
        {
            yield return null;
        }
        inRoom.CloseDoor();
        yield return null;
    }
}