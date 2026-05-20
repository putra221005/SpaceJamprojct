using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelMenuManager : MonoBehaviour
{
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
            tombolLevel2.image.sprite = spriteLevel2_Terbuka; // Pakai gambar angka 2 menyala
            TampilkanBintang("Level1_Bintang", bintangLevel2);
        }
        else
        {
            tombolLevel2.interactable = false;
            tombolLevel2.image.sprite = spriteLevel2_Terkunci; // Pakai gambar angka 2 abu-abu
            MatikanSemuaBintang(bintangLevel2);
        }

        // ================= LEVEL 3 SYSTEM =================
        int level3Terbuka = PlayerPrefs.GetInt("Level3_Terbuka", 0);

        if (level3Terbuka == 1)
        {
            tombolLevel3.interactable = true;
            tombolLevel3.image.sprite = spriteLevel3_Terbuka; // Pakai gambar angka 3 menyala
            TampilkanBintang("Level2_Bintang", bintangLevel3);
        }
        else
        {
            tombolLevel3.interactable = false;
            tombolLevel3.image.sprite = spriteLevel3_Terkunci; // Pakai gambar angka 3 abu-abu
            MatikanSemuaBintang(bintangLevel3);
        }
    }

    void TampilkanBintang(string keyPrefs, Image[] bintangUI)
    {
        int jumlahBintang = PlayerPrefs.GetInt(keyPrefs, 0);
        for (int i = 0; i < bintangUI.Length; i++)
        {
            if (i < jumlahBintang)
            {
                bintangUI[i].gameObject.SetActive(true);
            }
            else
            {
                bintangUI[i].gameObject.SetActive(false);
            }
        }
    }

    void MatikanSemuaBintang(Image[] bintangUI)
    {
        foreach (Image bintang in bintangUI)
        {
            bintang.gameObject.SetActive(false);
        }
    }

    public void BackToMainMenu() { SceneManager.LoadScene("Menu"); }
    public void GoToProlog() { SceneManager.LoadScene("Prolog"); }
    public void GoToLevel2() { SceneManager.LoadScene("Level2"); }
    public void GoToLevel3() { SceneManager.LoadScene("Level3"); }
}