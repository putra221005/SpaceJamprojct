using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour
{
    [Header("Level Settings")]
    [Tooltip("Nama scene tujuan. Contoh: Level2 atau LevelMenu")]
    public string nextSceneName;

    [Header("Save Data Settings")]
    [Tooltip("Ketik Key untuk membuka level selanjutnya. Contoh: Level2_Terbuka")]
    public string levelUnlockKey;

    [Tooltip("Ketik Key untuk menyimpan bintang level INI. Contoh: Level1_Bintang")]
    public string starSaveKey;

    [Header("Audio Settings (BARU)")]
    [Tooltip("Seret file audio Portal/Level Clear ke sini di Inspector")]
    public AudioClip portalSFX;
    [Range(0f, 1f)] public float volume = 1f; // Pengatur volume di Inspector

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Memastikan yang menyentuh portal adalah Player
        if (collision.CompareTag("Player"))
        {
            LevelSelesai();
        }
    }

    void LevelSelesai()
    {
        // =======================================================
        // BARU: MEMUTAR SFX PORTAL (Sama seperti logika Coin)
        // =======================================================
        if (portalSFX != null)
        {
            // Memaksa posisi Z ke -10f (sejajar kamera) agar suara terdengar sangat dekat dan keras
            Vector3 soundPosition = new Vector3(transform.position.x, transform.position.y, -10f);
            AudioSource.PlayClipAtPoint(portalSFX, soundPosition, volume);
        }

        // 1. CARI STAR MANAGER DAN SIMPAN BINTANG
        StarManager starManager = Object.FindFirstObjectByType<StarManager>();

        if (starManager != null && !string.IsNullOrEmpty(starSaveKey))
        {
            starManager.SaveStarsToPrefs(starSaveKey);
        }

        // 2. BUKA LEVEL SELANJUTNYA
        if (!string.IsNullOrEmpty(levelUnlockKey))
        {
            PlayerPrefs.SetInt(levelUnlockKey, 1);
        }

        // 3. JANGAN LUPA SAVE DATA KE MEMORI
        PlayerPrefs.Save();

        // 4. PINDAH SCENE
        if (!string.IsNullOrEmpty(nextSceneName))
        {
            SceneManager.LoadScene(nextSceneName);
        }
        else
        {
            Debug.LogWarning("Nama scene berikutnya belum diisi di Inspector Portal!");
        }
    }
}