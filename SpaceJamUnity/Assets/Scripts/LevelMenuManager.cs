using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelMenuManager : MonoBehaviour
{
    [Header("Pengaturan Sprite Bintang Menu")]
    public Sprite fullStar;    // Masukkan gambar BINTANG MENYALA di Inspector
    public Sprite emptyStar;   // Masukkan gambar BINTANG MATI di Inspector

    [Header("UI & Sprites Level 2 (Mars)")]
    public Button tombolLevel2;
    public Sprite spriteLevel2_Terbuka;   // Masukkan "Button 2_on"
    public Sprite spriteLevel2_Terkunci;  // Masukkan "Button 2_off"
    public Image[] bintangLevel2;

    [Header("UI & Sprites Level 3 (Dino)")]
    public Button tombolLevel3;
    public Sprite spriteLevel3_Terbuka;   // Masukkan "Button 3_on"
    public Sprite spriteLevel3_Terkunci;  // Masukkan "Button 3_off"
    public Image[] bintangLevel3;

    void Start()
    {
        UpdateLevelSelectionUI();
    }

    void UpdateLevelSelectionUI()
    {
        // ================= LEVEL 2 SYSTEM =================
        int level2Terbuka = PlayerPrefs.GetInt("Level2_Terbuka", 0);

        if (level2Terbuka == 1)
        {
            tombolLevel2.interactable = true;
            tombolLevel2.image.sprite = spriteLevel2_Terbuka;
        }
        else
        {
            tombolLevel2.interactable = false;
            tombolLevel2.image.sprite = spriteLevel2_Terkunci;
        }
        // Selalu tampilkan bintang (meskipun 0, agar terlihat kosong)
        TampilkanBintang("Level2_Bintang", bintangLevel2);


        // ================= LEVEL 3 SYSTEM =================
        int level3Terbuka = PlayerPrefs.GetInt("Level3_Terbuka", 0);

        if (level3Terbuka == 1)
        {
            tombolLevel3.interactable = true;
            tombolLevel3.image.sprite = spriteLevel3_Terbuka;
        }
        else
        {
            tombolLevel3.interactable = false;
            tombolLevel3.image.sprite = spriteLevel3_Terkunci;
        }
        // Selalu tampilkan bintang (meskipun 0, agar terlihat kosong)
        TampilkanBintang("Level3_Bintang", bintangLevel3);
    }

    void TampilkanBintang(string keyPrefs, Image[] bintangUI)
    {
        // Ambil data bintang yang disave, default 0
        int jumlahBintang = PlayerPrefs.GetInt(keyPrefs, 0);

        for (int i = 0; i < bintangUI.Length; i++)
        {
            // Pastikan objeknya aktif
            bintangUI[i].gameObject.SetActive(true);

            if (i < jumlahBintang)
            {
                bintangUI[i].sprite = fullStar;   // Bintang menyala
            }
            else
            {
                bintangUI[i].sprite = emptyStar;  // Bintang mati/kosong
            }
        }
    }

    public void BackToMainMenu() { SceneManager.LoadScene("Menu"); }
    public void GoToProlog() { SceneManager.LoadScene("Prolog"); }
    public void GoToLevel2() { SceneManager.LoadScene("Level2"); }
    public void GoToLevel3() { SceneManager.LoadScene("Level3"); }
}