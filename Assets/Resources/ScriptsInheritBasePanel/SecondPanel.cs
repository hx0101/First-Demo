using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SecondPanel : BasePanel
{
    private static string name = "SecondPanel";
    private static string path = "UIPanel/SecondPanel";
    public static readonly UIType uIType = new UIType(path, name);

    Button audio;
    Image close;

    public SecondPanel() : base(uIType)
    {
        
    }

    public override void OnStart()
    {
        base.OnStart();
        audio = UIMethod.GetInstance().GetOrAddSingleComponentInChild<Button>(ActiveObj, "Audio");
        audio.onClick.AddListener(Audio);
        UIMethod.GetInstance().GetOrAddSingleComponentInChild<Button>(ActiveObj, "StartGame").onClick.AddListener(StartGame);
        UIMethod.GetInstance().GetOrAddSingleComponentInChild<Button>(ActiveObj, "ContinueGame").onClick.AddListener(ContinueGame);
        UIMethod.GetInstance().GetOrAddSingleComponentInChild<Button>(ActiveObj, "Producers").onClick.AddListener(Producers);
        UIMethod.GetInstance().GetOrAddSingleComponentInChild<Button>(ActiveObj, "ExitGame").onClick.AddListener(ExitGame);

        close = audio.transform.Find("Close").GetComponent<Image>();
        close.enabled = false;
    }

    void Audio()
    {
        if (!close.enabled)
        {
            Image image = audio.gameObject.GetComponent<Image>();
            image.color = new Color(image.color.r, image.color.g, image.color.b, 0.4f);
            close.enabled = true;
            close.gameObject.SetActive(true);
            AudioManager.current.musicSource.Pause();
        }
        else
        {
            Image image = audio.gameObject.GetComponent<Image>();
            image.color = new Color(image.color.r, image.color.g, image.color.b, 1.0f);
            close.enabled = false;
            close.gameObject.SetActive(false);
            AudioManager.current.musicSource.Play();
        }
    }

    public void StartGame()
    {
        //MainScene mainScene = new MainScene();
        //SceneControl.GetInstance().LoadScene(mainScene.SceneName,mainScene);

        LoadManager.Instance.LoadNextLevel();
        
    }

    void ContinueGame()
    {
        UIManager.GetInstance().Push(new SavePanel());
    }

    void Producers()
    {

    }

    void ExitGame()
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
