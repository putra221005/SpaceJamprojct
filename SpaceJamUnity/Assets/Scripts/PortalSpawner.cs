using UnityEngine;

public class PortalSpawner : MonoBehaviour
{
    [Header("References")]
    public CoinManager coinManager;

    [Header("Portal Settings")]
    public GameObject portalPrefab;
    public float spawnDistance = 2f;
    public KeyCode spawnKey = KeyCode.E;
    public Animator playerAnim;

    [Header("Audio Settings")] // TAMBAHAN SFX
    public AudioSource portalAudioSource; // Hubungkan dengan AudioSource di Player via Inspector
    public AudioClip spawnPortalSFX;     // Seret file audio portal ke sini

    private GameObject currentPortal;

    void Update()
    {
        if (Input.GetKeyDown(spawnKey))
        {
            if (coinManager != null && coinManager.AreAllCoinsCollected())
            {
                if (playerAnim != null)
                {
                    playerAnim.SetTrigger("TembakPortal");
                }

                // Jika kamu memanggil instant spawn langsung di sini:
                // SpawnPortal(); 
            }
            else
            {
                Debug.Log("Koin belum terkumpul!");
            }
        }
    }

    public void SpawnPortal() // Diubah ke public jika dipanggil lewat Animation Event
    {
        if (currentPortal != null)
        {
            Destroy(currentPortal);
        }

        // TAMBAHAN SFX: Mainkan suara portal keluar
        if (portalAudioSource != null && spawnPortalSFX != null)
        {
            portalAudioSource.PlayOneShot(spawnPortalSFX);
        }

        float direction = Mathf.Sign(transform.localScale.x);
        Vector3 spawnPosition = transform.position + new Vector3(direction * spawnDistance, 0f, 0f);

        currentPortal = Instantiate(portalPrefab, spawnPosition, Quaternion.identity);

        Animator portalAnim = currentPortal.GetComponent<Animator>();
        if (portalAnim != null)
        {
            portalAnim.Rebind();
            portalAnim.Update(0f);
        }

        Debug.Log("Portal berhasil dikeluarkan!");
    }
}