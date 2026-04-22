using UnityEngine;

public class StartGame : MonoBehaviour
{
    public void OnStartGame()
    {
        GameManager.Instance.ChangeState(GameManager.GameState.Gameplay);
        GameManager.Instance.LoadScene("SampleScene");
    }

    public void QuitGame()
    {
        Debug.Log("Saindo do jogo...");
        Application.Quit();
    }
}