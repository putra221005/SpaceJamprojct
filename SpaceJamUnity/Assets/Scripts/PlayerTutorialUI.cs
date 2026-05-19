using UnityEngine;

public class PlayerTutorialUI : MonoBehaviour
{
    [Header("UI Settings")]
    [SerializeField] private GameObject tutorialVisual;
    [SerializeField] private float delayBeforeShow = 1.5f;

    [Header("Physics Component")]
    [SerializeField] private Rigidbody2D rb; // Tarik Rigidbody2D Player ke sini

    private float idleTimer;
    private bool isMoving;

    void Start()
    {
        // Jika lupa narik Rigidbody2D di Inspector, script akan otomatis mencarinya
        if (rb == null)
        {
            rb = GetComponent<Rigidbody2D>();
        }
    }

    void Update()
    {
        // 1. Cek input jalan (Horizontal & Vertical)
        float moveInputX = Input.GetAxisRaw("Horizontal");
        float moveInputY = Input.GetAxisRaw("Vertical");

        // 2. Cek apakah player bergerak lewat input WASD
        bool hasInput = (moveInputX != 0 || moveInputY != 0);

        // 3. Cek apakah player sedang melompat/jatuh (kecepatan Y tidak sama dengan 0)
        bool isJumpingOrFalling = Mathf.Abs(rb.linearVelocity.y) > 0.05f;

        // Player dianggap "bergerak/aktif" jika punya input ATAU sedang melompat/jatuh
        isMoving = hasInput || isJumpingOrFalling;

        if (isMoving)
        {
            // Jika bergerak atau melompat, langsung sembunyikan UI dan reset timer
            tutorialVisual.SetActive(false);
            idleTimer = 0f;
        }
        else
        {
            // Jika benar-benar diam di tanah, hitung waktu delay
            idleTimer += Time.deltaTime;

            if (idleTimer >= delayBeforeShow)
            {
                tutorialVisual.SetActive(true);
            }
        }

        // ==================== KODE BARU UNTUK FIX UI TERBALIK ====================
        if (tutorialVisual.activeInHierarchy)
        {
            // Cek arah skala X dari Player saat ini (1 atau -1)
            float playerScaleX = transform.localScale.x;

            // Ambil skala lokal UI saat ini
            Vector3 uiLocalScale = tutorialVisual.transform.localScale;

            // Jika Player bernilai minus (hadap kiri), paksa UI lokal ikut minus agar hasilnya saling menetralkan (+ / tidak terbalik)
            float correctSign = playerScaleX < 0 ? -1f : 1f;

            tutorialVisual.transform.localScale = new Vector3(
                correctSign * Mathf.Abs(uiLocalScale.x),
                uiLocalScale.y,
                uiLocalScale.z
            );
        }
        // =========================================================================
    }
}