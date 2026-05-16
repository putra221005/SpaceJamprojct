using UnityEngine;
using System.Collections.Generic;

public class TimeGhost : MonoBehaviour
{
    private List<ActiveData> ghostData;
    private int currentFrame = 0;
    private SpriteRenderer sr;
    private Rigidbody2D rb;
    private Animator anim; // Tambahan untuk memutar animasi bayangan

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>(); // Ambil komponen Animator pada Clone

        if (sr != null)
        {
            Color color = sr.color;
            color.a = 0.5f; // Transparansi 50%
            sr.color = color;
        }
    }

    public void SetData(List<ActiveData> dataToReplay)
    {
        ghostData = dataToReplay;
        currentFrame = 0;
    }

    void FixedUpdate()
    {
        if (ghostData != null && currentFrame < ghostData.Count)
        {
            ActiveData frameData = ghostData[currentFrame];

            // Pindahkan posisi secara mulus (Kinematic)
            rb.MovePosition(frameData.position);

            // Terapkan arah hadap (Flip) sesuai masa lalu
            transform.localScale = frameData.scale;

            // PUTAR ANIMASI: Meniru persis apa yang dilakukan player utama di masa lalu
            if (anim != null)
            {
                anim.SetBool("Run", frameData.animRun);
                anim.SetBool("Grounded", frameData.animGrounded);

                if (frameData.animJumpTrigger)
                {
                    anim.SetTrigger("Jump");
                }
            }

            currentFrame++;
        }
    }
}