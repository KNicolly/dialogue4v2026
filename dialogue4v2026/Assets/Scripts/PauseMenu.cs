using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class PauseMenu : MonoBehaviour
{
    [Header("Painéis")]
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private GameObject savePanel;
    [SerializeField] private GameObject loadPanel;

    private bool jogoPausado = false;

    private PlayerInput playerInput;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
    }

    private void Start()
    {
        if (pausePanel != null)
            pausePanel.SetActive(false);

        if (savePanel != null)
            savePanel.SetActive(false);

        if (loadPanel != null)
            loadPanel.SetActive(false);
    }

    private void OnEnable()
    {
        if (playerInput == null)
            playerInput = GetComponent<PlayerInput>();

        if (playerInput != null)
        {
            InputAction pauseAction = playerInput.actions.FindAction("Pause");

            if (pauseAction != null)
            {
                pauseAction.performed += OnPausePerformed;
                pauseAction.Enable();
            }
            else
            {
                Debug.LogError("A Action 'Pause' não foi encontrada no PlayerInput.");
            }
        }
        else
        {
            Debug.LogError("PlayerInput não encontrado no mesmo objeto do PauseMenu.");
        }
    }

    private void OnDisable()
    {
        if (playerInput != null)
        {
            InputAction pauseAction = playerInput.actions.FindAction("Pause");

            if (pauseAction != null)
            {
                pauseAction.performed -= OnPausePerformed;
            }
        }
    }

    private void OnPausePerformed(InputAction.CallbackContext context)
    {
        AlternarPause();
    }

    // =========================
    // ABRIR / FECHAR PAUSA
    // =========================

    public void AlternarPause()
    {
        if (jogoPausado)
        {
            FecharPause();
        }
        else
        {
            AbrirPause();
        }
    }

    public void AbrirPause()
    {
        jogoPausado = true;

        Time.timeScale = 0f;

        if (pausePanel != null)
            pausePanel.SetActive(true);

        if (savePanel != null)
            savePanel.SetActive(false);

        if (loadPanel != null)
            loadPanel.SetActive(false);
    }

    public void FecharPause()
    {
        jogoPausado = false;

        Time.timeScale = 1f;

        if (pausePanel != null)
            pausePanel.SetActive(false);

        if (savePanel != null)
            savePanel.SetActive(false);

        if (loadPanel != null)
            loadPanel.SetActive(false);
    }

    // =========================
    // SALVAR JOGO
    // =========================

    public void AbrirTelaDeSalvar()
    {
        if (savePanel == null)
            return;

        savePanel.SetActive(true);

        if (pausePanel != null)
            pausePanel.SetActive(false);

        if (loadPanel != null)
            loadPanel.SetActive(false);
    }

    public void SalvarSlot(int slot)
    {
        if (SaveSystem.Instance == null)
        {
            Debug.LogError("SaveSystem não encontrado.");
            return;
        }

        if (slot < 1 || slot > 3)
        {
            Debug.LogError("O slot deve ser 1, 2 ou 3.");
            return;
        }

        bool salvo = SaveSystem.Instance.SaveInSlot(slot);

        if (salvo)
        {
            Debug.Log("Jogo salvo no Slot " + slot + "!");

            FecharPause();
        }
        else
        {
            Debug.LogError(
                "Não foi possível salvar no Slot " + slot + "."
            );
        }
    }

    // =========================
    // CARREGAR JOGO
    // =========================

    public void AbrirTelaDeCarregar()
    {
        if (loadPanel == null)
            return;

        loadPanel.SetActive(true);

        if (pausePanel != null)
            pausePanel.SetActive(false);

        if (savePanel != null)
            savePanel.SetActive(false);
    }

    public void CarregarSlot(int slot)
    {
        if (SaveSystem.Instance == null)
        {
            Debug.LogError("SaveSystem não encontrado.");
            return;
        }

        if (slot < 1 || slot > 3)
        {
            Debug.LogError("O slot deve ser 1, 2 ou 3.");
            return;
        }

        if (!SaveSystem.Instance.SaveExists(slot))
        {
            Debug.Log("O Slot " + slot + " está vazio.");
            return;
        }

        if (!SaveSystem.Instance.LoadFromSlot(slot))
        {
            Debug.LogError(
                "Não foi possível carregar o Slot " + slot + "."
            );
            return;
        }

        Debug.Log("Slot " + slot + " carregado!");

        Time.timeScale = 1f;

        GameManager.Instance.IniciarJogoCarregado();
    }

    // =========================
    // VOLTAR
    // =========================

    public void VoltarParaPause()
    {
        if (savePanel != null)
            savePanel.SetActive(false);

        if (loadPanel != null)
            loadPanel.SetActive(false);

        if (pausePanel != null)
            pausePanel.SetActive(true);
    }

    // =========================
    // MENU PRINCIPAL
    // =========================

    public void VoltarParaMenu()
    {
        Time.timeScale = 1f;

        SceneManager.LoadScene("MainMenu");
    }
}