using UnityEngine;

public class playerMovement : MonoBehaviour
{
    private Rigidbody2D body;
    private Animator anim;
    public float speed = 5f;
    public float jumpForce = 10f;
    private bool isGrounded = false;
      
    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }
    private void Update()
    {
        float moveX = Input.GetAxis("Horizontal");
        body.linearVelocity = new Vector2(moveX * speed, body.linearVelocity.y);


        //flip player ketika gerak kiri-kanan
        if (moveX > 0.01f)
            transform.localScale = Vector3.one;
        else if (moveX < -0.01f)
            transform.localScale = new Vector3(-1, 1, 1);

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
            Jump();

        //set animator parameter
        anim.SetBool("Run", moveX != 0);
        anim.SetBool("Grounded", isGrounded);
    }

    //ngatur lompat
    private void Jump()
    {
        body.linearVelocity = new Vector2(body.linearVelocity.x, jumpForce);
        anim.SetTrigger("Jump");
        isGrounded = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Ground"))
            isGrounded = true;
    }
}
