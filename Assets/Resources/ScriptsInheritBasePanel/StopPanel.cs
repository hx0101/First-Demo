using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SaveSystemTutorial;

public class StopPanel : BasePanel
{
    private static string name = "StopPanel";
    private static string path = "UIPanel/StopPanel";
    public static readonly UIType uIType = new UIType(path, name);

    public StopPanel() : base(uIType)
    {

    }

    public override void OnStart()
    {
        base.OnStart();
        UIMethod.GetInstance().GetOrAddSingleComponentInChild<Button>(ActiveObj, "继续游戏").onClick.AddListener(继续游戏);
        UIMethod.GetInstance().GetOrAddSingleComponentInChild<Button>(ActiveObj, "保存游戏").onClick.AddListener(保存游戏);
        UIMethod.GetInstance().GetOrAddSingleComponentInChild<Button>(ActiveObj, "加载游戏").onClick.AddListener(加载游戏);
        UIMethod.GetInstance().GetOrAddSingleComponentInChild<Button>(ActiveObj, "退出游戏").onClick.AddListener(退出游戏);
    }

    public void 继续游戏()
    {
        UIManager.GetInstance().Pop(false);
    }

    public void 保存游戏()
    {
        DataManager.Instance.SaveByJson("Save1.dt");
    }

    public void 加载游戏()
    {
        //加载读档面板
        UIManager.GetInstance().Push(new SavePanel());
    }

    public void 退出游戏()
    {

    }

    public override void OnEnable()
    {
        base.OnEnable();
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
    }

    public override void OnDisable()
    {
        base.OnDisable();
    }

    public override void OnDestroy()
    {
        base.OnDestroy();
    }
}
