using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.ShaderKeywordFilter;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Speed")]
    public float startingSpeed = 5.0f;
    public float speed;
    private SpriteRenderer spriteRenderer;

    private bool bowling;
    private float rcStart;
    private bool holdingRight;
    public float rcChargeTime = 0.1f;
    public float maxrockdist = 1f;
    public float rockMult = 3f;
    [Header("Jump")]
    public float jumpPower = 1f;
    public float burstjumpMult = 1f;
    public float jumpCooldown = 0.1f;
    private bool jumpOnCD;
    public float coyoteTime = 0.1f;
    public bool groundedJump;
    public float boxcastDist = 1f;
    public float gravityBonus = 1f;
    public float rockgravBonus = 1.2f;
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
    public float maxthrowdist = 5f;
    
    void Start()
    {
        speed = startingSpeed;
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<BoxCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        groundedJump = Physics2D.BoxCast(transform.position, new(coll.size.x / 2, boxcastDist), 0, Vector2.down, boxcastDist, mask);
        if (!bowling)
        {
            float hori = Input.GetAxisRaw("Horizontal");
            float vert = Input.GetAxisRaw("Vertical");
            if (!bursting)
            {
                rb.AddRelativeForce(new Vector2(hori * speed * Time.deltaTime * 100, 0), ForceMode2D.Force);

                if (hori < 0 && rb.velocity.x > -speed || hori > 0 && rb.velocity.x < speed)
                {

                }


                if (groundedJump)
                {
                    burstjump = false;
                }
                if (rb.velocity.y < 0)
                {
                    rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y - (gravityBonus * Time.deltaTime));
                    Debug.Log("Gravity bonus applied.");
                }
                if (Input.GetKey(KeyCode.Space))
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

                if (Input.GetMouseButtonDown(0))
                {
                    Vector2 direction = transform.position;

                    if (Vector3.Distance(Camera.main.ScreenToWorldPoint(Input.mousePosition), transform.position) < maxthrowdist)
                    {
                        direction = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;

                    }
                    else
                    {
                        direction = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
                        direction.Normalize();
                        direction *= maxthrowdist;
                    }
                    GameObject pot = Instantiate(potion, transform.position, Quaternion.identity);
                    potrb = pot.GetComponent<Rigidbody2D>();
                    potrb.AddForce(direction * throwpower, ForceMode2D.Impulse);


                }
                if (Input.GetMouseButtonDown(1))
                {
                    rcStart = Time.time;
                    holdingRight = true;
                   
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
        }
        else
        {
           
            if (rb.velocity.y < 0)
            {
                if (groundedJump)
                {
                    if (rb.velocity.magnitude > 1)
                    {

                    }
                    bowling = false;
                    spriteRenderer.color = Color.white;
                    rb.drag = 3;
                    rb.gravityScale = 2;


                }
                rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y - (rockgravBonus * Time.deltaTime));

            }

            if (Input.GetMouseButtonUp(1))
            {
                bowling = false;
                spriteRenderer.color = Color.white;

                rb.drag = 3;
                rb.gravityScale = 2;



            }
        }
        if (holdingRight && Input.GetMouseButtonUp(1))
        {
            bursting = true;
            burstStart = Time.time;
            
            Vector2 dir = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;

            burst = dir.normalized;
            burstjump = true;
        }
        
        if (holdingRight && Input.GetMouseButton(1))
        {
            if (Time.time - rcStart > rcChargeTime)
            {
                holdingRight = false;
                bowling = true;
                spriteRenderer.color = Color.cyan;
                rb.drag = 1;
                rb.gravityScale = 4;
                Vector2 direction = transform.position;

                if (Vector3.Distance(Camera.main.ScreenToWorldPoint(Input.mousePosition), transform.position) < maxrockdist)
                {
                    direction = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;

                }
                else
                {
                    direction = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
                    direction.Normalize();
                    direction += new Vector2(0, 0.5f);
                    direction *= maxrockdist;
                    
                }
                rb.AddForce(direction * rockMult, ForceMode2D.Impulse);
            }
        }
        
        
    }

    private void endJumpCooldown()
    {
        jumpOnCD = false;
    }
}
