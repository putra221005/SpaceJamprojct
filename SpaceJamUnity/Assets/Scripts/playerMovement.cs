using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class playerMovement : MonoBehaviour
{
    [Header("Audio Settings")]
    public AudioSource movementAudioSource;
    public AudioClip jumpSFX;
    public AudioClip walkSFX;
    public float walkStepDelay = 0.35f;
    private float walkTimer;

    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float jumpForce;
    private float moveInput;
    private bool isFacingRight = true;

    [Header("Ground Check (Updated to BoxCast)")]
    public Collider2D playerCollider;
    public float castDistance = 0.1f;
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
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
                anim.SetTrigger("Jump");
                recordJumpTrigger = true;

                // TAMBAHAN SFX: Putar suara lompat sekali tembak
                if (movementAudioSource != null && jumpSFX != null)
                {
                    movementAudioSource.PlayOneShot(jumpSFX);
                }
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
        isGrounded = CheckIfGrounded();

        rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);

        // TAMBAHAN SFX: Logika Suara Langkah Kaki Berkala saat Bergerak di Tanah
        if (moveInput != 0f && isGrounded)
        {
            walkTimer += Time.fixedDeltaTime;
            if (walkTimer >= walkStepDelay)
            {
                if (movementAudioSource != null && walkSFX != null)
                {
                    movementAudioSource.PlayOneShot(walkSFX);
                }
                walkTimer = 0f; // Reset timer langkah kaki
            }
        }
        else
        {
            // Ketika diam, timer disiapkan agar langkah pertama langsung berbunyi instan
            walkTimer = walkStepDelay;
        }

        currentRunData.Add(new ActiveData(
            transform.position,
            isFacingRight,
            moveInput != 0f,
            isGrounded,
            recordJumpTrigger
        ));

        recordJumpTrigger = false;
    }

    private bool CheckIfGrounded()
    {
        if (playerCollider == null) return false;

        RaycastHit2D hit = Physics2D.BoxCast(
            playerCollider.bounds.center,
            playerCollider.bounds.size,
            0f,
            Vector2.down,
            castDistance,
            groundLayer
        );

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