using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameRoot : MonoBehaviour
{
    private static GameRoot instance;

    private UIManager UIManager;

    public UIManager UIManager_Root { get => UIManager; }

    private SceneControl SceneControl;
    public SceneControl SceneControl_Root { get => SceneControl; }

    public static GameRoot GetInstance() 
    {
        if (instance == null)  
        {
            Debug.LogWarning("GameRoot Ins is false!");
            return instance;
        }

        return instance;
    }

    private void Awake()
    {
        if (instance == null) 
        {
            instance = this;
        }
        else 
        {
            Destroy(this.gameObject);
        }

        UIManager = new UIManager();
        SceneControl = new SceneControl();
       
    }

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
        UIManager_Root.canvasObj = UIMethod.GetInstance().FindCanvas();

        StartScene startScene = new StartScene();
        SceneControl_Root.dict_scene.Add(startScene.SceneName, startScene);

        //UIManager_Root.Push(new SecondPanel());
        UIManager_Root.Push(new FirstPanel());
    }

    // Update is called once per frame
    void Update()
    {
        if (UIManager.stack_ui.Count > 0)
        {
            UIManager.stack_ui.Peek().OnUpdate();
        }
    }
}
