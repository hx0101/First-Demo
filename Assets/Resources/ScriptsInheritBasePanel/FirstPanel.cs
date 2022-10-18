using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FirstPanel : BasePanel
{
    private static string name = "FirstPanel";
    private static string path = "UIPanel/FirstPanel";
    public static readonly UIType uIType = new UIType(path, name);

    TextMeshProUGUI timeText;
    TextMeshProUGUI startAnyKey;

    float alphaSpeed;
    bool flag;

    public FirstPanel() : base(uIType)
    {

    }

    public override void OnStart()
    {
        base.OnStart();
        timeText = UIMethod.GetInstance().GetOrAddSingleComponentInChild<TextMeshProUGUI>(ActiveObj, "TimeText");

        startAnyKey = UIMethod.GetInstance().GetOrAddSingleComponentInChild<TextMeshProUGUI>(ActiveObj, "StartAnyKey");
        startAnyKey.alpha = 0.4f;
        alphaSpeed = 0.3f;
    }

    public override void OnUpdate()
    {
        timeText.text = System.DateTime.Now.ToString("T");

        if (startAnyKey.alpha >= 0.7f)
        {
            flag = true;
        }
        else if (startAnyKey.alpha <= 0.3f)
        {
            flag = false;
        }
        if (flag)
        {
            startAnyKey.alpha -= alphaSpeed * Time.deltaTime;
        }
        else
        {
            startAnyKey.alpha += alphaSpeed * Time.deltaTime;
        }

        if (Input.anyKeyDown)
        {
            UIManager.GetInstance().Pop(false);
            UIManager.GetInstance().Push(new SecondPanel());
        }
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
