using UnityEngine;
using UnityEngine.SceneManagement;

public class Checkpoint : MonoBehaviour
{
    private bool ativado = false;

    private void OnTriggerEnter(Collider other)
    {
        if (ativado)
            return;

        if (!other.CompareTag("Player"))
            return;

        ativado = true;

        int faseAtual = 1;

        if (SceneManager.GetActiveScene().name == "fase2")
        {
            faseAtual = 2;
        }

        SaveSystem.Instance.SalvarCheckpoint(
            faseAtual,
            transform.position,
            CoinManager.Instance.currentCoins,
            CoinManager.Instance.GetCollectedCoins()
        );

        Debug.Log(
            "Checkpoint ativado na fase " + faseAtual + "!"
        );
    }
}