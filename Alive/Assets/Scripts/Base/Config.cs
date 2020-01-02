using System;
using UnityEngine;
using System.Reflection;
using System.IO;
using System.Text.RegularExpressions;
using System.Collections.Generic;

// 一个自定义特性 Config 被赋给类及其成员
[AttributeUsage(AttributeTargets.Class |
AttributeTargets.Constructor |
AttributeTargets.Field |
AttributeTargets.Method |
AttributeTargets.Property,
AllowMultiple = true)]
class Config : Attribute
{
    public Config() {}
}

struct ConfigData
{
    public Type Type;
    public List<FieldInfo> FieldInfoList;
    public List<PropertyInfo> PropertyInfoList;
};
class ConfigManager : Singleton<ConfigManager>
{
    //文件名
    private const string CONFIGPATH = "Config";
    //注释分隔符
    private const char NOTESIGN = ';';
    //类名修饰
    private const string CLASSSIGNBEGIN = "[";
    private const string CLASSSIGNEND = "]";
    //等于号
    private const char EQUALSIGN = '=';
    //默认配置文件格式
    private const string FORMATSIGN = ".ini";
    //构造体结构
    private const char STRUCTSIGNBEGIN = '(';
    private const char STRUCTSIGNEND = ')';
    private const char STRUCTNOTESIGN = ',';
    //配置文件路径
    private static string DirectionPath = UnityEngine.Application.dataPath + "/../" + CONFIGPATH;
    //存取所有类的配置信息Map
    private Dictionary<Type, Dictionary<string, string>> m_ConfigDict = new Dictionary<Type, Dictionary<string, string>>();
    //所有配置基类
    private Type BaseClassType = typeof(BaseClass);
    private Type BaseClassTypeMono = typeof(BaseClassMono);

