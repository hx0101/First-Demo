using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenPanel : BasePanel
{
    private static string name = "OpenPanel";
    private static string path = "UIPanel/OpenPanel";
    public static readonly UIType uIType = new UIType(path, name);

    public OpenPanel() : base(uIType)
    {

    }

    public override void OnStart()
    {
        base.OnStart();
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
