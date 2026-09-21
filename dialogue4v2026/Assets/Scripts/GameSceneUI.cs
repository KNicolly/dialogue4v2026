using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSceneUI : MonoBehaviour
{
    private void Start()
    {
        if (SceneManager.GetSceneByName("GUI").isLoaded)
            return;

        SceneManager.LoadScene(
            "GUI",
            LoadSceneMode.Additive
        );
    }
}