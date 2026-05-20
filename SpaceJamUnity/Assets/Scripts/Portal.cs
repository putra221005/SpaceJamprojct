using UnityEngine;
using UnityEngine.SceneManagement; // Wajib diimport untuk pindah scene

public class Portal : MonoBehaviour
{
    [Header("Level Settings")]
    [Tooltip("Nama scene tujuan. Pastikan nama sesuai dan sudah dimasukkan ke Build Settings!")]
    public string nextSceneName;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Memastikan yang menyentuh portal adalah Player
        if (collision.CompareTag("Player"))
        {
            TeleportToNextStage();
        }
    }

    void TeleportToNextStage()
    {
        if (!string.IsNullOrEmpty(nextSceneName))
        {
            SceneManager.LoadScene(nextSceneName);
        }
        else
        {
            Debug.LogWarning("Nama scene berikutnya belum diisi di Inspector Portal!");
        }
    }
    void LevelSelesai()

    {
        // 1. Beritahu Unity kalau Level 2 sekarang sudah boleh terbuka
        PlayerPrefs.SetInt("Level2_Terbuka", 1);
        // 2. Simpan jumlah bintang yang didapat player di Level 1 (misal dapet 3 bintang)
        int totalBintangYangDidapat = 3; // Kamu bisa ganti ini pakai variabel koin kamu
        PlayerPrefs.SetInt("Level1_Bintang", totalBintangYangDidapat);
        // Jangan lupa di-save datanya
        PlayerPrefs.Save();
        // Baru pindah scene atau kembali ke LevelMenu
        UnityEngine.SceneManagement.SceneManager.LoadScene("LevelMenu");
    }
}