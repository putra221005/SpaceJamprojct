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
        // 1. CARI STAR MANAGER DAN SIMPAN BINTANG
        // Menggunakan FindFirstObjectByType (Fitur Unity baru) agar otomatis mencari StarManager di scene
        StarManager starManager = Object.FindFirstObjectByType<StarManager>();

        if (starManager != null && !string.IsNullOrEmpty(starSaveKey))
        {
            // Simpan jumlah bintang sesuai kalkulasi clone di StarManager
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