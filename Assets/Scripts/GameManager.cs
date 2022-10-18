using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    static GameManager instence;

    private void Awake()
    {
        if (instence != null)
        {
            Destroy(gameObject);
            return;
        }

        instence = this;

        DontDestroyOnLoad(this);
    }

    public static void PlayerDied()
    {
        instence.Invoke("RestartScene",0f);
    }

    void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
