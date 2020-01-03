using UnityEngine;
using System.Collections.Generic;
using DG.Tweening;
using System.Linq;

public enum DirectionType
{
    Up,
    Down,
    Right,
    Left,
};

public struct DoorData
{
    public Room NextRoom;
    public bool IsOpen;
    public bool IsFinalRoom;
}

public class Room : BaseClassMono
{
    private const int DOORWIDTH = 5;

    private GameObject WallGoTemplate;
    private GameObject DoorGoTemplate;
    private GameObject[] DestroyedGoTemplates;
    private GameObject[] ObstacleGoTemplates;
    private GameObject[] GroundGoTemplates;

    private BoxCollider m_Trigger;

    private int EnemyCount;
    private bool bTrigger;
    private bool bEndRoom;

    //房间内所有游戏物体
    List<GameObject> elementGoList;
    List<Door> doorGoList;
    List<Vector3> groundLocList;
    ElementType[,] elementTypes;

    private int roomLength;
    private int cellWidth;

    public void Initialize(int inLength, Vector3 inStartPoint, int inCellWidth, GameObject inWallGoTemplate, GameObject inDoorGoTemplate
        , GameObject[] inDestroyedGoTemplates, GameObject[] inObstacleGoTemplates, GameObject[] inGroundGoTemplates)
    {
        elementTypes = new ElementType[inLength, inLength];
        elementGoList = new List<GameObject>();
        doorGoList = new List<Door>();

        //添加触发器
        m_Trigger = gameObject.AddComponent<BoxCollider>();
        m_Trigger.size = new Vector3(inLength - 4, inLength, inLength - 4);
        m_Trigger.center = new Vector3(inLength / 2, 0, inLength / 2);
        m_Trigger.isTrigger = true;

        roomLength = inLength;
        cellWidth = inCellWidth;

        WallGoTemplate = inWallGoTemplate;
        DoorGoTemplate = inDoorGoTemplate;
        DestroyedGoTemplates = inDestroyedGoTemplates;
        ObstacleGoTemplates = inObstacleGoTemplates;
        GroundGoTemplates = inGroundGoTemplates;

        transform.localPosition = inStartPoint;
    }

    /// <summary>
    /// 开启房间门
    /// </summary>
    public void OpenDoor()
    {
        for (int i = 0; i < doorGoList.Count; i++)
        {
            doorGoList[i].transform.DOLocalMoveY(-1.5f, 0.5f);
            doorGoList[i].OpenDoor();
        }
    }
    /// <summary>
    /// 关闭房间门
    /// </summary>
    public void CloseDoor()
    {
        for (int i = 0; i < doorGoList.Count; i++)
        {
            doorGoList[i].transform.DOLocalMoveY(0, 0.5f);
            doorGoList[i].CloseDoor();
        }
        SpawnEnemy();
    }


    /// <summary>
    /// 构建地图的第一个房间
    /// </summary>
    public void CreateFirstRoom(List<Vector2> inPointList, Vector2 inPoint)
    {
        CreateGround();
        CreateAllDoor(inPointList, inPoint);
        CreateWall();
        bTrigger = true;
    }

    /// <summary>
    /// 构建地图中间的房间
    /// </summary>
    public void CreateLoopRoom(List<Vector2> inPointList, Vector2 inPoint)
    {
        CreateGround();
        CreateAllDoor(inPointList, inPoint);
        CreateWall();
        CreateObstacle(10);
        CreateDestroyedItem(10);
        EnemyCount = 2;
    }

    /// <summary>
    /// 构建地图终点房间
    /// </summary>
    public void CreateEndRoom(List<Vector2> inPointList, Vector2 inPoint)
    {
        CreateGround();
        CreateAllDoor(inPointList, inPoint);
        CreateWall();
        bEndRoom = true;
    }

    /// <summary>
    /// 获得元素的位置
    /// </summary>
    private Vector3 GetElementPosition(int inX, int inY)
    {
        Vector3 tVec = new Vector3
        {
            x = inX * cellWidth,
            y = 0,
            z = inY * cellWidth
        };
        return tVec;
    }

    /// <summary>
    /// 构建地面
    /// </summary>
    private void CreateGround()
    {
        for (int i = 0; i < roomLength; i++)
        {
            for (int j = 0; j < roomLength; j++)
            {
                GameObject go = GameObject.Instantiate(GroundGoTemplates[Random.Range(0, GroundGoTemplates.Length)], transform);
                Vector3 tVec = GetElementPosition(i, j);
                tVec.y -= 1;
                go.transform.localPosition = tVec;
                elementGoList.Add(go);
            }
        }
    }

