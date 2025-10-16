using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishMoveStraight : MonoBehaviour
{
    public Vector2 dir;
    private Rigidbody2D rb;
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        rb.velocity = dir;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    }
}
