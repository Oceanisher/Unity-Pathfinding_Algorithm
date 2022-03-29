using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 单例模式
/// </summary>
/// <typeparam name="T"></typeparam>
public class Singleton<T> : MonoBehaviour where T : Singleton<T>
{
    private static T instance; 

    public static T Instance
    {
        get
        {
            return instance;
        }
    }

    public void Awake()
    {
        if (null != instance)
        {
            Debug.LogError("Object is singleton.");
            Destroy(gameObject);
        }
        else
        {
            instance = (T)this;
        }
    }
}
