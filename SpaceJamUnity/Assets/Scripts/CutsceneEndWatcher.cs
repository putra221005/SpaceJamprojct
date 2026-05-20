using UnityEngine;
using UnityEngine.Playables;

public class CutsceneEndWatcher : MonoBehaviour
{
    [Header("Timeline Component")]
    public PlayableDirector timelineDirector;

    [Header("Karakter Swap Settings")]
    public GameObject realPlayer;
    public GameObject cutsceneDummy;
    public MonoBehaviour playerMovementScript;

    [Header("Portal Settings")]
    public GameObject portalObject; // Tarik objek Portal yang ada di Hierarchy ke sini

    void OnEnable()
    {
        if (timelineDirector != null)
        {
            timelineDirector.stopped += OnCutsceneFinished;
        }
    }

    void OnCutsceneFinished(PlayableDirector director)
    {
        // 1. SOLUSI PORTAL: Hancurkan objek portal dari Hierarchy agar bersih total
        if (portalObject != null)
        {
            Destroy(portalObject);
            // Atau jika ingin sekadar mematikan: portalObject.SetActive(false);
        }

        // 2. Matikan/Sembunyikan karakter dummy
        if (cutsceneDummy != null)
            cutsceneDummy.SetActive(false);

        if (realPlayer != null)
        {
            // 3. Samakan posisi koordinat Player asli dengan titik terakhir dummy berhenti
            if (cutsceneDummy != null)
            {
                realPlayer.transform.position = cutsceneDummy.transform.position;
            }

            // 4. Munculkan kembali karakter asli kamu di layar
            realPlayer.SetActive(true);
        }

        // 5. Aktifkan script pergerakan agar pemain bisa kontrol lagi
        if (playerMovementScript != null)
        {
            playerMovementScript.enabled = true;
        }

        // Putus hubungan event agar memori bersih
        if (timelineDirector != null)
        {
            timelineDirector.stopped -= OnCutsceneFinished;
        }

        this.enabled = false;
    }

    void OnDisable()
    {
        if (timelineDirector != null)
        {
            timelineDirector.stopped -= OnCutsceneFinished;
        }
    }
}