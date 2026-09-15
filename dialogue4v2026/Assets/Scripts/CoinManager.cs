using System.Collections.Generic;
using UnityEngine;

public class CoinManager : MonoBehaviour
{
    public static CoinManager Instance;

    public int currentCoins;

    private Dictionary<int, Coin> coins =
        new Dictionary<int, Coin>();

    private HashSet<int> collectedCoins =
        new HashSet<int>();

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
        if (collectedCoins.Contains(id))
            return;

        collectedCoins.Add(id);

        currentCoins++;
    }

    public void ResetCoins()
    {
        currentCoins = 0;
        collectedCoins.Clear();
    }

    public List<int> GetCollectedCoins()
    {
        return new List<int>(collectedCoins);
    }

    public void LoadCoins(
        int quantidade,
        List<int> moedasColetadas)
    {
        currentCoins = quantidade;

        collectedCoins.Clear();

        if (moedasColetadas != null)
        {
            foreach (int id in moedasColetadas)
            {
                collectedCoins.Add(id);
            }
        }

        foreach (Coin coin in coins.Values)
        {
            if (collectedCoins.Contains(coin.coinID))
            {
                coin.gameObject.SetActive(false);
            }
            else
            {
                coin.gameObject.SetActive(true);
            }
        }
    }
}