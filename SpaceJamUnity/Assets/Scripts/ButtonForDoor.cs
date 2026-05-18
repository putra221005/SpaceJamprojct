using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic; // Wajib ditambahkan untuk menggunakan List

public class ButtonForDoor : MonoBehaviour
{
    [Header("Connect to Doors (Bisa Banyak Pintu)")]
    // MENGUBAH MENJADI LIST: Sekarang bisa menampung lebih dari 1 pintu
    [SerializeField] private List<InteractiveDoor> targetDoors;

    [Header("Connect to UI")]
    [SerializeField] private GameObject interactUI;

    [Header("Visual Feedback (Optional)")]
    [SerializeField] private Sprite pressedSprite;
    [SerializeField] private Sprite defaultSprite;

    private SpriteRenderer sr;
    private bool isPlayerInside = false;
    private bool isDoorOpen = false;

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
                ToggleDoorState();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerInside = true;
            if (interactUI != null) interactUI.SetActive(true);
        }

        // KONTROL UNTUK CLONE (Tag: Ground)
        if (collision.CompareTag("Ground"))
        {
            ToggleDoorState();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerInside = false;
            if (interactUI != null) interactUI.SetActive(false);
        }
    }

    private void ToggleDoorState()
    {
        isDoorOpen = !isDoorOpen;

        // Ubah visual tombol fisik
        if (isDoorOpen)
        {
            if (pressedSprite != null) sr.sprite = pressedSprite;
        }
        else
        {
            if (defaultSprite != null) sr.sprite = defaultSprite;
        }

        // LOOPING: Kirim sinyal buka/tutup ke SEMUA pintu yang terdaftar di List
        foreach (var door in targetDoors)
        {
            if (door != null)
            {
                door.SetDoorState(isDoorOpen);
            }
        }
    }
}