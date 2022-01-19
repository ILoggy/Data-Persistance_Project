using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;
using UnityEngine.UI;
using Unity.IO;

public class MenuManager : MonoBehaviour
{
    public Text placeHolder;
    public InputField inputName;

    public string PlayerName;

    public static MenuManager Instance;
    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(this);
    }

    public void LaunchGame()
    {
        PlayerName = inputName.text;
        Debug.LogWarning(PlayerName);
        SceneManager.LoadScene(1);
    }

    public void Exit()
    {
        if (EditorApplication.isPlaying)
        {
            EditorApplication.ExitPlaymode();
        }
        else
        {
            Application.Quit();
        }
    }

    
}
