using UnityEngine;
using System;
using System.Collections.Generic;

public class Point
{
    public Point parent;
    public int x;
    public int y;
    public int g;
    public int h;

    public Point(int inX,int inY,Point inParent = null)
    {
        x = inX;
        y = inY;
        parent = inParent;
    }

    /// <summary>
    /// 曼哈顿方式计算H值
    /// </summary>
    public int Manhattan(int targetX, int targetY)
    {
        h = (int)(Mathf.Abs(targetX - x) + Mathf.Abs(targetY - y)) * 10;
        return h;
    }
    public int F { get { return g + h; } }

    public static Point operator +(Point p1, Point p2)
    {
        return new Point(p1.x + p2.x, p1.y + p2.y);
    }
    //public static bool operator ==(Point p1, Point p2)
    //{
    //    return ((p1.x==p2.x) && (p1.y == p2.y));
    //}
    //public static bool operator !=(Point p1, Point p2)
    //{
    //    return ((p1.x!=p2.x) && (p1.y != p2.y));
    //}
}