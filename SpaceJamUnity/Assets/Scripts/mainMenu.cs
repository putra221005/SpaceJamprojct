using UnityEngine;
using UnityEngine.SceneManagement; // WAJIB dimasukkan untuk sistem pindah scene

public class mainMenu : MonoBehaviour
{
    // Fungsi untuk tombol PLAY
    public void PlayGame()
    {
        // Membuka scene selanjutnya berdasarkan nama di Build Settings
        // Sesuai daftar kamu, kita akan membuka "LevelMenu"
        SceneManager.LoadScene("LevelMenu");
    }

    // Fungsi untuk tombol QUIT

    public void BackToMainMenu()
    {
        // Membuka kembali scene bernama "Menu" sesuai daftar Build Settings kamu
        SceneManager.LoadScene("Menu");
    }
    public void QuitGame()
{
    Debug.Log("Game berhasil ditutup!");

    // Jika dijalankan di dalam Unity Editor, matikan mode Play
    #if UNITY_EDITOR
    UnityEditor.EditorApplication.isPlaying = false;
    #else
    Application.Quit();
    #endif
}
}