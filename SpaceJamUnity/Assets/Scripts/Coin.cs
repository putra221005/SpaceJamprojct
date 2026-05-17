using UnityEngine;

public class Coin : MonoBehaviour
{
    private CoinManager coinManager;
    private bool isAlreadyCollected = false;

    void Start()
    {
        // Mencari CoinManager di dalam scene secara otomatis
        coinManager = FindFirstObjectByType<CoinManager>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Koin hanya bisa diambil jika belum pernah diambil pada run ini
        // Dan objek yang menabraknya wajib memiliki Tag "Player"
        if (!isAlreadyCollected && collision.CompareTag("Player"))
        {
            isAlreadyCollected = true; // Kunci agar tidak tidak sengaja terhitung double

            if (coinManager != null)
            {
                coinManager.coinCount--; // Kurangi angka koin di manager
            }

            Destroy(gameObject); // Koin menghancurkan dirinya sendiri secara permanen
        }
    }
}