    //public ConfigManager()
    //{
    //    Initialize();
    //}
    public override void Initialize()
    {
        if (!Directory.Exists(DirectionPath))
        {
            Debug.LogWarning("不存在该配置路径文件，Path : " + DirectionPath);
            return;
        }
        DirectoryInfo tDirectoryInfo = new DirectoryInfo(DirectionPath);
        if (tDirectoryInfo != null)
        {
            FileInfo[] files = tDirectoryInfo.GetFiles();
            for (int i = 0; i < files.Length; i++)
            {
                if (files[i].FullName.EndsWith(FORMATSIGN))
                {
                    ReadFileConfig(files[i].FullName);
                }
            }
        }
    }
    /// <summary>
    /// 读取文件中的所有配置
    /// </summary>
    private void ReadFileConfig(string inPath)
    {
        if (String.IsNullOrEmpty(inPath))
            return;

        if (File.Exists(inPath))
        {
            FileStream tFS = File.Open(inPath, FileMode.Open);
            StreamReader tReader = new StreamReader(tFS);
            Dictionary<string, string> tConfigDict = null;
            while (!tReader.EndOfStream)
            {
                string tStr = Regex.Replace(tReader.ReadLine(), @"\s", "");
                //当前行为类名
                if (tStr.StartsWith(CLASSSIGNBEGIN) && tStr.EndsWith(CLASSSIGNEND))
                {
                    Type tType = Tools.GetTypeByString(tStr.Substring(1, tStr.Length - 2));
                    if (tType != null)
                    {
                        tConfigDict = new Dictionary<string, string>();
                        m_ConfigDict.Add(tType, tConfigDict);
                    }
                }
                else
                {
                    if (tConfigDict != null && !tStr.StartsWith(NOTESIGN.ToString()))
                    {
                        string[] tStrArray = tStr.Split(NOTESIGN);
                        if (tStrArray.Length > 0)
                        {
                            if (tStrArray[0].Contains(STRUCTSIGNBEGIN.ToString()) && tStrArray[0].Contains(STRUCTSIGNEND.ToString()))
                            {
                                int tFirstEqualSignIndex = tStrArray[0].IndexOf(EQUALSIGN);

                                string tKey = tStrArray[0].Substring(0, tFirstEqualSignIndex);
                                string tValue = tStrArray[0].Substring(tFirstEqualSignIndex + 1, tStrArray[0].Length - tFirstEqualSignIndex - 1);

                                if (!String.IsNullOrEmpty(tValue))
                                {
                                    if (tConfigDict.ContainsKey(tKey.ToUpper()))
                                    {
                                        tConfigDict[tKey] += STRUCTNOTESIGN + tValue;
                                    }
                                    else
                                    {
                                        tConfigDict.Add(tKey.ToUpper(), tValue);
                                    }
                                }
                            }
                            else
                            {
                                tStrArray = tStrArray[0].Split(EQUALSIGN);
                                if (tStrArray.Length == 2)
                                {
                                    //Key:0, Value:1
                                    if (tConfigDict.ContainsKey(tStrArray[0].ToUpper()))
                                    {
                                        tConfigDict[tStrArray[0].ToUpper()] += STRUCTNOTESIGN + tStrArray[1];
                                    }
                                    else
                                    {
                                        tConfigDict.Add(tStrArray[0].ToUpper(), tStrArray[1]);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
        else
        {
            Debug.LogWarning("不存在该文件，Path:" + inPath);
        }
    }

    /// <summary>
    /// 类初始化调用
    /// </summary>
    public void ObjectInitialize(object inObj)
    {
        NodeList NodeList = new NodeList();
        List<FieldInfo> tFieldInfoList;
        List<PropertyInfo> tPropertyInfoList;

        Type tType = inObj.GetType();
        while (tType != BaseClassType && tType != BaseClassTypeMono && tType != null)
        {
            FieldInfo[] tFieldInfos = tType.GetFields(BindingFlags.Instance | BindingFlags.DeclaredOnly
                | BindingFlags.NonPublic | BindingFlags.Public);
            PropertyInfo[] tProperties = tType.GetProperties(BindingFlags.Instance | BindingFlags.DeclaredOnly
                | BindingFlags.NonPublic | BindingFlags.Public);

            tFieldInfoList = new List<FieldInfo>();
            tPropertyInfoList = new List<PropertyInfo>();
            if (tFieldInfos.Length > 0 )
            {
                tFieldInfoList.AddRange(tFieldInfos);
            }
            if (tProperties.Length > 0 )
            {
                tPropertyInfoList.AddRange(tProperties);
            }
            NodeList.Push_Back(new ConfigData { Type = tType, FieldInfoList = tFieldInfoList, PropertyInfoList = tPropertyInfoList });

            tType = tType.BaseType;
        }

        Node tNode = NodeList.EndNode;
        tFieldInfoList = new List<FieldInfo>();
        tPropertyInfoList = new List<PropertyInfo>();
        while (tNode != null)
        {
            ConfigData configData = (ConfigData)tNode.Data;

            tFieldInfoList.AddRange(configData.FieldInfoList);
            tPropertyInfoList.AddRange(configData.PropertyInfoList);
            configData.FieldInfoList = tFieldInfoList;
            configData.PropertyInfoList = tPropertyInfoList;
            tNode.Data = configData;

            tNode = tNode.PreNode;
        }

        tNode = NodeList.EndNode;
        while (tNode != null)
        {
            foreach (FieldInfo fieldInfo in ((ConfigData)tNode.Data).FieldInfoList)
            {
                Config tConfig = fieldInfo.GetCustomAttribute<Config>();
                if (tConfig != null)
                {
                    SetObjectProperty(inObj, fieldInfo, ((ConfigData)tNode.Data).Type);
                }
            }
            foreach (PropertyInfo propertyInfo in ((ConfigData)tNode.Data).PropertyInfoList)
            {
                Config tConfig = propertyInfo.GetCustomAttribute<Config>();
                if (tConfig != null)
                {
                    SetObjectProperty(inObj, propertyInfo, ((ConfigData)tNode.Data).Type);
                }
            }
            tNode = tNode.PreNode;
        }
    }

    /// <summary>
    /// 设置类属性
    /// </summary>
    private void SetObjectProperty(object inObj, PropertyInfo inInfo, Type inType)
    {
        Dictionary<string, string> tDict = m_ConfigDict.TryGetValueByKey(inType);
        if (tDict != null)
        {
            string tStr = tDict.TryGetValueByKey(inInfo.Name.ToUpper());
            if (!String.IsNullOrEmpty(tStr))
            {
                try
                {
                    string[] tStrArray = tStr.Split(STRUCTNOTESIGN);
                    if (inInfo.PropertyType.ToString().Contains("System.Collections.Generic.List"))
                    {
                        Type type = GetListType(inInfo.PropertyType.ToString());
                        object entityList = Activator.CreateInstance(inInfo.PropertyType);
                        BindingFlags flag = BindingFlags.Instance | BindingFlags.Public; MethodInfo methodInfo = inInfo.PropertyType.GetMethod("Add", flag);

                        for (int i = 0; i < tStrArray.Length; i++)
                        {
                            methodInfo.Invoke(entityList, new object[] { Convert.ChangeType(tStrArray[i], type) });//相当于List<T>调用Add方法
                        }
                        inInfo.SetValue(inObj, entityList);
                    }
                    else
                    {
                        if (tStrArray[0].StartsWith(STRUCTSIGNBEGIN.ToString()) && tStrArray[0].EndsWith(STRUCTSIGNEND.ToString()))
                        {
                            tStrArray[0] = tStrArray[0].Substring(1, tStrArray[0].Length - 2);
                            object tObj = inInfo.GetValue(inObj, null);
                            SetStructProperty(tObj, tStrArray[0]);
                            inInfo.SetValue(inObj, tObj);
                        }
                        else
                        {
                            inInfo.SetValue(inObj, Convert.ChangeType(tStrArray[0], inInfo.PropertyType));
                        }
                    }
                }
                catch (Exception e)
                {
                    Debug.LogWarning(string.Format("无法将{0}转成{1}类型! 报错:{2}", tStr, inInfo.PropertyType.ToString(),e.Message));
                }
            }
        }
    }

    private Type GetListType(string inTypeStr)
    {
        if (inTypeStr.Contains("System.Int32"))
        {
            return typeof(int);
        }
        else if (inTypeStr.Contains("System.Single"))
        {
            return typeof(float);
        }
        else if (inTypeStr.Contains("System.Double"))
        {
            return typeof(double);
        }
        else if (inTypeStr.Contains("System.Int64"))
        {
            return typeof(long);
        }
        else if (inTypeStr.Contains("System.String"))
        {
            return typeof(string);
        }
        return null;
    }

    /// <summary>
    /// 设置构造体内字段值
    /// </summary>
    /// <param name="inObj">一个类里的构造体对象</param>
    /// <param name="inValue">值:格式(X=1,Y=2,Z=3)</param>
    private void SetStructProperty(object inObj,string inValue)
    {
        String[] tValueArray = inValue.Split(STRUCTNOTESIGN);

        FieldInfo[] tFieldInfos = inObj.GetType().GetFields(BindingFlags.Instance | BindingFlags.DeclaredOnly
            | BindingFlags.NonPublic | BindingFlags.Public);
        
        for (int i = 0; i < tValueArray.Length; i++)
        {
            String[] tStrArray = tValueArray[i].Split(EQUALSIGN);
            if (tStrArray.Length == 2)
            {
                for (int tFieldIndex = 0; tFieldIndex < tFieldInfos.Length; tFieldIndex++)
                {
                    if (tFieldInfos[tFieldIndex].Name.ToUpper() == tStrArray[0])
                    {
                        tFieldInfos[tFieldIndex].SetValue(inObj, Convert.ChangeType(tStrArray[1], tFieldInfos[tFieldIndex].FieldType));
                    }
                }
            }
        }
    }
    private void SetObjectProperty(object inObj, FieldInfo inInfo, Type inType)
    {
        Dictionary<string, string> tDict = m_ConfigDict.TryGetValueByKey(inType);
        if (tDict != null)
        {
            string tStr = tDict.TryGetValueByKey(inInfo.Name.ToUpper());
            if (!String.IsNullOrEmpty(tStr))
            {
                try
                {
                    string[] tStrArray = tStr.Split(STRUCTNOTESIGN);
                    if (inInfo.FieldType.ToString().Contains("System.Collections.Generic.List"))
                    {
                        Type type = GetListType(inInfo.FieldType.ToString());
                        object entityList = Activator.CreateInstance(inInfo.FieldType);
                        BindingFlags flag = BindingFlags.Instance | BindingFlags.Public; MethodInfo methodInfo = inInfo.FieldType.GetMethod("Add", flag);

                        for (int i = 0; i < tStrArray.Length; i++)
                        {
                            methodInfo.Invoke(entityList, new object[] { Convert.ChangeType(tStrArray[i], type) });//相当于List<T>调用Add方法
                        }
                        inInfo.SetValue(inObj, entityList);
                    }
                    else
                    {
                        if (tStrArray[0].StartsWith(STRUCTSIGNBEGIN.ToString()) && tStrArray[0].EndsWith(STRUCTSIGNBEGIN.ToString()))
                        {
                            tStrArray[0] = tStrArray[0].Substring(1, tStrArray[0].Length - 2);
                            SetStructProperty(inInfo.GetValue(inObj), tStrArray[0]);
                        }
                        else
                        {
                            inInfo.SetValue(inObj, Convert.ChangeType(tStrArray[0], inInfo.FieldType));
                        }
                    }
                }
                catch (Exception e)
                {
                    Debug.LogWarning(string.Format("无法将{0}转成{1}类型! 报错:{2}", tStr, inInfo.FieldType.ToString(), e.Message));
                }
            }
        }
    }
}