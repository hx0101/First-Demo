using SaveSystemTutorial;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SavePanel : BasePanel
{
    private static string name = "SavePanel";
    private static string path = "UIPanel/SavePanel";
    public static readonly UIType uIType = new UIType(path, name);

    public string saveName;

    public SavePanel() : base(uIType)
    {

    }

    public override void OnStart()
    {
        base.OnStart();
        UIMethod.GetInstance().GetOrAddSingleComponentInChild<Button>(ActiveObj, "CloseButton").onClick.AddListener(CloseButton);
        UIMethod.GetInstance().GetOrAddSingleComponentInChild<Button>(ActiveObj, "Save1").onClick.AddListener(Save1);
        UIMethod.GetInstance().GetOrAddSingleComponentInChild<Button>(ActiveObj, "Save2").onClick.AddListener(Save2);
        UIMethod.GetInstance().GetOrAddSingleComponentInChild<Button>(ActiveObj, "Save3").onClick.AddListener(Save3);
        UIMethod.GetInstance().GetOrAddSingleComponentInChild<Button>(ActiveObj, "DeleteButton").onClick.AddListener(DeleteButton);
        UIMethod.GetInstance().GetOrAddSingleComponentInChild<Button>(ActiveObj, "SaveButton").onClick.AddListener(SaveButton);
        UIMethod.GetInstance().GetOrAddSingleComponentInChild<Button>(ActiveObj, "LoadButton").onClick.AddListener(LoadButton);

        if (UIManager.GetInstance().stack_ui.Count == 2)
        {
            UIMethod.GetInstance().GetOrAddSingleComponentInChild<Button>(ActiveObj, "SaveButton").gameObject.SetActive(false);
        }
    }

    void CloseButton()
    {
        UIManager.GetInstance().Pop(false);
    }

    void Save1()
    {
        saveName = "Save1.dt";
    }

    void Save2()
    {
        saveName = "Save2.dt";
    }

    void Save3()
    {
        saveName = "Save3.dt";
    }

    void DeleteButton()
    {
        //弹出是否删除面板
    }

    void SaveButton()
    {
        //判断该存档是否存在，若不存在，直接存档，若存在，弹出是否覆盖存档面板

    }

    void LoadButton()
    {
        if (SceneControl.GetInstance().dict_scene.ContainsKey("MainScene"))
        {
            DataManager.Instance.LoadFromJson(saveName);
            UIManager.GetInstance().Pop(false);
        }
        else
        {
            //切换场景
            MainScene mainScene = new MainScene();
            SceneControl.GetInstance().LoadScene(mainScene.SceneName, mainScene);
            mainScene.GetSaveName(saveName);
        }

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
