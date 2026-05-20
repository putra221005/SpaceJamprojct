using UnityEngine;
using UnityEngine.Playables; // Wajib untuk mengontrol PlayableDirector (Timeline)

public class CinemachineCutsceneUnity6 : MonoBehaviour
{
    [Header("Timeline / Sequencing")]
    // Kita ganti CinemachineCamera menjadi PlayableDirector karena kamera sudah dikontrol di dalam Timeline
    public PlayableDirector timelineDirector;

    [Header("Pengaturan 2 Karakter (Cutscene Swap)")]
    public GameObject realPlayer;          // Karakter utama yang kamu mainkan sehari-hari
    public GameObject cutscenePlayer;      // Karakter dummy/clone khusus untuk animasi cutscene

    private bool hasPlayed = false;

    private void Start()
    {
        // Saat game baru mulai, pastikan karakter dummy cutscene dalam keadaan mati/sembunyi
        if (cutscenePlayer != null)
            cutscenePlayer.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Cek apakah yang menginjak trigger adalah player asli dan cutscene belum pernah jalan
        if (other.CompareTag("Player") && !hasPlayed)
        {
            StartCutscene();
        }
    }

    void StartCutscene()
    {
        hasPlayed = true;

        // 1. Matikan karakter asli agar pemain tidak bisa menggerakkannya saat cutscene
        if (realPlayer != null) realPlayer.SetActive(false);

        // 2. Munculkan karakter khusus cutscene yang sudah kamu pasangi animasi di Timeline
        if (cutscenePlayer != null) cutscenePlayer.SetActive(true);

        // 3. Jalankan Timeline (Kamera Cinemachine, animasi bicaranya, dan teks monolog akan jalan dari sini)
        if (timelineDirector != null)
        {
            timelineDirector.Play();

            // Daftarkan fungsi ke event 'stopped' agar script tahu kapan Timeline ini selesai
            timelineDirector.stopped += OnCutsceneFinished;
        }
    }

    // Fungsi ini otomatis dipanggil oleh Unity saat durasi Timeline kamu habis
    void OnCutsceneFinished(PlayableDirector director)
    {
        // 4. Sembunyikan/matikan kembali karakter khusus cutscene
        if (cutscenePlayer != null) cutscenePlayer.SetActive(false);

        if (realPlayer != null)
        {
            // PRO-TIP: Samakan posisi karakter asli dengan posisi terakhir karakter cutscene berjalan
            // supaya pas cutscene selesai, karaktermu tidak "teleportasi" balik ke posisi awal trigger.
            if (cutscenePlayer != null)
            {
                realPlayer.transform.position = cutscenePlayer.transform.position;
            }

            // 5. Hidupkan kembali karakter asli. Kontrol gameplay kembali normal!
            realPlayer.SetActive(true);
        }

        // Putus hubungan event agar memori tetap bersih
        timelineDirector.stopped -= OnCutsceneFinished;

        // 6. Hancurkan objek trigger ini karena cutscene sudah beres
        Destroy(gameObject);
    }
}