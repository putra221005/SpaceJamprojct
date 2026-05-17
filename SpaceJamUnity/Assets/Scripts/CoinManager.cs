using UnityEngine;
using TMPro;

public class CoinManager : MonoBehaviour
{
    public int coinCount;
    public TextMeshProUGUI coinText;

    void Start()
    {
        // Menghitung otomatis jumlah semua koin yang ada di map saat game mulai
        // Pastikan semua objek koin kamu sudah diberi komponen 'CoinObject'
        coinCount = FindObjectsByType<Coin>(FindObjectsSortMode.None).Length;
    }

    void Update()
    {
        if (coinText != null)
        {
            coinText.text = ": " + coinCount.ToString();
        }
    }

    public bool AreAllCoinsCollected()
    {
        return coinCount <= 0;
    }
}