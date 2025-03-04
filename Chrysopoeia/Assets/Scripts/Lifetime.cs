using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lifetime : MonoBehaviour
{
    public float lifetime = 5f;
    private float start;
    void Start()
    {
        start = Time.time;
    }

    void Update()
    {
        if (Time.time - start > lifetime)
        {
            Destroy(gameObject);
        }
    }
}
