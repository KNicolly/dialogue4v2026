using UnityEngine;
using UnityEngine.SceneManagement;

public class Boot : MonoBehaviour
{
    private static bool bootExecutado = false;

    private void Awake()
    {
        if (bootExecutado)
        {
            return;
        }

        bootExecutado = true;

        SceneManager.LoadScene("MainMenu");
    }
}