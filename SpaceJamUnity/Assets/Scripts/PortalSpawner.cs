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

    private GameObject currentPortal;

    void Update()
    {
        if (Input.GetKeyDown(spawnKey))
        {
            if (coinManager != null && coinManager.AreAllCoinsCollected())
            {
                // JALANKAN ANIMASI PROFESSOR MENEMBAK
                if (playerAnim != null)
                {
                    playerAnim.SetTrigger("TembakPortal");
                }
            }
            else
            {
                Debug.Log("Koin belum terkumpul!");
            }
        }
    }

    void SpawnPortal()
    {
        if (currentPortal != null)
        {
            Destroy(currentPortal);
        }

        float direction = Mathf.Sign(transform.localScale.x);
        Vector3 spawnPosition = transform.position + new Vector3(direction * spawnDistance, 0f, 0f);

        currentPortal = Instantiate(portalPrefab, spawnPosition, Quaternion.identity);

        // TAMBAHAN AMAN: Memastikan komponen Animator di portal langsung aktif memainkan animasi defaultnya
        Animator portalAnim = currentPortal.GetComponent<Animator>();
        if (portalAnim != null)
        {
            // Jika kamu ingin memaksa animasi memutar dari awal dari script
            portalAnim.Rebind();
            portalAnim.Update(0f);
        }

        Debug.Log("Portal berhasil dikeluarkan!");
    }
}