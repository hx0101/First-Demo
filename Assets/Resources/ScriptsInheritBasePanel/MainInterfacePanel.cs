using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainInterfacePanel : BasePanel
{
    private static string name = "MainInterfacePanel";
    private static string path = "UIPanel/MainInterfacePanel";
    public static readonly UIType uIType = new UIType(path, name);

    Image greenBlood;

    PlayerFSM playerFsm;
    public MainInterfacePanel() : base(uIType)
    {

    }

    public override void OnStart()
    {
        Cursor.visible = false;
        base.OnStart();
        greenBlood = UIMethod.GetInstance().GetOrAddSingleComponentInChild<Image>(ActiveObj, "GreenBlood");
        UIMethod.GetInstance().GetOrAddSingleComponentInChild<Button>(ActiveObj, "InventoryButton").onClick.AddListener(InventoryButton);
        UIMethod.GetInstance().GetOrAddSingleComponentInChild<Button>(ActiveObj,"StopButton" ).onClick.AddListener(Stop);
        UIMethod.GetInstance().GetOrAddSingleComponentInChild<Button>(ActiveObj, "OpenButton").onClick.AddListener(Open);

        playerFsm = GameObject.FindWithTag("Player").GetComponent<PlayerFSM>();
    }

    void InventoryButton()
    {
        UIManager.GetInstance().Push(new InventoryPanel());
    }
    void Stop()
    {
        UIManager.GetInstance().Push(new StopPanel());
    }

    void Open()
    {
        UIManager.GetInstance().Push(new OpenPanel());
    }

    public override void OnEnable()
    {
        base.OnEnable();
        Cursor.visible = false;
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        greenBlood.fillAmount = playerFsm.health / playerFsm.maxHealthh;

        if (Input.GetKey(KeyCode.LeftAlt))
        {
            Cursor.visible = true;
            GameObject.FindWithTag("Player").GetComponent<PlayerFSM>().enabled = false;
            Time.timeScale = 0.2f;
        }
        else
        {
            Cursor.visible = false;
            GameObject.FindWithTag("Player").GetComponent<PlayerFSM>().enabled = true;
            Time.timeScale = 1;
        }

        if (Input.GetKey(KeyCode.Escape))
        {
            UIManager.GetInstance().Push(new StopPanel());
        }

        if (Input.GetKey(KeyCode.B))
        {
            UIManager.GetInstance().Push(new InventoryPanel());
        }
    }

    public override void OnDisable()
    {
        base.OnDisable();
        Cursor.visible = true;
    }

    public override void OnDestroy()
    {
        base.OnDestroy();
        Cursor.visible = true;
    }
}
