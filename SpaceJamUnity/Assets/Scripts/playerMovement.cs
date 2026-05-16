using UnityEngine;
using UnityEngine.InputSystem; // Menggunakan Input System baru Unity 6
using System.Collections.Generic;

public class playerMovement : MonoBehaviour
{
    private Rigidbody2D body;
    private Animator anim;
    public float speed = 5f;
    public float jumpForce = 10f;
    private bool isGrounded = false;

    private float moveX;
    private bool recordJumpTrigger = false;

    [Header("Time Ghost Mechanic")]
    [SerializeField] private GameObject ghostPrefab; // Tarik Prefab Bayangan ke sini di Inspector
    private List<ActiveData> currentRunData = new List<ActiveData>();
    private Vector3 startPosition;

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    private void Start()
    {
        // Catat posisi awal spawn player sebagai titik reset
        startPosition = transform.position;
    }

    private void Update()
    {
        moveX = 0f;

        // Membaca input menggunakan Input System Baru (Keyboard)
        if (Keyboard.current != null)
        {
            // Input Horizontal (A/D atau Panah Kiri/Kanan)
            if (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed) moveX = -1f;
            if (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed) moveX = 1f;

            // Input Lompat (Spasi)
            if (Keyboard.current.spaceKey.wasPressedThisFrame && isGrounded)
            {
                Jump();
                recordJumpTrigger = true; // Tandai bahwa frame ini player melompat (untuk rekaman)
            }

            // TOMBOL RESET / REWIND TIME (Tekan R)
            if (Keyboard.current.rKey.wasPressedThisFrame)
            {
                ResetTimeAndSpawnGhost();
            }
        }

        // Flip player ketika gerak kiri-kanan
        if (moveX > 0.01f)
            transform.localScale = Vector3.one;
        else if (moveX < -0.01f)
            transform.localScale = new Vector3(-1, 1, 1);

        // Set animator parameter player utama
        anim.SetBool("Run", moveX != 0);
        anim.SetBool("Grounded", isGrounded);
    }

    private void FixedUpdate()
    {
        // Pergerakan fisik dari kode Anda
        body.linearVelocity = new Vector2(moveX * speed, body.linearVelocity.y);

        // REKAM semua data posisi, arah hadap, dan animasi di setiap frame fisik
        currentRunData.Add(new ActiveData(
            transform.position,
            transform.localScale,
            moveX != 0,
            isGrounded,
            recordJumpTrigger
        ));

        // Reset trigger rekaman lompat setelah dicatat di frame ini
        recordJumpTrigger = false;
    }

    private void Jump()
    {
        body.linearVelocity = new Vector2(body.linearVelocity.x, jumpForce);
        anim.SetTrigger("Jump");
        isGrounded = false;
    }

    private void ResetTimeAndSpawnGhost()
    {
        if (currentRunData.Count > 0)
        {
            // Spawn bayangan di posisi awal
            GameObject newGhost = Instantiate(ghostPrefab, startPosition, Quaternion.identity);

            // Kirim data perjalanan dan animasi ke bayangan
            TimeGhost ghostScript = newGhost.GetComponent<TimeGhost>();
            if (ghostScript != null)
            {
                ghostScript.SetData(new List<ActiveData>(currentRunData));
            }

            // Bersihkan data run saat ini agar siap merekam run baru
            currentRunData = new List<ActiveData>();
        }

        // Kembalikan Player Utama ke posisi awal dan hentikan momentumnya
        transform.position = startPosition;
        body.linearVelocity = Vector2.zero;
        isGrounded = false;
        transform.localScale = Vector3.one;

        // Reset animasi player utama ke posisi idle saat respawn
        anim.SetBool("Run", false);
        anim.SetBool("Grounded", false);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
            isGrounded = true;
    }
}