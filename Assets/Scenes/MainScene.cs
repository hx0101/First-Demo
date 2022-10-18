using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SaveSystemTutorial;
using DG.Tweening;

public class MainScene : SceneBase
{
    public readonly string SceneName = "MainScene";

    string saveName;
    public override void EnterScene()
    {
        if (!AudioManager.current.musicSource)
        {
            AudioManager.current.musicSource.Play();
        }
        
        LoadManager.Instance.LoadMainPanel();
        if (saveName != null)
        {
            DataManager.Instance.LoadFromJson(saveName);
        }
        

    }


    public override void ExitScene()
    {
        
    }

    public void GetSaveName(string SaveName)
    {
        saveName = SaveName;
    }
}
