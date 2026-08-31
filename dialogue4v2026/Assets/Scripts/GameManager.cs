using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    private void Start()
    {
        LoadGameplay();
    }

    private void LoadGameplay()
    {
        SceneManager.LoadScene("fase1");
        SceneManager.LoadScene("GUI", LoadSceneMode.Additive);
    }

    public void LoadScenes(string sceneName)
    {
        if (sceneName != "fase1" && sceneName != "fase2")
            return;


        SceneManager.LoadScene(sceneName);
        SceneManager.LoadScene("GUI", LoadSceneMode.Additive);
    }
}
