using UnityEngine;

public class KeycapsInputWatcher : MonoBehaviour
{
    [Header("Input Settings")]
    [Tooltip("Tentukan tombol apa yang harus ditekan pemain (Misal: E, Space, Return)")]
    public KeyCode targetKey = KeyCode.E;

    void Update()
    {
        // Setiap frame, cek apakah pemain menekan tombol yang ditentukan
        if (Input.GetKeyDown(targetKey))
        {
            // Panggil fungsi untuk menghilangkan petunjuk ini
            OnKeyPressedSuccess();
        }
    }

    void OnKeyPressedSuccess()
    {
        // Tambahkan efek suara atau logika penambahan poin di sini jika perlu
        Debug.Log("Pemain berhasil menekan tombol: " + targetKey.ToString());

        // Langsung nonaktifkan GameObject ini dari layar
        gameObject.SetActive(false);
    }
}