using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager
{
    /// <summary>
    /// 存储UIPanel的栈结构
    /// </summary>
    public Stack<BasePanel> stack_ui;

    /// <summary>
    /// 存储Panel的名称与物体的对应关系
    /// </summary>
    public Dictionary<string, GameObject> dict_uiobject;

    /// <summary>
    /// 当前场景对应的canvas
    /// </summary>
    public GameObject canvasObj;

    private static UIManager instance;

    public static UIManager GetInstance()
    {
        if (instance == null)
        {
            Debug.Log("UIManager实例不存在");
            return instance;
        }
        else
        {
            return instance;
        }
    }

    public UIManager()
    {
        instance = this;
        stack_ui = new Stack<BasePanel>();
        dict_uiobject = new Dictionary<string, GameObject>();
    }

    public GameObject GetSingleObject(UIType uIType)
    {
        if (dict_uiobject.ContainsKey(uIType.Name))
        {
            return dict_uiobject[uIType.Name];
        }

        if (canvasObj == null)
        {
            canvasObj = UIMethod.GetInstance().FindCanvas();
        }

        GameObject gameObject = GameObject.Instantiate<GameObject>(Resources.Load<GameObject>(uIType.Path),canvasObj.transform);
        return gameObject;
    }


    /// <summary>
    /// 往stack里面压入一个Panel
    /// </summary>
    /// <param name="basePanel">目标panel</param>
    public void Push(BasePanel basePanel)
    {
        Debug.Log($"{basePanel.uiType.Name}被Push进stack");

        if (stack_ui.Count > 0)
        {
            stack_ui.Peek().OnDisable();
        }

        GameObject ui_object = GetSingleObject(basePanel.uiType);

        dict_uiobject.Add(basePanel.uiType.Name, ui_object);

        basePanel.ActiveObj = ui_object;

        if (stack_ui.Count == 0)
        {
            stack_ui.Push(basePanel);
        }
        else
        {
            if (stack_ui.Peek().uiType.Name != basePanel.uiType.Name)
            {
                stack_ui.Push(basePanel);
            }
        }

        basePanel.OnStart();
    }

    /// <summary>
    /// 出栈
    /// </summary>
    /// <param name="isload">isload为真时pop全部</param>
    public void Pop(bool isload)
    {
        if (isload == true)
        {
            if (stack_ui.Count > 0)
            {
                stack_ui.Peek().OnDisable();
                stack_ui.Peek().OnDestroy();
                GameObject.Destroy(dict_uiobject[stack_ui.Peek().uiType.Name]);
                dict_uiobject.Remove(stack_ui.Peek().uiType.Name);
                stack_ui.Pop();
                Pop(true);
            }
        }
        else
        {
            if (stack_ui.Count > 0)
            {
                stack_ui.Peek().OnDisable();
                stack_ui.Peek().OnDestroy();
                GameObject.Destroy(dict_uiobject[stack_ui.Peek().uiType.Name]);
                dict_uiobject.Remove(stack_ui.Peek().uiType.Name);
                stack_ui.Pop();

                if (stack_ui.Count > 0)
                {
                    stack_ui.Peek().OnEnable();
                }
            }
        }
    }
}

