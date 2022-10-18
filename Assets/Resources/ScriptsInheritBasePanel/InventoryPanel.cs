using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class InventoryPanel : BasePanel
{
    private static string name = "InventoryPanel";
    private static string path = "UIPanel/InventoryPanel";
    public static readonly UIType uIType = new UIType(path, name);

    public InventoryPanel() : base(uIType)
    {

    }

    public override void OnStart()
    {
        base.OnStart();
        UIMethod.GetInstance().GetOrAddSingleComponentInChild<Button>(ActiveObj, "CloseButton").onClick.AddListener(CloseButton);
    }

    void CloseButton()
    {
        UIManager.GetInstance().Pop(false);
    }

    public override void OnEnable()
    {
        base.OnEnable();
    }

    public override void OnUpdate()
    {

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
