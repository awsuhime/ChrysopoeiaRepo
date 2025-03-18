using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public GameObject projectile;
    public float reloadTime = 2f;
    private bool reloading;
    public float detectionRange = 10f;
    private GameObject player;
    void Start()
    {
        player = GameObject.Find("Player");
    }

    void Update()
    {
        if (!reloading && Vector2.Distance(transform.position, player.transform.position) < detectionRange)
        {
            reloading = true;
            Invoke(nameof(Reload), reloadTime);
            Vector2 rotation = player.transform.position - transform.position;
            float rotz = Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg;
            Instantiate(projectile, transform.position, Quaternion.Euler(0, 0, rotz));
        }
    }

    void Reload()
    {
        reloading = false;
    }
}
