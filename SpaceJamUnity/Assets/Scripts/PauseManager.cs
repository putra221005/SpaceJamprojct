using UnityEngine;
using UnityEngine.SceneManagement; // Digunakan untuk pindah scene dan restart

public class PauseManager : MonoBehaviour
{
    [Header("UI Panel")]
    public GameObject pausePanel; // Seret objek PausePanel ke sini

    private bool isPaused = false;

    void Update()
    {
        // Fitur opsional: Pemain juga bisa menekan tombol Escape (ESC) di keyboard untuk pause
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
                ResumeGame();
            else
                PauseGame();
        }
    }

    // 1. FUNGSI UNTUK MEMBUKA MENU PAUSE
    public void PauseGame()
    {
        pausePanel.SetActive(true); // Memunculkan panel pause di layar
        Time.timeScale = 0f;        // Menghentikan total waktu game (game membeku)
        isPaused = true;
    }

    // 2. FUNGSI TOMBOL RESUME (HIJAU)
    public void ResumeGame()
    {
        pausePanel.SetActive(false); // Menyembunyikan panel pause
        Time.timeScale = 1f;         // Menjalankan kembali waktu game normal
        isPaused = false;
    }

    // 3. FUNGSI TOMBOL RESTART (ABU-ABU BULAT)
    public void RestartGame()
    {
        Time.timeScale = 1f; // WAJIB mengembalikan waktu ke normal sebelum memuat ulang scene

        // Mengambil nama scene aktif saat ini secara dinamis lalu memuatnya kembali dari awal
        string sceneSekarang = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(sceneSekarang);
    }

    // 4. FUNGSI TOMBOL PANAH KEMBALI (MERAH)
    public void ExitToLevelMenu()
    {
        Time.timeScale = 1f; // WAJIB mengembalikan waktu ke normal sebelum keluar
        SceneManager.LoadScene("LevelMenu"); // Kembali ke scene menu pemilihan level
    }
}