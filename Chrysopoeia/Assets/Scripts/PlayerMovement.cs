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
    private Vector2 burst;
    private bool bursting;
    public float burstSpeed = 5f;
    public float burstDuration = 1f;
    private float burstStart;
    private bool burstjump;

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
        if (!bursting)
        {
            rb.velocity = new Vector2(hori * speed, rb.velocity.y);

            groundedJump = Physics2D.BoxCast(transform.position, new(coll.size.x / 2, boxcastDist), 0, Vector2.down, boxcastDist, mask);
            if (groundedJump)
            {
                burstjump = false;
            }
            if (rb.velocity.y < 0 && !burstjump)
            {
                rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y - 2f * Time.deltaTime);
            }
            if ( Input.GetKey(KeyCode.Space))
            {
                if (!jumpOnCD && groundedJump)
                {
                    rb.velocity = new Vector2(rb.velocity.x, 0);
                    jumpOnCD = true;
                    rb.AddForce(new(0, jumpPower), ForceMode2D.Impulse);
                    Invoke(nameof(endJumpCooldown), jumpCooldown);
                }
                else if (burstjump)
                {
                    rb.velocity = new Vector2(rb.velocity.x, 0);
                    jumpOnCD = true;
                    rb.AddForce(new(0, jumpPower * 0.7f), ForceMode2D.Impulse);
                    burstjump = false;
                    Invoke(nameof(endJumpCooldown), jumpCooldown);
                }
                
            }

            if (Input.GetKeyDown(KeyCode.L))
            {
                GameObject pot = Instantiate(potion, transform.position, Quaternion.identity);
                potrb = pot.GetComponent<Rigidbody2D>();
                if (vert < 0)
                {
                    potrb.AddForce(new Vector2(hori * 1.5f, vert) * throwpower, ForceMode2D.Impulse);




                }
                else
                {
                    if (vert > 0)
                    {
                        potrb.AddForce(new Vector2(hori, vert + 0.5f) * throwpower, ForceMode2D.Impulse);

                    }
                    else
                    {
                        potrb.AddForce(new Vector2(hori * 1.5f, vert + 0.5f) * throwpower, ForceMode2D.Impulse);

                    }

                }
                potrb.velocity += new Vector2(0, 0);
            }
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                bursting = true;
                burstStart = Time.time;
                burst = new Vector2(hori, vert).normalized;
                burstjump = true;
            }
        }
        else
        {
            rb.velocity = burst * burstSpeed;
            if (Time.time - burstStart > burstDuration)
            {
                bursting = false;
                rb.velocity = Vector2.zero;
            }
        }
        
    }

    private void endJumpCooldown()
    {
        jumpOnCD = false;
    }
}
