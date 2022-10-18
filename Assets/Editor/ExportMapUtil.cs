using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using System.IO;

public class ExportMapUtil
{
    private static string _getNumberToFixed2(Vector3 data)
    {
        string str = String.Format("{0:F},{1:F},{2:F}", data.x, data.y, data.z);
        return str; 
    }
    public static void ExportMapToFile(GameObject mapRoot,string fileName)
    {
        //打开一个文件
        StreamWriter sw = new StreamWriter(Application.dataPath + fileName);
        //写下文件数据
        //文件头
        sw.WriteLine("编号，对应资源编号，位置，缩放，角度");
        sw.WriteLine("number,string,string,string,string");
        sw.WriteLine("ID,name,position,scale,eul");

        //遍历mapNode下面的孩子
        for (int i = 0; i < mapRoot.transform.childCount;i++)
        {
            GameObject mapItem = mapRoot.transform.GetChild(i).gameObject;

            
            string name = mapItem.name;
            Vector3 position = mapItem.transform.localPosition;
            Vector3 scale = mapItem.transform.localScale;
            Vector3 eulerAngles = mapItem.transform.localEulerAngles;

            string pos = _getNumberToFixed2(position);
            string s = _getNumberToFixed2(scale);
            string eul = _getNumberToFixed2(eulerAngles);

            string lineData = String.Format("{0},{1},\"{2}\",\"{3}\",\"{4}\"", i + 1, name, pos, s, eul);
            sw.WriteLine(lineData);
        }

        //end

        //关闭一个文件
        sw.Flush();
        sw.Close();
        //刷新我们的AssetDatabase让你能快速的识别
        AssetDatabase.Refresh();
    }
}
