using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class playerMovement : MonoBehaviour
{
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
    public CoinManager cm;

    private Rigidbody2D rb;
    private Animator anim;
    private bool isGrounded;
    private bool recordJumpTrigger = false;

    [Header("Time Ghost Mechanic")]
    [SerializeField] private GameObject ghostPrefab;
    private List<ActiveData> currentRunData = new List<ActiveData>();
    private Vector3 startPosition;

    // ==========================================
    // SOLUSI 2 AUDIOSOURCE (ANTI-BENTROK)
    // ==========================================
    private AudioSource walkChannel;
    private AudioSource sfxChannel;

    [Header("Audio Clips")]
    public AudioClip walkSound;
    public AudioClip jumpSound;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        // Mengambil dua AudioSource yang ada di objek Player
        AudioSource[] sources = GetComponents<AudioSource>();
        if (sources.Length >= 2)
        {
            walkChannel = sources[0]; // Saluran pertama untuk jalan
            sfxChannel = sources[1];  // Saluran kedua untuk sfx instan
        }
        else
        {
            // Pengaman jika di Inspector kamu lupa menambahkannya
            walkChannel = gameObject.AddComponent<AudioSource>();
            sfxChannel = gameObject.AddComponent<AudioSource>();
        }

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

            // ==========================================
            // SFX MELOMPAT (JUMP) - Di Saluran SFX
            // ==========================================
            if (Keyboard.current.spaceKey.wasPressedThisFrame && isGrounded)
            {
                // Matikan suara jalan di saluran sebelah secara instan saat melompat
                if (walkChannel != null)
                {
                    walkChannel.Stop();
                }

                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
                anim.SetTrigger("Jump");
                recordJumpTrigger = true;

                // Mainkan suara lompat di saluran SFX khusus. Dijamin TIDAK AKAN tertimpa suara jalan lagi!
                if (sfxChannel != null && jumpSound != null)
                {
                    sfxChannel.PlayOneShot(jumpSound);
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

        // ==========================================
        // SFX BERJALAN (WALK) - Di Saluran Walk khusus
        // ==========================================
        if (walkChannel != null && walkSound != null)
        {
            if (moveInput != 0f && isGrounded)
            {
                if (walkChannel.clip != walkSound || !walkChannel.isPlaying)
                {
                    walkChannel.clip = walkSound;
                    walkChannel.loop = true;
                    walkChannel.Play();
                }
            }
            else
            {
                if (walkChannel.clip == walkSound && walkChannel.isPlaying)
                {
                    walkChannel.Stop();
                }
            }
        }
    }

    private void FixedUpdate()
    {
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

        if (walkChannel != null) walkChannel.Stop();
        if (sfxChannel != null) sfxChannel.Stop();

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