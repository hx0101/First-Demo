using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMethod
{
    private static UIMethod instance;

    /// <summary>
    /// 将UIMethod制作成单例模式
    /// 其方法大都是提供给Panel使用的
    /// </summary>
    public static UIMethod GetInstance()
    {
        if (instance == null)
        {
            instance = new UIMethod();
        }
        return instance;
    }

    /// <summary>
    /// 获得当前场景中的Canvas
    /// </summary>
    /// <returns>Canvas Object</returns>
    public GameObject FindCanvas()
    {
        GameObject gameObject = GameObject.FindGameObjectWithTag("MainCanvas").gameObject;
        if (gameObject == null)
        {
            Debug.LogError("没有在场景中找到Canvas");
        }

        return gameObject;
    }

    /// <summary>
    /// 从目标对象中根据名字找到子对象
    /// </summary>
    /// <param name="panel">目标Panel</param>
    /// <param name="child_name">目标子对象名称</param>
    /// <returns></returns>
    public GameObject FindObjectInChild(GameObject panel, string child_name)
    {
        Transform[] transform = panel.GetComponentsInChildren<Transform>();

        foreach (var tra in transform)
        {
            if (tra.gameObject.name == child_name)
            {
                return tra.gameObject;
            }
        }

        Debug.LogWarning($"在{panel.name}在物体中没有找到{child_name}物体！");
        return null;
    }

    /// <summary>
    /// 从目标对象中获得对应组件
    /// </summary>
    /// <typeparam name="T">对应组件</typeparam>
    /// <param name="Get_Obj">目标对象</param>
    /// <returns></returns>
    public T AddOrGetComponent<T>(GameObject Get_Obj) where T : Component
    {
        if (Get_Obj.GetComponent<T>() != null)
        {
            return Get_Obj.GetComponent<T>();
        }

        Debug.LogWarning($"无法在{Get_Obj}物体上获得目标组件！");
        return null;
    }

    /// <summary>
    ///从目标Panel的子物体中，根据子物体的名称获得对应组件 
    /// </summary>
    /// <typeparam name="T">对应组件</typeparam>
    /// <param name="panel">目标Panel</param>
    /// <param name="ComponentName">子物体名称</param>
    /// <returns></returns>
    public T GetOrAddSingleComponentInChild<T>(GameObject panel, string ComponentName) where T : Component
    {
        Transform[] transforms = panel.GetComponentsInChildren<Transform>();

        foreach (Transform tra in transforms)
        {
            if (tra.gameObject.name == ComponentName)
            {
                return tra.gameObject.GetComponent<T>();
            }
        }

        Debug.LogWarning($"没有在{panel.name}中找到{ComponentName}物体！");
        return null;
    }
}
