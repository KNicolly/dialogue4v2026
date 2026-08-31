using System.Collections.Generic;
using UnityEngine;

public class CoinManager : MonoBehaviour
{
    public static CoinManager Instance;

    public int currentCoins;

    private Dictionary<int, Coin> coins = new Dictionary<int, Coin>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void RegisterCoin(Coin coin)
    {
        if (!coins.ContainsKey(coin.coinID))
        {
            coins.Add(coin.coinID, coin);
        }
    }

    public void CollectCoin(int id)
    {
        currentCoins++;
    }

    public void ResetCoins()
    {
        currentCoins = 0;
    }
}