using UnityEngine;
using UnityEngine.SceneManagement;

public class VictoryZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        Debug.Log("Jogador entrou na zona de vitória!");

        Time.timeScale = 1f;

        if (SaveSystem.Instance != null)
        {
            SaveSystem.Save save =
                SaveSystem.Instance.GetSave(0);

            if (save != null)
            {
                save.fase = 2;

                save.checkpointAtivado = false;

                save.checkpointX = 0;
                save.checkpointY = 0;
                save.checkpointZ = 0;

                save.moedasNoCheckpoint = 0;

                save.moedasColetadas.Clear();

                SaveSystem.Instance.SaveToFile(0);
            }
        }

        SceneManager.LoadScene("fase2");
    }
}