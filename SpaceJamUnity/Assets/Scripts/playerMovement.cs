using UnityEngine;
using UnityEngine.InputSystem; // Menggunakan Input System baru Unity 6
using System.Collections.Generic;

public class playerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float jumpForce = 10f;
    private float moveInput;
    private bool isFacingRight = true;

    [Header("Ground Check")]
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;

    private Rigidbody2D rb;
    private Animator anim; // Tambahan komponen Animator
    private bool isGrounded;
    private bool recordJumpTrigger = false; // Untuk mencatat trigger lompat ke data Clone

    [Header("Time Ghost Mechanic")]
    [SerializeField] private GameObject ghostPrefab; // Tarik Prefab Bayangan Anda ke sini di Inspector
    private List<ActiveData> currentRunData = new List<ActiveData>();
    private Vector3 startPosition;

    public CoinManager cm;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>(); // Ambil komponen Animator

        // Catat posisi awal sebagai tempat checkpoint/reset
        startPosition = transform.position;
    }

    void Update()
    {
        moveInput = 0f;

        // Membaca input menggunakan Input System Baru (Keyboard)
        if (Keyboard.current != null)
        {
            // Input Horizontal (A/D atau Panah Kiri/Kanan)
            if (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed) moveInput = -1f;
            if (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed) moveInput = 1f;

            // Input Lompat (Spasi)
            if (Keyboard.current.spaceKey.wasPressedThisFrame && isGrounded)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
                anim.SetTrigger("Jump"); // Mainkan animasi lompat player utama
                recordJumpTrigger = true; // Tandai frame ini melompat untuk direkam
            }

            // TOMBOL RESET / TIME REWIND (Tekan R)
            if (Keyboard.current.rKey.wasPressedThisFrame)
            {
                ResetTimeAndSpawnGhost();
            }
        }

        // Mengatur arah hadap sprite karakter (Flip)
        Flip();

        // Set animator parameter player utama sesuai pergerakan Anda
        anim.SetBool("Run", moveInput != 0f);
        anim.SetBool("Grounded", isGrounded);
    }

    private void FixedUpdate()
    {
        // Ground Check milik Anda
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        // Pergerakan fisik dipindah ke FixedUpdate agar sinkron dengan perekaman data fisik
        rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);

        // REKAM posisi, arah hadap, dan animasi Anda di setiap frame fisik
        currentRunData.Add(new ActiveData(
            transform.position,
            isFacingRight,
            moveInput != 0f,
            isGrounded,
            recordJumpTrigger
        ));

        // Reset status record jump setelah sukses dicatat pada frame ini
        recordJumpTrigger = false;
    }

    private void ResetTimeAndSpawnGhost()
    {
        // Hanya buat bayangan jika player sempat bergerak (ada data yang direkam)
        if (currentRunData.Count > 0)
        {
            // Spawn bayangan di posisi awal
            GameObject newGhost = Instantiate(ghostPrefab, startPosition, Quaternion.identity);

            // Kirim rekaman perjalanan kita ke bayangan tersebut
            TimeGhost ghostScript = newGhost.GetComponent<TimeGhost>();
            if (ghostScript != null)
            {
                ghostScript.SetData(new List<ActiveData>(currentRunData));
            }

            // Reset data run saat ini agar siap merekam perjalanan yang baru
            currentRunData = new List<ActiveData>();
        }

        // Kembalikan Player Utama ke posisi awal
        transform.position = startPosition;
        rb.linearVelocity = Vector2.zero;

        // Reset arah hadap player ke kanan semula
        isFacingRight = true;
        Vector3 localScale = transform.localScale;
        localScale.x = Mathf.Abs(localScale.x);
        transform.localScale = localScale;

        // Reset animasi player utama saat respawn agar kembali ke Idle
        anim.SetBool("Run", false);
        anim.SetBool("Grounded", false);
    }

    private void Flip()
    {
        if ((isFacingRight && moveInput < 0f) || (!isFacingRight && moveInput > 0f))
        {
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Coin"))
        {
            Destroy(other.gameObject);
            cm.coinCaunt++;
        }
    }
}