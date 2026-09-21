using UnityEngine;
using UnityEditor;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject continueButton;
    [SerializeField] private GameObject loadPanel;

    private void Start()
    {
        // O LoadPanel começa escondido
        if (loadPanel != null)
        {
            loadPanel.SetActive(false);
        }
        else
        {
            Debug.LogError("LoadPanel não foi atribuído no Inspector.");
        }

        // Mostra o botão Continue apenas se existir um AutoSave no Slot 0
        if (SaveSystem.Instance != null)
        {
            if (continueButton != null)
            {
                continueButton.SetActive(
                    SaveSystem.Instance.SaveExists(0)
                );
            }
        }
        else
        {
            if (continueButton != null)
            {
                continueButton.SetActive(false);
            }
        }
    }

    // NOVO JOGO
    public void NovoJogo()
    {
        GameManager.Instance.NovoJogo();
    }

    // CONTINUAR JOGO
    public void ContinuarJogo()
    {
        if (SaveSystem.Instance == null)
        {
            Debug.LogError("SaveSystem não encontrado.");
            return;
        }

        if (!SaveSystem.Instance.LoadFromSlot(0))
        {
            Debug.Log("Não existe um jogo salvo para continuar.");
            return;
        }

        GameManager.Instance.IniciarJogoCarregado();
    }

    // ABRIR TELA DE SLOTS
    public void CarregarJogo()
    {
        if (loadPanel == null)
        {
            Debug.LogError("LoadPanel não foi atribuído no Inspector.");
            return;
        }

        // Ativa o painel
        loadPanel.SetActive(true);

        // Coloca o painel na frente dos outros elementos da UI
        loadPanel.transform.SetAsLastSibling();

        Debug.Log("LoadPanel aberto!");
    }

    // CARREGAR UM DOS SLOTS
    public void CarregarSlot(int slot)
    {
        if (SaveSystem.Instance == null)
        {
            Debug.LogError("SaveSystem não encontrado.");
            return;
        }

        if (!SaveSystem.Instance.SaveExists(slot))
        {
            Debug.Log("O Slot " + slot + " está vazio.");
            return;
        }

        if (!SaveSystem.Instance.LoadFromSlot(slot))
        {
            Debug.LogError("Não foi possível carregar o Slot " + slot + ".");
            return;
        }

        Debug.Log("Slot " + slot + " carregado com sucesso!");

        GameManager.Instance.IniciarJogoCarregado();
    }

    // FECHAR TELA DE SLOTS
    public void FecharTelaDeSlots()
    {
        if (loadPanel != null)
        {
            loadPanel.SetActive(false);
        }
    }

    // SAIR DO JOGO
    public void SairDoJogo()
    {
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif

        Debug.Log("Jogo fechado.");
    }
}