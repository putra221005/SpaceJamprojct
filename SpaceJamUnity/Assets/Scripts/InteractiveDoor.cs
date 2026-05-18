using UnityEngine;
using System.Collections.Generic; // Wajib untuk menggunakan List

public class InteractiveDoor : MonoBehaviour
{
    [System.Serializable]
    public struct DoorSpriteParts
    {
        public SpriteRenderer spriteRenderer; // Tempat narik komponen Sprite Renderer bagian laser
        public Sprite lockedSprite;          // Sprite saat MERAH/Terkunci untuk bagian ini
        public Sprite openedSprite;          // Sprite saat ABU-ABU/Terbuka untuk bagian ini
    }

    [Header("Laser Parts Settings")]
    [SerializeField] private List<DoorSpriteParts> doorParts; // Daftar semua bagian pintu laser

    private List<BoxCollider2D> doorColliders = new List<BoxCollider2D>();

    void Awake()
    {
        // Otomatis mencari dan mengumpulkan semua BoxCollider2D yang ada di objek ini beserta anak-anaknya
        BoxCollider2D[] colliders = GetComponentsInChildren<BoxCollider2D>();
        doorColliders.AddRange(colliders);

        // Mulai game dengan kondisi terkunci (false)
        SetDoorState(false);
    }

    // Fungsi ini yang dipanggil oleh script ButtonForDoor
    public void SetDoorState(bool isOpen)
    {
        // Looping/Ulangi perintah untuk setiap bagian sprite laser yang terdaftar
        foreach (var part in doorParts)
        {
            if (part.spriteRenderer != null)
            {
                // Jika diganti terbuka, ubah ke sprite terbuka. Jika ditutup, kembalikan ke sprite merah.
                part.spriteRenderer.sprite = isOpen ? part.openedSprite : part.lockedSprite;
            }
        }

        // Jalankan perintah mati/nyalakan collider untuk semua bagian collider yang ditemukan
        foreach (var col in doorColliders)
        {
            if (col != null)
            {
                // Jika terbuka, collider MATI (enabled = false). Jika tertutup, collider NYALA (enabled = true).
                col.enabled = !isOpen;
            }
        }
    }
}