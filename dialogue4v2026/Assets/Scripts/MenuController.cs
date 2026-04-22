using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MenuController : MonoBehaviour
{
    public string sceneName = "SampleScene";
    private void Start()
    {
        GameManager.Instance.ChangeState(GameManager.GameState.MenuPrincipal);
        CreateUI();
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

    private void CreateUI()
    {
        // Create Canvas
        GameObject canvasGO = new GameObject("Canvas");
        Canvas canvas = canvasGO.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvasGO.AddComponent<CanvasScaler>();
        canvasGO.AddComponent<GraphicRaycaster>();

        // Create EventSystem if not exists
        if (FindObjectOfType<EventSystem>() == null)
        {
            GameObject eventGO = new GameObject("EventSystem");
            eventGO.AddComponent<EventSystem>();
            eventGO.AddComponent<StandaloneInputModule>();
        }

        // Create Start Button
        GameObject startBtnGO = new GameObject("StartButton");
        startBtnGO.transform.SetParent(canvasGO.transform);
        Button startBtn = startBtnGO.AddComponent<Button>();
        Image startImg = startBtnGO.AddComponent<Image>();
        startImg.color = Color.white;
        Text startTxt = new GameObject("Text").AddComponent<Text>();
        startTxt.transform.SetParent(startBtnGO.transform);
        startTxt.text = "Start Game";
        startTxt.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
        startTxt.color = Color.black;
        startTxt.alignment = TextAnchor.MiddleCenter;
        startBtn.onClick.AddListener(StartGame);
        RectTransform startRect = startBtnGO.GetComponent<RectTransform>();
        startRect.sizeDelta = new Vector2(200, 50);
        startRect.anchoredPosition = new Vector2(0, 50);

        // Create Quit Button
        GameObject quitBtnGO = new GameObject("QuitButton");
        quitBtnGO.transform.SetParent(canvasGO.transform);
        Button quitBtn = quitBtnGO.AddComponent<Button>();
        Image quitImg = quitBtnGO.AddComponent<Image>();
        quitImg.color = Color.white;
        Text quitTxt = new GameObject("Text").AddComponent<Text>();
        quitTxt.transform.SetParent(quitBtnGO.transform);
        quitTxt.text = "Quit Game";
        quitTxt.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
        quitTxt.color = Color.black;
        quitTxt.alignment = TextAnchor.MiddleCenter;
        quitBtn.onClick.AddListener(QuitGame);
        RectTransform quitRect = quitBtnGO.GetComponent<RectTransform>();
        quitRect.sizeDelta = new Vector2(200, 50);
        quitRect.anchoredPosition = new Vector2(0, -50);
    }
}