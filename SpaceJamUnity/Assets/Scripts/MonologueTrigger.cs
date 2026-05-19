using UnityEngine;
using TMPro;
using System.Collections;

public class MonologueTrigger : MonoBehaviour
{
    [Header("Monologue Settings")]
    [TextArea(3, 5)]
    [SerializeField] private string monologueContent;
    [SerializeField] private float typingSpeed = 0.04f;
    [SerializeField] private float displayDuration = 2.5f;
    [SerializeField] private bool triggerOnlyOnce = true;

    [Header("UI Reference")]
    [SerializeField] private TextMeshProUGUI textUI;

    private bool hasTriggered = false;
    private GameObject textObject; // Untuk menyimpan referensi game object teks

    void Start()
    {
        if (textUI != null)
        {
            // Ambil Game Object dari teks UI tersebut
            textObject = textUI.gameObject;

            // Pastikan teks dalam keadaan MATI saat game baru mulai
            textObject.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (triggerOnlyOnce && hasTriggered) return;

            hasTriggered = true;

            StopAllCoroutines();
            StartCoroutine(TypeText());
        }
    }

    private IEnumerator TypeText()
    {
        // 1. Aktifkan Game Object teks terlebih dahulu
        textObject.SetActive(true);
        textUI.text = "";

        // 2. Jalankan efek ketik
        foreach (char letter in monologueContent.ToCharArray())
        {
            textUI.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }

        // 3. Jeda membaca setelah teks utuh
        yield return new WaitForSeconds(displayDuration);

        // 4. Matikan kembali Game Object teks agar menghilang dari layar
        textObject.SetActive(false);

        if (triggerOnlyOnce)
        {
            Destroy(gameObject);
        }
    }
}