    /// <summary>
    /// 构建围墙
    /// </summary>
    private void CreateWall()
    {
        for (int i = 0; i < roomLength; i++)
        {
            CreateOneWall(0, i);
            CreateOneWall(roomLength - 1, i);
            CreateOneWall(i, 0);
            CreateOneWall(i, roomLength - 1);
        }
    }

    /// <summary>
    /// 构建房间围墙
    /// </summary>
    private void CreateOneWall(int inX, int inY)
    {
        if (elementTypes[inX, inY] == ElementType.None)
        {
            GameObject go = GameObject.Instantiate(WallGoTemplate, transform);
            go.transform.localPosition = GetElementPosition(inX, inY);
            elementGoList.Add(go);
            elementTypes[inX, inY] = ElementType.Wall;
        }
    }

    /// <summary>
    /// 构建过道围墙
    /// </summary>
    private void CreateGangWayWall(int inX, int inY)
    {
        GameObject go = GameObject.Instantiate(WallGoTemplate, transform);
        go.transform.localPosition = GetElementPosition(inX, inY);
        elementGoList.Add(go);
    }

    /// <summary>
    /// 构建门
    /// </summary>
    private void CreateAllDoor(List<Vector2> inPointList, Vector2 inPoint)
    {
        for (int i = 0; i < inPointList.Count; i++)
        {
            if (inPointList[i] != inPoint && Vector2.Distance(inPoint, inPointList[i]) == 1)
            {
                int x = (int)(inPointList[i].x - inPoint.x);
                int y = (int)(inPointList[i].y - inPoint.y);
                if (x == 1)
                {
                    CreateDoor(DirectionType.Down);
                }
                else if (x == -1)
                {
                    CreateDoor(DirectionType.Up);
                }
                if (y == 1)
                {
                    CreateDoor(DirectionType.Right);
                }
                else if (y == -1)
                {
                    CreateDoor(DirectionType.Left);
                }
            }
        }
    }
    private void CreateDoor(DirectionType inDirectionType)
    {
        switch (inDirectionType)
        {
            case DirectionType.Up:
                CreateOneDoor(0, roomLength / 2 - DOORWIDTH / 2, DOORWIDTH, inDirectionType);
                break;
            case DirectionType.Down:
                CreateOneDoor(roomLength - 1, roomLength / 2 - DOORWIDTH / 2, DOORWIDTH, inDirectionType);
                break;
            case DirectionType.Left:
                CreateOneDoor(roomLength / 2 - DOORWIDTH / 2, 0, DOORWIDTH, inDirectionType);
                break;
            case DirectionType.Right:
                CreateOneDoor(roomLength / 2 - DOORWIDTH / 2, roomLength - 1, DOORWIDTH, inDirectionType);
                break;
            default:
                break;
        }
    }
    private void CreateOneDoor(int inX, int inY, int inDoorWidth, DirectionType inDirectionType)
    {
        //x轴正方向是Down，负方向是Up，y轴正方向是Right,负方向是Left
        if (elementTypes[inX, inY] == ElementType.None)
        {
            switch (inDirectionType)
            {
                case DirectionType.Up:
                    for (int i = 0; i < roomLength / 2 + 1; i++)
                    {
                        CreateGangWayWall(inX - i, inY - 1);
                        CreateGangWayWall(inX - i, inY + DOORWIDTH);
                    }
                    for (int i = 0; i < inDoorWidth; i++)
                    {
                        CreateOneDoor(inX, inY + i, -1, 0);
                    }
                    break;
                case DirectionType.Down:
                    for (int i = 0; i < roomLength / 2 + 1; i++)
                    {
                        CreateGangWayWall(inX + i, inY - 1);
                        CreateGangWayWall(inX + i, inY + DOORWIDTH);
                    }
                    for (int i = 0; i < inDoorWidth; i++)
                    {
                        CreateOneDoor(inX, inY + i, 1, 0);
                    }
                    break;
                case DirectionType.Left:
                    for (int i = 0; i < roomLength / 2 + 1; i++)
                    {
                        CreateGangWayWall(inX - 1, inY - i);
                        CreateGangWayWall(inX + DOORWIDTH, inY - i);
                    }
                    for (int i = 0; i < inDoorWidth; i++)
                    {
                        CreateOneDoor(inX + i, inY, 0, -1);
                    }
                    break;
                case DirectionType.Right:
                    for (int i = 0; i < roomLength / 2 + 1; i++)
                    {
                        CreateGangWayWall(inX - 1, inY + i);
                        CreateGangWayWall(inX + DOORWIDTH, inY + i);
                    }
                    for (int i = 0; i < inDoorWidth; i++)
                    {
                        CreateOneDoor(inX + i, inY, 0, 1);
                    }
                    break;
            }
        }
    }
    private void CreateOneDoor(int inX, int inY, int addX, int addY)
    {
        GameObject go;
        go = GameObject.Instantiate(DoorGoTemplate, transform);
        go.transform.localPosition = GetElementPosition(inX, inY);
        elementTypes[inX, inY] = ElementType.Door;
        elementGoList.Add(go);
        doorGoList.Add(go.GetComponent<Door>());

        //生成过道
        for (int j = 0; j < roomLength / 2 + 1; j++)
        {
            go = GameObject.Instantiate(GroundGoTemplates[Random.Range(0, GroundGoTemplates.Length)], transform);
            Vector3 tVec = GetElementPosition(inX + j * addX, inY + j * addY);
            tVec.y -= 1;
            go.transform.localPosition = tVec;
            elementGoList.Add(go);
        }
    }

