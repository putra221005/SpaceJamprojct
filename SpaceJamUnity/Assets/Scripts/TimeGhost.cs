using UnityEngine;
using System.Collections.Generic;

public class TimeGhost : MonoBehaviour
{
    private List<ActiveData> ghostData;
    private int currentFrame = 0;
    private SpriteRenderer sr;
    private Rigidbody2D rb;
    private Animator anim; // Komponen animator si bayangan

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>(); // Ambil komponen Animator pada Clone

        if (sr != null)
        {
            Color color = sr.color;
            color.a = 0.5f; // Transparansi bayangan 50%
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

            // Pindahkan posisi secara mulus (Kinematic Rigidbody)
            rb.MovePosition(frameData.position);

            // Atur arah hadap (Flip) mengikuti rekaman isFacingRight milik Player
            Vector3 localScale = transform.localScale;
            if (frameData.isFacingRight)
            {
                localScale.x = Mathf.Abs(localScale.x);
            }
            else
            {
                localScale.x = -Mathf.Abs(localScale.x);
            }
            transform.localScale = localScale;

            // PUTAR ANIMASI CLONE: Meniru parameter animasi player utama
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