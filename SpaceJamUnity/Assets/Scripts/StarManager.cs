using UnityEngine;
using UnityEngine.UI; // Wajib untuk mengakses komponen UI Image

public class StarManager : MonoBehaviour
{
    [Header("UI Bintang")]
    public Image[] starImages; // Array untuk menyimpan 3 UI Image bintang
    public Sprite fullStar;    // Sprite gambar bintang penuh
    public Sprite emptyStar;   // Sprite gambar bintang kosong/hilang

    [Header("Pengaturan Batas Clone")]
    public int batasDuaBintang = 3;  // Jika pakai 3 clone, sisa 2 bintang
    public int batasSatuBintang = 4; // Jika pakai 4 clone, sisa 1 bintang
    public int batasNolBintang = 5;  // Jika pakai 5 clone, sisa 0 bintang

    private int cloneUsed = 0;
    private int currentStars = 3;

    void Start()
    {
        // Pastikan saat mulai, UI bintang penuh
        UpdateStarUI();
    }

    // Fungsi ini akan dipanggil oleh PlayerMovement setiap kali spawn clone
    public void RecordCloneUsage()
    {
        cloneUsed++;
        CalculateStars();
        UpdateStarUI();
    }

    private void CalculateStars()
    {
        if (cloneUsed >= batasNolBintang)
        {
            currentStars = 0;
        }
        else if (cloneUsed >= batasSatuBintang)
        {
            currentStars = 1;
        }
        else if (cloneUsed >= batasDuaBintang)
        {
            currentStars = 2;
        }
        else
        {
            currentStars = 3; // Jika pakai 0, 1, atau 2 clone
        }
    }

    private void UpdateStarUI()
    {
        // Loop untuk memperbarui gambar pada ketiga bintang
        for (int i = 0; i < starImages.Length; i++)
        {
            if (i < currentStars)
            {
                starImages[i].sprite = fullStar;   // Nyala
            }
            else
            {
                starImages[i].sprite = emptyStar;  // Mati/Kosong
            }
        }
    }
}