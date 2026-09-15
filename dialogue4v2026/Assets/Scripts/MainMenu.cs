using UnityEngine;
using UnityEditor;

public class MainMenu : MonoBehaviour
{
    [SerializeField]
    private GameObject continueButton;

    [SerializeField]
    private GameObject loadSlotsPanel;

    private void Start()
    {
        loadSlotsPanel.SetActive(false);

        if (SaveSystem.Instance != null)
        {
            continueButton.SetActive(
                SaveSystem.Instance.SaveExists(0)
            );
        }
        else
        {
            continueButton.SetActive(false);
        }
    }

    // =========================================================
    // NOVO JOGO
    // =========================================================

    public void NovoJogo()
    {
        GameManager.Instance.NovoJogo();
    }

    // =========================================================
    // CONTINUAR JOGO
    // =========================================================

    public void ContinuarJogo()
    {
        if (!SaveSystem.Instance.LoadFromSlot(0))
        {
            return;
        }

        GameManager.Instance.IniciarJogoCarregado();
    }

    // =========================================================
    // ABRIR TELA DE SLOTS
    // =========================================================

    public void CarregarJogo()
    {
        loadSlotsPanel.SetActive(true);
    }

    // =========================================================
    // CARREGAR SLOT
    // =========================================================

    public void CarregarSlot(int slot)
    {
        if (SaveSystem.Instance == null)
            return;

        if (!SaveSystem.Instance.SaveExists(slot))
        {
            Debug.Log(
                "O Slot " + slot + " está vazio."
            );

            return;
        }

        if (!SaveSystem.Instance.LoadFromSlot(slot))
        {
            Debug.Log(
                "Não foi possível carregar o Slot " + slot
            );

            return;
        }

        GameManager.Instance.IniciarJogoCarregado();
    }

    // =========================================================
    // VOLTAR
    // =========================================================

    public void FecharTelaDeSlots()
    {
        loadSlotsPanel.SetActive(false);
    }

    // =========================================================
    // SAIR
    // =========================================================

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