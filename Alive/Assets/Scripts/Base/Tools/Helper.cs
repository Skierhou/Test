using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public static class DictionaryHelper
{
    public static TValue TryGetValueByKey<TKey, TValue>(this IDictionary<TKey, TValue> dictionary,TKey inKey)
    {
        if (dictionary == null)
        {
            throw new ArgumentNullException("字典为空,不能排序");
        }

        TValue tValue;
        if (dictionary.TryGetValue(inKey, out tValue))
        {
            return tValue;
        }

        return tValue;
    }
}

public static class Helper
{
    public static Point FindInList(this List<Point> points,Point p)
    {
        for (int i = 0; i < points.Count; i++)
        {
            if (points[i].x == p.x && points[i].y == p.y)
                return points[i];
        }
        return null;
    }
}

public static class ConfigHelper
{
    /// <summary>
    /// 所有对象读取配置方法
    /// </summary>
    public static void ReadConfig(this object inObj)
    {
        if (inObj != null)
        {
            ConfigManager.Instance.ObjectInitialize(inObj);
        }
    }
}

