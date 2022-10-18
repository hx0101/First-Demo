using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadPanel : BasePanel
{
    private static string name = "LoadScenePanel";
    private static string path = "UIPanel/LoadScenePanel";
    public static readonly UIType uIType = new UIType(path, name);

    public LoadPanel() : base(uIType)
    {

    }
    public override void OnDestroy()
    {
        base.OnDestroy();
    }

    public override void OnDisable()
    {
        base.OnDisable();
    }

    public override void OnEnable()
    {
        base.OnEnable();
    }

    public override void OnStart()
    {
        base.OnStart();
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
    }
}
