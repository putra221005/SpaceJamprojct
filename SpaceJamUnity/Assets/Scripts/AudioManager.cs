using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("Audio Sources")]
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource SFX;

    [Header("Audio Clips (Sekarang Public agar bisa diakses script lain)")]
    [SerializeField] private AudioClip background; // Tetap private tidak apa-apa karena hanya dipakai di script ini (fungsi Start)

    // UBAH MENJADI PUBLIC DI SINI:
    public AudioClip Walk;
    public AudioClip Jump;
    public AudioClip Collect;
    public AudioClip Portal;

    private void Start()
    {
        if (musicSource != null && background != null)
        {
            musicSource.clip = background;
            musicSource.Play();
        }
    }

    public void PlaySFX(AudioClip clip)
    {
        if (SFX != null && clip != null)
        {
            SFX.PlayOneShot(clip);
        }
    }
}