    /// <summary>
    /// 随机地图中间创建
    /// </summary>
    /// <param name="inCount">随机个数</param>
    /// <param name="inCreateItem">创建Item的回调</param>
    private void CreateElementInMap(int inCount, System.Action<int, int> inCreateItem)
    {
        if (inCreateItem == null)
            return;

        while (inCount > 0)
        {
            int tRandX = Random.Range(2, roomLength - 2);
            int tRandY = Random.Range(2, roomLength - 2);

            if (elementTypes[tRandX, tRandY] == ElementType.None)
            {
                inCreateItem(tRandX, tRandY);
                inCount--;
            }
        }
    }

    /// <summary>
    /// 生成障碍物
    /// </summary>
    private void CreateObstacle(int inCount)
    {
        CreateElementInMap(inCount, CreateOneObstacle);
    }
    private void CreateOneObstacle(int inX, int inY)
    {
        if (elementTypes[inX, inY] == ElementType.None)
        {
            GameObject go = GameObject.Instantiate(ObstacleGoTemplates[Random.Range(0, ObstacleGoTemplates.Length)], transform);
            go.transform.localPosition = GetElementPosition(inX, inY);
            elementTypes[inX, inY] = ElementType.Obstacle;
            elementGoList.Add(go);
        }
    }

    private void CreateDestroyedItem(int inCount)
    {
        CreateElementInMap(inCount, CreateOneDestroyedItem);
    }
    private void CreateOneDestroyedItem(int inX, int inY)
    {
        if (elementTypes[inX, inY] == ElementType.None)
        {
            GameObject go = GameObject.Instantiate(DestroyedGoTemplates[Random.Range(0, DestroyedGoTemplates.Length)], transform);
            go.transform.localPosition = GetElementPosition(inX, inY);
            elementTypes[inX, inY] = ElementType.DestroyedItem;
            elementGoList.Add(go);
        }
    }


    private void SpawnEnemy()
    {
        if (bEndRoom)
        {
            //Boss
        }
        else
        {
            int count = EnemyCount;
            for (int i = 0; i < count; i++)
            {
                CharacterManager.Instance.SpawnEnemy(FindGroundPosition());
            }
        }
    }
    /// <summary>
    /// 找到空地
    /// </summary>
    private Vector3 FindGroundPosition()
    {
        if (groundLocList == null)
        {
            groundLocList = new List<Vector3>();
            for (int i = 0; i < roomLength; i++)
            {
                for (int j = 0; j < roomLength; j++)
                {
                    if (elementTypes[i, j] == ElementType.None)
                    {
                        groundLocList.Add(transform.position + Vector3.right * i + Vector3.forward * j);
                    }
                }
            }
        }

        int index = Random.Range(0, groundLocList.Count);
        Vector3 vec = groundLocList[index];
        groundLocList.RemoveAt(index);
        return vec;
    }

