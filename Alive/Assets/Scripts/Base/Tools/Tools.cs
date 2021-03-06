﻿using System;
using System.Collections.Generic;
using UnityEngine;

class Tools
{
    public static Type GetTypeByString(string type)
    {
        switch (type.ToLower())
        {
            case "bool":
                return Type.GetType("System.Boolean", true, true);
            case "byte":
                return Type.GetType("System.Byte", true, true);
            case "sbyte":
                return Type.GetType("System.SByte", true, true);
            case "char":
                return Type.GetType("System.Char", true, true);
            case "decimal":
                return Type.GetType("System.Decimal", true, true);
            case "double":
                return Type.GetType("System.Double", true, true);
            case "float":
                return Type.GetType("System.Single", true, true);
            case "int":
                return Type.GetType("System.Int32", true, true);
            case "uint":
                return Type.GetType("System.UInt32", true, true);
            case "long":
                return Type.GetType("System.Int64", true, true);
            case "ulong":
                return Type.GetType("System.UInt64", true, true);
            case "object":
                return Type.GetType("System.Object", true, true);
            case "short":
                return Type.GetType("System.Int16", true, true);
            case "ushort":
                return Type.GetType("System.UInt16", true, true);
            case "string":
                return Type.GetType("System.String", true, true);
            case "date":
            case "datetime":
                return Type.GetType("System.DateTime", true, true);
            case "guid":
                return Type.GetType("System.Guid", true, true);
            default:
                return Type.GetType(type, false, true);
        }
    }

    public static Sprite CreateSprite(string inIconPath)
    {
        Texture2D texture = Resources.Load<Texture2D>(inIconPath);
        return Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
    }
}