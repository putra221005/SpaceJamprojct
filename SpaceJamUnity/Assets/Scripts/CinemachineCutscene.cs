using UnityEngine;
using UnityEngine.Playables;

public class CinemachineCutsceneUnity6 : MonoBehaviour
{
    [Header("Timeline / Sequencing")]
    public PlayableDirector timelineDirector;

    [Header("Pengaturan 2 Karakter (Cutscene Swap)")]
    public GameObject realPlayer;
    public GameObject cutscenePlayer;

    [Header("Pengaturan Keycaps Prompt")]
    [Tooltip("Tarik GameObject Keycaps Prompt (yang ada Sprite Renderer tombol) ke sini")]
    public GameObject keycapsPrompt; // Tambah variabel ini

    private bool hasPlayed = false;

    private void Start()
    {
        if (cutscenePlayer != null)
            cutscenePlayer.SetActive(false);

        // Pastikan di awal permainan objek keycaps dalam keadaan mati
        if (keycapsPrompt != null)
            keycapsPrompt.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !hasPlayed)
        {
            StartCutscene();
        }
    }

    void StartCutscene()
    {
        hasPlayed = true;

        if (realPlayer != null) realPlayer.SetActive(false);
        if (cutscenePlayer != null) cutscenePlayer.SetActive(true);

        if (timelineDirector != null)
        {
            timelineDirector.Play();
            timelineDirector.stopped += OnCutsceneFinished;
        }
    }

    void OnCutsceneFinished(PlayableDirector director)
    {
        if (cutscenePlayer != null) cutscenePlayer.SetActive(false);

        if (realPlayer != null)
        {
            if (cutscenePlayer != null)
            {
                realPlayer.transform.position = cutscenePlayer.transform.position;
            }

            realPlayer.SetActive(true);
        }

        // BARU: Munculkan petunjuk tombol/keycaps setelah cutscene selesai
        if (keycapsPrompt != null)
        {
            keycapsPrompt.SetActive(true);
        }

        timelineDirector.stopped -= OnCutsceneFinished;

        // JANGAN gunakan Destroy(gameObject) di sini dulu jika script ini menempel pada trigger,
        // atau pastikan script KeycapsWatcher ditaruh di objek keycaps-nya sendiri agar aman.
        Destroy(gameObject);
    }
}