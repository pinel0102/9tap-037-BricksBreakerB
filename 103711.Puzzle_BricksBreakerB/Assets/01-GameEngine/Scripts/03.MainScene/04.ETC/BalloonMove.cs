using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalloonMove : MonoBehaviour
{
    public float speedMin = 4f;
    public float speedMax = 8f;

    private float speed = 5f;
    private Vector2 angle = Vector2.one;
    private Vector2 lastVelocity;
    private Rigidbody2D rb;
    private const float multiplier = 1000f;
    
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        SetVelocity();
    }

    private void Update()
    {
        lastVelocity = rb.velocity;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.CompareTag(GlobalDefine.TAG_WALL))
        {
            rb.velocity = Vector3.Reflect(lastVelocity.normalized, other.GetContact(0).normal) * Mathf.Max(0, lastVelocity.magnitude);
        }
    }

    private void SetVelocity()
    {
        speed = Random.Range(speedMin, speedMax);
        angle = new Vector2(Random.Range(0, 2) % 2 == 0 ? -1 : 1, Random.Range(0.2f, 0.6f));
        rb.velocity = new Vector2(Time.deltaTime * multiplier * speed * angle.x, Time.deltaTime * multiplier * speed * angle.y);
    }
}
