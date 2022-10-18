using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadManager : Singleton<LoadManager>
{
    MainScene mainScene = new MainScene();

    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }
    public void LoadNextLevel()
    {
        StartCoroutine(SceneControl.GetInstance().LoadLevel(mainScene.SceneName, mainScene, new LoadPanel()));
    }

    public void LoadMainPanel()
    {
        StartCoroutine(LoadMainInterfacePanel());
    }

    public IEnumerator LoadMainInterfacePanel()
    {
        yield return new WaitForSeconds(0.01f);
        UIManager.GetInstance().canvasObj = UIMethod.GetInstance().FindCanvas();
        UIManager.GetInstance().Push(new MainInterfacePanel());
    }
}
