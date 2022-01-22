using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float runSpeed = 10f;
    [SerializeField] float jumpSpeed = 10f;
    [SerializeField] float climbSpeed = 5f;
    [SerializeField] GameObject bullet;
    [SerializeField] Transform gun;
    

    Vector2 moveInput;
    Rigidbody2D rb;
    Animator anim;
    CapsuleCollider2D bodyCollider;
    BoxCollider2D feetCollider;
    ParticleSystem deathParticles;
    float startGravityScale; 
    bool isAlive;
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        bodyCollider = GetComponent<CapsuleCollider2D>();
        feetCollider = GetComponent<BoxCollider2D>();
        deathParticles = GetComponent<ParticleSystem>();
        startGravityScale = rb.gravityScale;
        isAlive = true; 
    }

    void Update()
    {
        if(!isAlive){ return; }
        Run();
        FlipSprite();
        ClimbLadder();
        Die();
        
    }

    private void OnMove(InputValue value)
    {
        if(!isAlive){ return; }
        moveInput = value.Get<Vector2>();
    }

    private void OnJump(InputValue value)
    {
        if(!isAlive){ return; }
        if(!feetCollider.IsTouchingLayers(LayerMask.GetMask("Ground")))
        {
            return;
        }

        if(value.isPressed)
        {
            rb.velocity += new Vector2(0f, jumpSpeed);
        }
    }

    private void OnFire(InputValue value)
    {
        if(!isAlive){ return; }
        Instantiate(bullet, gun.position, transform.rotation);
    }

    private void Run()
    {
        if(!isAlive){ return; }
        Vector2 playerVelocity = new Vector2(moveInput.x * runSpeed, rb.velocity.y);
        rb.velocity = playerVelocity;
        
        bool playerHasHorizSpeed = Mathf.Abs(rb.velocity.x) > Mathf.Epsilon;
        anim.SetBool("isRunning", playerHasHorizSpeed);
    }

    private void FlipSprite()
    {
        bool playerHasHorizSpeed = Mathf.Abs(rb.velocity.x) > Mathf.Epsilon;
        if(playerHasHorizSpeed)
        {
            transform.localScale = new Vector2(Mathf.Sign(rb.velocity.x), 1f);
        }
        
    }
    private void ClimbLadder()
    {
        if(!bodyCollider.IsTouchingLayers(LayerMask.GetMask("Ladder")))
        {
            rb.gravityScale = startGravityScale;
            anim.SetBool("isClimbing", false);
            return;
        }
        Vector2 climbVelocity = new Vector2(rb.velocity.x, moveInput.y * climbSpeed);
        rb.velocity = climbVelocity;
        rb.gravityScale = 0f;
        bool playerHasVertSpeed = Mathf.Abs(rb.velocity.y) > Mathf.Epsilon;
        anim.SetBool("isClimbing", playerHasVertSpeed);
    }
    private void Die() 
    {
        if(bodyCollider.IsTouchingLayers(LayerMask.GetMask("Enemies", "Hazards")))
        {
            isAlive = false;
            anim.SetTrigger("Dying");
            Vector2 stopVelocity = new Vector2(0, 0);
            rb.velocity = stopVelocity;
            deathParticles.Play();
            Invoke("HelperDeath", 2f);
        }
    }
    private void HelperDeath()
    {
        FindObjectOfType<GameSession>().ProcessPlayerDeath();
    }
}
