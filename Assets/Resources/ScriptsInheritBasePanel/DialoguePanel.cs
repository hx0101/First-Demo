using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialoguePanel : BasePanel
{
    private static string name = "DialoguePanel";
    private static string path = "UIPanel/DialoguePanel";
    public static readonly UIType uIType = new UIType(path, name);

    float alphaSpeed;
    bool flag;

    public DialoguePanel() : base(uIType)
    {

    }

    public override void OnStart()
    {
        base.OnStart();
       
    }

    public override void OnUpdate()
    {
        
    }

    public override void OnEnable()
    {
        base.OnEnable();
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
