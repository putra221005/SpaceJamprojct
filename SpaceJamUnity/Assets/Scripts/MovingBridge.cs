using UnityEngine;

public class MovingBridge : MonoBehaviour
{
    [Header("Bridge Settings")]
    [SerializeField] private float moveSpeed = 3f;

    // MENGGANTI UPPERDISTANCE: Sekarang Anda bisa mengatur arah dan jaraknya sendiri
    // Contoh: Isi Y = 3 untuk naik, isi Y = -3 jika ingin gerakan pertamanya ke BAWAH
    [SerializeField] private Vector2 moveDirection = new Vector2(0, 3f);

    [SerializeField] private float delayBeforeStart = 2f; // JEDA WAKTU sebelum jembatan bergerak

    private Vector3 startPosition; // Posisi awal jembatan saat game mulai
    private Vector3 targetPositionGoal; // Posisi tujuan setelah tombol ditekan
    private bool shouldMove = false;

    void Start()
    {
        // Catat posisi default jembatan saat ditaruh di panggung
        startPosition = transform.position;

        // Hitung posisi target berdasarkan arah yang Anda tentukan di Inspector
        targetPositionGoal = startPosition + new Vector3(moveDirection.x, moveDirection.y, 0);
    }

    void Update()
    {
        // Tentukan ke mana jembatan harus bergerak berdasarkan status tombol
        Vector3 currentTarget = shouldMove ? targetPositionGoal : startPosition;

        // Gerakkan jembatan secara mulus
        transform.position = Vector3.MoveTowards(transform.position, currentTarget, moveSpeed * Time.deltaTime);
    }

    // Fungsi ini dipanggil oleh tombol saat ditekan pertama kali
    public void TriggerBridgeActivation()
    {
        CancelInvoke("StartMovingToTarget");
        Invoke("StartMovingToTarget", delayBeforeStart);
    }

    // Fungsi ini dipanggil oleh tombol saat ditekan kedua kali
    public void TriggerBridgeDeactivation()
    {
        CancelInvoke("StartMovingToTarget");
        shouldMove = false; // Jembatan langsung kembali ke posisi awal
    }

    private void StartMovingToTarget()
    {
        shouldMove = true;
    }
}