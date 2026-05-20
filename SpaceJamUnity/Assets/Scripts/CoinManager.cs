using UnityEngine;
using TMPro;

public class CoinManager : MonoBehaviour
{
    public int coinCount;
    public TextMeshProUGUI coinText;

    [Header("Pengaturan Keycaps Prompt")]
    [Tooltip("Tarik GameObject Keycaps Prompt dari bawah Player ke sini")]
    public GameObject keycapsPrompt;

    private bool promptHasSpawned = false; // Pengaman agar perintah aktifkan objek tidak dipanggil terus-menerus di Update

    void Start()
    {
        // Menghitung otomatis jumlah semua koin yang ada di map saat game mulai
        coinCount = FindObjectsByType<Coin>(FindObjectsSortMode.None).Length;

        // Pastikan di awal game petunjuk keycaps dalam keadaan mati/sembunyi
        if (keycapsPrompt != null)
        {
            keycapsPrompt.SetActive(false);
        }
    }

    void Update()
    {
        if (coinText != null)
        {
            coinText.text = ": " + coinCount.ToString();
        }

        // BARU: Cek setiap frame, jika koin sudah habis DAN prompt belum pernah dimunculkan
        if (AreAllCoinsCollected() && !promptHasSpawned)
        {
            SpawnKeycapsPrompt();
        }
    }

    public bool AreAllCoinsCollected()
    {
        return coinCount <= 0;
    }

    void SpawnKeycapsPrompt()
    {
        promptHasSpawned = true; // Kunci agar fungsi ini hanya berjalan 1 kali saja

        if (keycapsPrompt != null)
        {
            keycapsPrompt.SetActive(true); // Munculkan gambar tombol keycaps di layar/di dekat player
            Debug.Log("Semua koin habis! Menampilkan petunjuk tombol.");
        }
    }

    // Fungsi tambahan: Panggil fungsi ini dari script koin kamu setiap kali player mengambil koin
    // Contoh di script koin: FindFirstObjectByType<CoinManager>().ReduceCoin();
    public void ReduceCoin()
    {
        coinCount--;
    }
}