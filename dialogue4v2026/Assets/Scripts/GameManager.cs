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

        // Começa sempre pela fase 1
        SceneManager.LoadScene("fase1");
    }

    // =========================================================
    // INICIAR JOGO CARREGADO
    // =========================================================

    public void IniciarJogoCarregado()
    {
        Time.timeScale = 1f;

        if (SaveSystem.Instance == null)
        {
            Debug.LogError("SaveSystem não encontrado.");
            return;
        }

        SaveSystem.Save save =
            SaveSystem.Instance.GetSave(0);

        if (save == null)
        {
            Debug.LogError("Nenhum save encontrado no Slot 0.");
            return;
        }

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
    }

    // =========================================================
    // CARREGAR UMA FASE
    // =========================================================

    public void LoadScenes(string sceneName)
    {
        if (sceneName != "fase1" &&
            sceneName != "fase2")
        {
            Debug.LogError(
                "Cena inválida: " + sceneName
            );

            return;
        }

        Time.timeScale = 1f;

        SceneManager.LoadScene(sceneName);
    }

    // =========================================================
    // CARREGAR GUI
    // =========================================================

    public void CarregarGUI()
    {
        // Verifica se a GUI já está carregada
        Scene cenaGUI = SceneManager.GetSceneByName("GUI");

        if (!cenaGUI.isLoaded)
        {
            SceneManager.LoadScene(
                "GUI",
                LoadSceneMode.Additive
            );
        }
    }
}