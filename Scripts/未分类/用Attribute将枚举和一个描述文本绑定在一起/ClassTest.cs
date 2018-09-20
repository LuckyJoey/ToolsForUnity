using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

//属性绑定信息描述
[System.AttributeUsage(System.AttributeTargets.Field | System.AttributeTargets.Enum|System.AttributeTargets.Struct)]
public class EnumPropertiesDesc : System.Attribute
{
    string desc;
    public EnumPropertiesDesc(string str)
    {
        desc = str;
    }
    public string Desc
    {
        get
        {
            return desc;
        }
    }
}
public class EnumPropertiesUtils
{
    static Dictionary<Type, Dictionary<string, string>> cache = new Dictionary<Type, Dictionary<string, string>>();
    public static string GetPropertiesUtils(object o)
    {
        var type = o.GetType();
        Debug.Log("PropertiesUtils_type：" + type);
        if(!cache.ContainsKey(type))
        {
            Cache(type);
        }
        var fieldNameToDesc = cache[type];
        var fieldName = o.ToString();
        Debug.Log("fieldName:" + fieldName);
        return fieldNameToDesc.ContainsKey(fieldName) ? fieldNameToDesc[fieldName] : string.Format("Can not found such desc for field `{0}` in type `{1}`", fieldName, type.Name);
    }
    static void Cache(Type type)
    {
        var dict = new Dictionary<string, string>();
        cache.Add(type, dict);
        var fields = type.GetFields();
        foreach(var field in fields)
        {
            var objs = field.GetCustomAttributes(typeof(EnumPropertiesDesc), true);
            if(objs.Length>0)
            {
                dict.Add(field.Name, ((EnumPropertiesDesc)objs[0]).Desc);
            }
        }
    }
}
public enum EnumTest
{
    [EnumPropertiesDesc("枚举A")]
    Test1,
    [EnumPropertiesDesc("枚举B")]
    Test2
}
public class ClassTest:MonoBehaviour
{
    EnumTest enumTest;
    public void Start()
    {
        enumTest = EnumTest.Test2;
        Debug.Log(string.Format("属性绑定信息测试：{0}", 
            EnumPropertiesUtils.GetPropertiesUtils(enumTest)));

    }
}

