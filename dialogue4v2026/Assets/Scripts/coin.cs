using UnityEngine;

public class Coin : MonoBehaviour
{
    public int coinID;

    private void Start()
    {
        CoinManager.Instance.RegisterCoin(this);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        CoinManager.Instance.CollectCoin(coinID);

        gameObject.SetActive(false);
    }
}