using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement; // WAJIB ditambahkan untuk perpindahan scene

public class EndingCutsceneHandler : MonoBehaviour
{
    [Header("Timeline Component")]
    public PlayableDirector timelineDirector; // Tarik objek Timeline ending ke sini

    [Header("Pengaturan Scene Tujuan")]
    [Tooltip("Tuliskan NAMA EXACT dari scene Main Menu kamu di Unity")]
    public string mainMenuSceneName = "MainMenu";

    void OnEnable()
    {
        // Daftarkan fungsi ke event 'stopped' milik Timeline
        if (timelineDirector != null)
        {
            timelineDirector.stopped += OnEndingCutsceneFinished;
        }
    }

    // Fungsi ini otomatis dipanggil Unity begitu cutscene tamat/menyentuh detik terakhir
    void OnEndingCutsceneFinished(PlayableDirector director)
    {
        Debug.Log("Cutscene tamat! Mengalihkan ke scene: " + mainMenuSceneName);

        // Putus hubungan event agar memori bersih sebelum pindah scene
        if (timelineDirector != null)
        {
            timelineDirector.stopped -= OnEndingCutsceneFinished;
        }

        // Pindah ke scene Menu Utama
        SceneManager.LoadScene(mainMenuSceneName);
    }

    void OnDisable()
    {
        // Pengaman ekstra jika objek dinonaktifkan di tengah jalan
        if (timelineDirector != null)
        {
            timelineDirector.stopped -= OnEndingCutsceneFinished;
        }
    }
}