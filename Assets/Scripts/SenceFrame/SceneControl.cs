using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class SceneControl
{
    public Dictionary<string, SceneBase> dict_scene;

    private static SceneControl instence;

    public static SceneControl GetInstance()
    {
        if (instence == null)
        {
            Debug.LogError("SceneControl实体不存在");
            return instence;
        }

        return instence;
    }

    public SceneControl()
    {
        instence = this;
        dict_scene = new Dictionary<string, SceneBase>();
    }
    /// <summary>
    /// 加载一个场景
    /// </summary>
    /// <param name="scene_name">场景名称</param>
    /// <param name="sceneBase">场景的SceneBase</param>
    public void LoadScene(string scene_name, SceneBase sceneBase)
    {
        if (!dict_scene.ContainsKey(scene_name))
        {
            dict_scene.Add(scene_name, sceneBase);
        }

        if (dict_scene.ContainsKey(SceneManager.GetActiveScene().name))
        {
            dict_scene[SceneManager.GetActiveScene().name].ExitScene();
        }
        else
        {
            Debug.LogWarning($"SceneControl的字典中不包含{SceneManager.GetActiveScene().name}!");
        }

        UIManager.GetInstance().Pop(true);
        SceneManager.LoadScene(scene_name);

        sceneBase.EnterScene();
                      
        
    }

    public IEnumerator LoadLevel(string scene_name, SceneBase sceneBase, BasePanel basePanel)
    {
        UIManager.GetInstance().Push(basePanel);
        Text text = UIMethod.GetInstance().GetOrAddSingleComponentInChild<Text>(basePanel.ActiveObj, "LoadText");
        Slider slider = UIMethod.GetInstance().GetOrAddSingleComponentInChild<Slider>(basePanel.ActiveObj, "LoadSlider");

        if (!dict_scene.ContainsKey(scene_name))
        {
            dict_scene.Add(scene_name, sceneBase);
        }

        if (dict_scene.ContainsKey(SceneManager.GetActiveScene().name))
        {
            dict_scene[SceneManager.GetActiveScene().name].ExitScene();
        }
        else
        {
            Debug.LogWarning($"SceneControl的字典中不包含{SceneManager.GetActiveScene().name}!");
        }

        AsyncOperation operation = SceneManager.LoadSceneAsync(scene_name);

        operation.allowSceneActivation = false;

        while (!operation.isDone)
        {
            slider.value = operation.progress;
            text.text = operation.progress * 100 + "%";

            if (operation.progress >= 0.9f)
            {
                slider.value = 1;

                text.text = "Press AnyKey To Continue";

                if (Input.anyKeyDown)
                {
                    UIManager.GetInstance().Pop(true);

                    sceneBase.EnterScene();
                    operation.allowSceneActivation = true;
                }
            }
            yield return null;
        }
    }
}

