using UnityEngine;

public class PortalSpawner : MonoBehaviour
{
    [Header("References")]
    public CoinManager coinManager; // Taruh objek CoinManager di sini nanti via Inspector

    [Header("Portal Settings")]
    public GameObject portalPrefab;
    public float spawnDistance = 2f;
    public KeyCode spawnKey = KeyCode.E;

    private GameObject currentPortal;

    void Update()
    {
        if (Input.GetKeyDown(spawnKey))
        {
            // Pengecekan: Apakah Coin Manager ada, dan apakah semua koin sudah terkumpul?
            if (coinManager != null && coinManager.AreAllCoinsCollected())
            {
                SpawnPortal();
            }
            else
            {
                // Opsional: Kamu bisa memunculkan teks peringatan di UI game-mu di sini
                Debug.Log("Koin belum terkumpul semua! Kamu tidak bisa mengeluarkan portal.");
            }
        }
    }

    void SpawnPortal()
    {
        if (currentPortal != null)
        {
            Destroy(currentPortal);
        }

        float direction = Mathf.Sign(transform.localScale.x);
        Vector3 spawnPosition = transform.position + new Vector3(direction * spawnDistance, 0f, 0f);

        currentPortal = Instantiate(portalPrefab, spawnPosition, Quaternion.identity);
        Debug.Log("Portal berhasil dikeluarkan!");
    }
}