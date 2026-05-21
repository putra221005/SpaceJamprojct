using UnityEngine;
using UnityEngine.InputSystem; // Menggunakan Input System baru Unity 6
using System.Collections.Generic;

public class playerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float jumpForce;
    private float moveInput;
    private bool isFacingRight = true;

    [Header("Ground Check (Updated to BoxCast)")]
    public Collider2D playerCollider; // Drag BoxCollider2D / CapsuleCollider2D Player ke sini via Inspector
    public float castDistance = 0.1f;  // Seberapa jauh sensor menembak ke bawah kaki
    public LayerMask groundLayer;

    [Header("UI & Managers")]
    public StarManager starManager;

    private Rigidbody2D rb;
    private Animator anim;
    private bool isGrounded;
    private bool recordJumpTrigger = false;

    [Header("Time Ghost Mechanic")]
    [SerializeField] private GameObject ghostPrefab;
    private List<ActiveData> currentRunData = new List<ActiveData>();
    private Vector3 startPosition;

    public CoinManager cm;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        // Pengaman otomatis jika lupa drag collider di Inspector
        if (playerCollider == null)
        {
            playerCollider = GetComponent<Collider2D>();
        }

        startPosition = transform.position;
    }

    void Update()
    {
        moveInput = 0f;

        if (Keyboard.current != null)
        {
            if (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed) moveInput = -1f;
            if (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed) moveInput = 1f;

            if (Keyboard.current.spaceKey.wasPressedThisFrame && isGrounded)
            {
                // Menggunakan Unity 6 Rigidbody2D linearVelocity
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
                anim.SetTrigger("Jump");
                recordJumpTrigger = true;
            }

            if (Keyboard.current.rKey.wasPressedThisFrame)
            {
                ResetTimeAndSpawnGhost();
            }
        }

        Flip();

        anim.SetBool("Run", moveInput != 0f);
        anim.SetBool("Grounded", isGrounded);
    }

    private void FixedUpdate()
    {
        // PERBAIKAN BUG: Menggunakan BoxCast agar deteksi murni ke bawah sumbu objek, bukan membulat ke samping
        isGrounded = CheckIfGrounded();

        rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);

        currentRunData.Add(new ActiveData(
            transform.position,
            isFacingRight,
            moveInput != 0f,
            isGrounded,
            recordJumpTrigger
        ));

        recordJumpTrigger = false;
    }

    // Fungsi pembantu baru untuk BoxCast deteksi bawah
    private bool CheckIfGrounded()
    {
        if (playerCollider == null) return false;

        // Menembakkan kotak bayangan dari posisi collider ke arah bawah (Vector2.down)
        // Ukuran lebar kotak disesuaikan dengan ukuran collider player agar presisi
        RaycastHit2D hit = Physics2D.BoxCast(
            playerCollider.bounds.center,
            playerCollider.bounds.size,
            0f,
            Vector2.down,
            castDistance,
            groundLayer
        );

        // Mengembalikan nilai true jika mendeteksi groundlayer tepat di bawah kaki
        return hit.collider != null;
    }

    private void ResetTimeAndSpawnGhost()
    {
        if (currentRunData.Count > 0)
        {
            GameObject newGhost = Instantiate(ghostPrefab, startPosition, Quaternion.identity);
            TimeGhost ghostScript = newGhost.GetComponent<TimeGhost>();
            if (ghostScript != null)
            {
                ghostScript.SetData(new List<ActiveData>(currentRunData));
            }

            if (starManager != null)
            {
                starManager.RecordCloneUsage();
            }

            currentRunData = new List<ActiveData>();
        }

        transform.position = startPosition;
        rb.linearVelocity = Vector2.zero;

        isFacingRight = true;
        Vector3 localScale = transform.localScale;
        localScale.x = Mathf.Abs(localScale.x);
        transform.localScale = localScale;

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
}