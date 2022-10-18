using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainManager : Singleton<MainManager>
{
    public ContainerUI MainContainerUI;

    private void Update()
    {
        MainContainerUI.RefreshUI();
    }
}
