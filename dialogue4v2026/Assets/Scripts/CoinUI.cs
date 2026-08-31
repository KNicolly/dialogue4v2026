using TMPro;
using UnityEngine;

public class CoinUI : MonoBehaviour
{
    public TMP_Text coinText;

    private void Update()
    {
        if (CoinManager.Instance == null)
            return;

        coinText.text = "Moedas: " + CoinManager.Instance.currentCoins;
    }
}