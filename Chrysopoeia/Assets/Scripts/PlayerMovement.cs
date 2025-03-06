using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Speed")]
    public float startingSpeed = 5.0f;
    public float speed;

    [Header("Jump")]
    public float jumpPower = 1f;
    public float burstjumpMult = 1f;
    public float jumpCooldown = 0.1f;
    private bool jumpOnCD;
    public float coyoteTime = 0.1f;
    public bool groundedJump;
    public float boxcastDist = 1f;
    public float gravityBonus = 1f;
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
    public float throwDelay = 0.2f;
    private bool throwing;
    private float throwStart;
    float throwhori = 0;
    float throwvert = 0;
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
        if (!bursting && !throwing)
        {
            rb.AddRelativeForce(new Vector2(hori * speed, 0), ForceMode2D.Force);

            if (hori < 0 && rb.velocity.x > -speed || hori > 0 && rb.velocity.x < speed)
            {

            }
            

            groundedJump = Physics2D.BoxCast(transform.position, new(coll.size.x / 2, boxcastDist), 0, Vector2.down, boxcastDist, mask);
            if (groundedJump)
            {
                burstjump = false;
            }
            if (rb.velocity.y < 0 )
            {
                rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y - gravityBonus * Time.deltaTime);
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
                    rb.AddForce(new(0, jumpPower * burstjumpMult), ForceMode2D.Impulse);
                    burstjump = false;
                    Invoke(nameof(endJumpCooldown), jumpCooldown);
                }
                
            }

            if (Input.GetKeyDown(KeyCode.L))
            {
                throwing = true;
                throwStart = Time.time;
                throwhori = hori;
                throwvert = vert;
                Time.timeScale = 0.7f;
            }
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                bursting = true;
                burstStart = Time.time;
                burst = new Vector2(hori, vert).normalized;
                burstjump = true;
            }
        }
        else if (bursting)
        {
            rb.velocity = burst * burstSpeed;
            if (Time.time - burstStart > burstDuration)
            {
                bursting = false;
                rb.velocity = Vector2.zero;
            }
        }
        else if (throwing)
        {
            
            if (hori != 0)
            {
                throwhori = hori;

            }
            if (vert != 0)
            {
               throwvert = vert;
            }
            if (Time.time - throwStart > throwDelay)
            {
                Time.timeScale = 1f;
                throwing = false;
                GameObject pot = Instantiate(potion, transform.position, Quaternion.identity);
                potrb = pot.GetComponent<Rigidbody2D>();
                if (throwvert < 0)
                {
                    potrb.AddForce(new Vector2(throwhori, throwvert) * throwpower, ForceMode2D.Impulse);




                }
                else
                {
                    if (throwvert > 0)
                    {
                        potrb.AddForce(new Vector2(throwhori, throwvert + 0.5f) * throwpower, ForceMode2D.Impulse);

                    }
                    else
                    {
                        potrb.AddForce(new Vector2(throwhori * 1.5f, throwvert + 0.5f) * throwpower, ForceMode2D.Impulse);

                    }

                }
                potrb.velocity += new Vector2(0, 0);
            }
        }
        
    }

    private void endJumpCooldown()
    {
        jumpOnCD = false;
    }
}
