using UnityEngine;

[System.Serializable]
public struct ActiveData
{
    public Vector3 position;
    public bool isFacingRight;
    public bool animRun;         // Merekam status animasi lari
    public bool animGrounded;    // Merekam status animasi menyentuh tanah
    public bool animJumpTrigger; // Merekam pemicu animasi lompat

    public ActiveData(Vector3 pos, bool facing, bool run, bool grounded, bool jumpTrigger)
    {
        this.position = pos;
        this.isFacingRight = facing;
        this.animRun = run;
        this.animGrounded = grounded;
        this.animJumpTrigger = jumpTrigger;
    }
}