using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Potion : MonoBehaviour
{
    private GameObject player;
    public float throwRange = 2f;
    public float throwPower = 5f;
    public float damage = 5;
    public GameObject explosionEffect;
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
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
            foreach (GameObject enemy in enemies)
            {
                if (Vector3.Distance(transform.position, enemy.transform.position) < throwRange)
                {
                    Health health = enemy.GetComponent<Health>();
                    health.takeDamage(damage);
                }
            }
            Instantiate(explosionEffect, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }

    public void Explode()
    {
        
    }
}
