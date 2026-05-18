using UnityEngine;
using UnityEngine.InputSystem;

public class PressurePlate : MonoBehaviour
{
    [Header("Connect to Bridge & UI")]
    [SerializeField] private MovingBridge targetBridge; // Tarik objek Jembatan ke sini
    [SerializeField] private GameObject interactUI;     // Tarik objek Text UI "Press F" ke sini

    [Header("Visual Feedback (Optional)")]
    [SerializeField] private Sprite pressedSprite;      // Gambar tombol saat mendem/aktif
    [SerializeField] private Sprite defaultSprite;      // Gambar tombol saat timbul/mati

    private SpriteRenderer sr;
    private bool isPlayerInside = false;
    private bool isBridgeUp = false; // Menyimpan status jembatan saat ini (apakah sedang di atas?)

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        // KONTROL UNTUK PLAYER UTAMA
        if (isPlayerInside)
        {
            if (Keyboard.current != null && Keyboard.current.fKey.wasPressedThisFrame)
            {
                // Bolak-balik status jembatan (jika true jadi false, jika false jadi true)
                isBridgeUp = !isBridgeUp;

                if (isBridgeUp)
                {
                    // AKSI 1: JEMBATAN NAIK
                    if (pressedSprite != null) sr.sprite = pressedSprite;
                    if (targetBridge != null) targetBridge.TriggerBridgeActivation();
                }
                else
                {
                    // AKSI 2: JEMBATAN TURUN
                    if (defaultSprite != null) sr.sprite = defaultSprite;
                    if (targetBridge != null) targetBridge.TriggerBridgeDeactivation();
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Munculkan UI "Press F" saat player mendekat
        if (collision.CompareTag("Player"))
        {
            isPlayerInside = true;
            if (interactUI != null) interactUI.SetActive(true);
        }

        // KONTROL UNTUK CLONE (Tag: Ground)
        // Clone otomatis memicu kebalikan dari status jembatan saat ini 
        // ketika dia sampai di area tombol di masa lalu
        if (collision.CompareTag("Ground"))
        {
            isBridgeUp = !isBridgeUp;

            if (isBridgeUp)
            {
                if (pressedSprite != null) sr.sprite = pressedSprite;
                if (targetBridge != null) targetBridge.TriggerBridgeActivation();
            }
            else
            {
                if (defaultSprite != null) sr.sprite = defaultSprite;
                if (targetBridge != null) targetBridge.TriggerBridgeDeactivation();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // Sembunyikan UI "Press F" saat player menjauh
        if (collision.CompareTag("Player"))
        {
            isPlayerInside = false;
            if (interactUI != null) interactUI.SetActive(false);
        }
    }
}