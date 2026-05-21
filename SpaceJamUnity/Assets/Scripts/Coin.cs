using UnityEngine;

public class Coin : MonoBehaviour
{
    private CoinManager coinManager;
    private bool isAlreadyCollected = false;

    [Header("Audio Settings")] // TAMBAHAN SFX
    public AudioClip collectSFX;   // Seret file audio koin ke sini di Inspector

    void Start()
    {
        coinManager = FindFirstObjectByType<CoinManager>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isAlreadyCollected && collision.CompareTag("Player"))
        {
            isAlreadyCollected = true;

            // TAMBAHAN SFX: Memutar suara di posisi koin sebelum objeknya hancur
            if (collectSFX != null)
            {
                AudioSource.PlayClipAtPoint(collectSFX, transform.position);
            }

            if (coinManager != null)
            {
                coinManager.coinCount--;
            }

            Destroy(gameObject);
        }
    }
}