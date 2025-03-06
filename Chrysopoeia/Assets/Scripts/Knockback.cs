using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knockback : MonoBehaviour
{
    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    public void knockbackAway(float force, Vector3 origin)
    {
        Vector2 dir =  (origin - transform.position).normalized;

        rb.AddForce(-dir * force, ForceMode2D.Impulse);
    }
}
