using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Speed")]
    public float startingSpeed = 5.0f;
    public float speed;

    [Header("Jump")]
    public float jumpPower = 1f;
    public float jumpCooldown = 0.1f;
    private bool jumpOnCD;
    public float coyoteTime = 0.1f;
    public bool groundedJump;
    public float boxcastDist = 1f;
    private Rigidbody2D rb;
    private BoxCollider2D coll;
    public LayerMask mask;

    [Header("Throw")]
    public GameObject potion;
    private Rigidbody2D potrb;
    public float throwpower = 5f;
    void Start()
    {
        speed = startingSpeed;
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<BoxCollider2D>();
    }

    void Update()
    {
        float hori = Input.GetAxisRaw("Horizontal");
        float vert = Input.GetAxisRaw("Vertical");
        rb.velocity = new Vector2(hori * speed, rb.velocity.y);

        groundedJump = Physics2D.BoxCast(transform.position, new(coll.size.x / 2, boxcastDist), 0, Vector2.down, boxcastDist, mask);
        if (rb.velocity.y < 0)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y - 2f * Time.deltaTime);
        }
        if (!jumpOnCD && Input.GetKey(KeyCode.Space) && groundedJump)
        {
            rb.velocity = new Vector2(rb.velocity.x, 0);
            jumpOnCD = true;
            rb.AddForce(new(0, jumpPower), ForceMode2D.Impulse);
            Invoke(nameof(endJumpCooldown), jumpCooldown);
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            GameObject pot = Instantiate(potion, transform.position, Quaternion.identity);
            potrb = pot.GetComponent<Rigidbody2D>();
            potrb.AddForce(new Vector2(hori, vert) * throwpower, ForceMode2D.Impulse);
            potrb.velocity += new Vector2(rb.velocity.x, throwpower);
        }
    }

    private void endJumpCooldown()
    {
        jumpOnCD = false;
    }
}
