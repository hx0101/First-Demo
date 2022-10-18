using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IEnumeratorClass : MonoBehaviour
{
    public IEnumerator Continum(SceneBase sceneBase)
    {
        yield return new WaitForSeconds(10f);
        sceneBase.EnterScene();
    }
}
