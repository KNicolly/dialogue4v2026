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
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    // =========================================================
    // NOVO JOGO
    // =========================================================

    public void NovoJogo()
    {
        Time.timeScale = 1f;

        SceneManager.LoadScene("fase1");
        SceneManager.LoadScene("GUI", LoadSceneMode.Additive);
    }

    // =========================================================
    // INICIAR JOGO CARREGADO
    // =========================================================

    public void IniciarJogoCarregado()
    {
        Time.timeScale = 1f;

        SaveSystem.Save save =
            SaveSystem.Instance.GetSave(0);

        if (save == null)
            return;

        if (save.fase == 1)
        {
            SceneManager.LoadScene("fase1");
        }
        else if (save.fase == 2)
        {
            SceneManager.LoadScene("fase2");
        }
        else
        {
            SceneManager.LoadScene("fase1");
        }

        SceneManager.LoadScene(
            "GUI",
            LoadSceneMode.Additive
        );
    }

    // =========================================================
    // CARREGAR UMA FASE
    // =========================================================

    public void LoadScenes(string sceneName)
    {
        if (sceneName != "fase1" &&
            sceneName != "fase2")
        {
            return;
        }

        Time.timeScale = 1f;

        SceneManager.LoadScene(sceneName);

        SceneManager.LoadScene(
            "GUI",
            LoadSceneMode.Additive
        );
    }
}


