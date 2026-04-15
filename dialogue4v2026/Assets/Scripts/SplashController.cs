using UnityEngine;
using System.Collections;

public class SplashController : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(WaitAndLoad());
    }

    IEnumerator WaitAndLoad()
    {
        yield return new WaitForSeconds(2f);

        GameManager.Instance.ChangeState(GameManager.GameState.MenuPrincipal);
        GameManager.Instance.LoadScene("MenuPrincipal");
    }
}