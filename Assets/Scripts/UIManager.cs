using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class UIManager : MonoBehaviour
{

    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(0);
    }

    public void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif

    }


}
