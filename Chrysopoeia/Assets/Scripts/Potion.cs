using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Potion : MonoBehaviour
{
    private GameObject player;
    public float throwRange = 2f;
    public float throwPower = 5f;
    private void Start()
    {
        player = GameObject.Find("Player");
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            if (Vector3.Distance(player.transform.position, transform.position) < throwRange)
            {
                Knockback kb = player.GetComponent<Knockback>();
                kb.knockbackAway(throwPower, transform.position);
            }

            Destroy(gameObject);
        }
    }

    public void Explode()
    {
        
    }
}
