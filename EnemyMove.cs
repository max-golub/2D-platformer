using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    [SerializeField] float moveSpeed = 1f;
    Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        rb.velocity = new Vector2(moveSpeed, 0f);
    }
    private void OnTriggerExit2D(Collider2D other) 
    {
        moveSpeed = -moveSpeed;
        FlipSprite();
    }
    private void FlipSprite()
    {
        transform.localScale = new Vector2(-(Mathf.Sign(rb.velocity.x)), 1f);
    }
}
