using UnityEngine;

[System.Serializable]
public struct ActiveData
{
    public Vector3 position;
    public Vector3 scale;    // Untuk merekam arah hadap (Flip) via localScale
    public bool animRun;     // Merekam status animasi lari
    public bool animGrounded;// Merekam status animasi menyentuh tanah
    public bool animJumpTrigger; // Merekam pemicu animasi lompat

    public ActiveData(Vector3 pos, Vector3 scale, bool run, bool grounded, bool jumpTrigger)
    {
        this.position = pos;
        this.scale = scale;
        this.animRun = run;
        this.animGrounded = grounded;
        this.animJumpTrigger = jumpTrigger;
    }
}