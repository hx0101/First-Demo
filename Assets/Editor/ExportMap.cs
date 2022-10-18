using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ExportMap : EditorWindow
{
    public static string writePath = "/ AssetsPackage / Datas / Map /";
    public static string prefix = "map";
    public static string levelNum = "1";
    [MenuItem("mapEditor/ExportMap")]
    private static void ShowWindows()
    {
        var window = GetWindow<ExportMap>();
        window.titleContent = new GUIContent("ExportMap");
        window.Show();
    }
    private void OnGUI()
    {
        GUILayout.Label("选择地图根节点");

        if (Selection.activeGameObject != null)
        {
            GUILayout.Label(Selection.activeGameObject.name);
        }
        else
        {
            GUILayout.Label("没有选择的地图节点，无法生成");
        }
        GUILayout.Label("\n输出路径");
        writePath = GUILayout.TextField(writePath);
        GUILayout.Label("地图文件前缀");
        prefix = GUILayout.TextField(prefix);
        GUILayout.Label("关卡的数字(1～N)");
        levelNum = GUILayout.TextField(levelNum);

        string fileName = writePath + prefix + levelNum + ".csv";
        GUILayout.Label(fileName + "\n");

        if (GUILayout.Button("生成地图文件"))
        {
            if (Selection.activeGameObject != null)
            {
                ExportMapUtil.ExportMapToFile(Selection.activeGameObject,fileName);
            }
            
        }
        if (GUILayout.Button("清理地图文件"))
        {

        }
    }

    void OnSelectionChange()
    {
        this.Repaint();
    }
}