    public Vector3 FindSpaceLind()
    {
        return groundLocList[Random.Range(0, groundLocList.Count)];
    }

    /// <summary>
    /// AStar寻路到目标点
    /// </summary>
    /// <returns></returns>
    public List<Vector3> AStarFind(Vector3 inStartLoc, Vector3 inEndLoc, out int outG)
    {
        //八方向的位置
        Point[] surroundPoints = new Point[] { new Point(-1,0), new Point(0, 1), new Point(0, -1),new Point(1,0)
        ,new Point(-1,1),new Point(-1,-1),new Point(1,1),new Point(1,-1)};

        List<Point> closeList = new List<Point>();
        List<Point> openList = new List<Point>();
        List<Point> tempList = new List<Point>();

        Point startPoint = new Point(Mathf.RoundToInt((inStartLoc - transform.position).x), Mathf.RoundToInt((inStartLoc - transform.position).z));
        Point endPoint = new Point(Mathf.RoundToInt((inEndLoc - transform.position).x), Mathf.RoundToInt((inEndLoc - transform.position).z));

        if (startPoint == endPoint)
        {
            outG = 0;
            return new List<Vector3>();
        }

        startPoint.Manhattan(endPoint.x, endPoint.y);
        openList.Add(startPoint);
        int f = startPoint.F;
        int curG = 0;

        while (true)
        {
            if (openList.Count <= 0)
                break;

            tempList.Clear();
            int minG = openList.Min((p) => p.g);
            f = openList.Min((p) => p.F);
            for (int i = 0; i < openList.Count; i++)
            {
                if (openList[i].F == f)
                    tempList.Add(openList[i]);
            }

            if (tempList.Count <= 0)
                break;

            for (int i = 0; i < tempList.Count; i++)
            {
                openList.Remove(tempList[i]);
                closeList.Add(tempList[i]);
            }
            curG += minG;

            for (int i = 0; i < tempList.Count; i++)
            {
                //遍历每个点八方向
                for (int j = 0; j < surroundPoints.Length; j++)
                {
                    Point child = tempList[i] + surroundPoints[j];
                    if (child.x < 0 || child.y < 0)
                        continue;
                    if (elementTypes[child.x, child.y] == ElementType.None)
                    {
                        if (Mathf.Abs(surroundPoints[j].x) + Mathf.Abs(surroundPoints[j].y) == 2)
                        {
                            if (tempList[i].x + surroundPoints[j].x >= 0 && tempList[i].y + surroundPoints[j].y >= 0)
                            {
                                if (elementTypes[tempList[i].x + surroundPoints[j].x, tempList[i].y] != ElementType.None
                                    && elementTypes[tempList[i].x, tempList[i].y + surroundPoints[j].y] != ElementType.None)
                                {
                                    continue;
                                }
                            }
                        }
                        child.g = curG + (int)(Mathf.Sqrt(Mathf.Abs(surroundPoints[j].x) + Mathf.Abs(surroundPoints[j].y)) * 10);
                    }
                    else
                    {
                        child.g = 100000;
                        closeList.Add(child);
                    }
                    child.Manhattan(endPoint.x, endPoint.y);
                    child.parent = tempList[i];
                    if (closeList.FindInList(child) == null)
                    {
                        Point tPoint = openList.FindInList(child);
                        if (tPoint != null)
                        {
                            if (tPoint.F > child.F)
                            {
                                tPoint = child;
                            }
                        }
                        else
                        {
                            openList.Add(child);
                        }
                    }
                    if (child.x == endPoint.x && child.y == endPoint.y)
                    {
                        endPoint = child;
                        break;
                    }
                }
                if (endPoint.g > 0)
                    break;
            }
            if (endPoint.g > 0)
                break;
        }

        List<Vector3> pathList = new List<Vector3>();
        outG = endPoint.g;
        while (endPoint != null)
        {
            pathList.Add(GetElementPosition(endPoint.x, endPoint.y) + transform.position);
            endPoint = endPoint.parent;
        }
        pathList.Reverse();

        return pathList;
    }


    /// <summary>
    /// 房间的触发
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerPawn>() != null)
        {
            MapManager.Instance.currentRoom = this;
            if (!bTrigger)
            {
                CloseDoor();
                MapManager.Instance.MonitorRoomCanClose(this, EnemyCount);
                bTrigger = true;
            }
        }
    }
}
