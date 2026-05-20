using UnityEngine;
using TMPro;
using System.Collections; // Wajib ditambahkan untuk menggunakan Coroutine (IEnumerator)

public class DialogueManager : MonoBehaviour
{
    public TMP_Text dialogueText;    // Tarik objek TextMeshPro ke sini

    [Header("Daftar Kalimat Monolog")]
    [TextArea(3, 5)]
    public string[] monologueLines; // Tulis teks monologmu di Inspector

    [Header("Kecepatan Efek Mengetik")]
    [Tooltip("Semakin kecil angkanya, semakin cepat ketikannya.")]
    public float typingSpeed = 0.05f;

    private int currentIndex = 0;
    private Coroutine typingCoroutine; // Untuk menyimpan referensi coroutine yang sedang berjalan

    // Fungsi ini yang akan dipanggil oleh Timeline / Signal Emitter
    public void ShowNextLine()
    {
        if (currentIndex < monologueLines.Length)
        {
            // Jika ada teks yang masih mengetik dari baris sebelumnya, hentikan paksa dulu
            if (typingCoroutine != null)
            {
                StopCoroutine(typingCoroutine);
            }

            // Jalankan efek mengetik untuk baris kalimat saat ini
            typingCoroutine = StartCoroutine(TypeText(monologueLines[currentIndex]));
            currentIndex++;
        }
        else
        {
            EndMonologue(); // Jika teks habis, otomatis tutup
        }
    }

    // Coroutine yang mengatur pemunculan huruf satu per satu
    IEnumerator TypeText(string line)
    {
        dialogueText.text = ""; // Kosongkan teks di layar terlebih dahulu

        // Ambil setiap huruf dari kalimat, lalu munculkan berurutan
        foreach (char letter in line.ToCharArray())
        {
            dialogueText.text += letter; // Tambahkan satu huruf ke layar
            yield return new WaitForSeconds(typingSpeed); // Tunggu selama beberapa detik sebelum huruf berikutnya
        }

        typingCoroutine = null; // Reset coroutine jika pengetikan kalimat sudah selesai penuh
    }

    public void EndMonologue()
    {
        // Pastikan ketikan berhenti total saat cutscene ditutup/selesai
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
        }

        dialogueText.text = ""; // Kosongkan teks di layar
        currentIndex = 0; // Reset urutan untuk cutscene berikutnya
    }
}