using UnityEngine;
using System.Collections;

public class MenuController : MonoBehaviour
{
    public string sceneName = "SampleScene";
    private void Start()
    {
        GameManager.Instance.ChangeState(GameManager.GameState.MenuPrincipal);
        StartGame();
    }

    public void StartGame()
    {
        StartCoroutine(loadScene());
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Saiu do jogo");
    }
    
    private IEnumerator loadScene()
    {
        yield return new WaitForSeconds(2f);
        
        GameManager.Instance.ChangeState(GameManager.GameState.Gameplay);
        GameManager.Instance.LoadScene(sceneName);
    }
}