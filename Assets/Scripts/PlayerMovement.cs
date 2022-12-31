using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class PlayerMovement : MonoBehaviour
{
    // Physics
    public Rigidbody2D _rb;
    private BoxCollider2D _bc;
    private Animator _spriteAnimator;
    [SerializeField] private LayerMask groundLayerMask;

    public Transform groundCheck;
    public LayerMask groundLayer;

    private float moveSpeed = 8f;
    private float jumpingPower = 8f;
    private int heading = 1;

    private float hinput;
    private bool jinput;
    private bool jinputHold; 
    public float groundCheckDistance;
    public Vector3 boxSize;

    // Visuals
    private SpriteRenderer _spriteRenderer;
    private Transform _spriteAnchor;
    private float scaleLerpFactor = 0.09f;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();        
        _bc = GetComponent<BoxCollider2D>();
        _spriteAnchor = transform.Find("SpriteAnchor");
        _spriteAnimator = _spriteAnchor.Find("Sprite").GetComponent<Animator>();
    }

    private void setScale(Vector3 newScale)
    {
        _spriteAnchor.localScale = newScale;
    }


    private void updateHeading()
    {
        Vector3 scale = _spriteAnchor.localScale;
        scale.x = Math.Abs(scale.x) * Math.Sign(hinput);
        setScale(scale);
    }

    void Update()
    {
        hinput =        getKeyInt(KeyCode.D) - getKeyInt(KeyCode.A);
        jinput =        Input.GetKeyDown(KeyCode.Space);
        jinputHold =    Input.GetKey(KeyCode.Space);


        
        _rb.velocity = new Vector2(hinput * moveSpeed, _rb.velocity.y);

    
        
        if (hinput != 0) {
            updateHeading();
        }

        // Jumping
        if (jinput && IsGrounded()) {
            _rb.velocity = new Vector2(_rb.velocity.x, jumpingPower);
        }


        _spriteAnimator.SetFloat("vspeed", _rb.velocity.y);
        _spriteAnimator.SetBool("grounded", IsGrounded());
        _spriteAnimator.SetBool("moving", hinput != 0);

    }

    // public void Jump(InputAction.CallbackContext context)
    // {
    //     if (context.performed && IsGrounded())
    //     {
            
    //     }

    //     if (context.canceled && _rb.velocity.y > 0f)
    //     {
    //         _rb.velocity = new Vector2(_rb.velocity.x, _rb.velocity.y * 0.5f);
    //     }
    // }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(groundCheck.position, 0.1f);
    }

    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.1f, groundLayer);
    }

    // private bool IsGrounded()
    // {
    //     return Physics2D.OverlapCircle(groundCheck.position, 0.1f, groundLayer);
    // }

    // private void Flip()
    // {
    //     isFacingRight = !isFacingRight;
    //     Vector3 localScale = transform.localScale;
    //     localScale.x *= -1f;
    //     transform.localScale = localScale;
    // }

    //public void Move(InputAction.CallbackContext context)
    //{
    //    horizontal = context.ReadValue<Vector2>().x;
    //}

    private int getKeyInt(KeyCode key) {
        return Input.GetKey(key) ? 1 : 0; 
    }


}
