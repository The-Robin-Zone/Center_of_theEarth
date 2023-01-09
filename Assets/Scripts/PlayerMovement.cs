using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class PlayerMovement : MonoBehaviour
{
    // Physics
    public Rigidbody2D _rb;
    public CapsuleCollider2D _bc;
    private Animator _spriteAnimator;
    [SerializeField] private LayerMask groundLayerMask;

    public Transform groundCheck;
    public LayerMask groundLayer;

    private float moveSpeed = 4.25f;
    private float jumpingPower = 6.5f;

    private float hinput;
    private bool jinput;
    private bool jinputHold; 

    public float groundCheckDistance;
    public bool playerJump = false;
    private Vector2 playerJumpSlowForce = new Vector2(0, -4f);
    private float maxFallSpeed = 10;
    private int coyoteTimeMax = 7;                              // amount of frames late a player can be with their jump and still jump after leaving the ground
    private float coyoteTimer = 0;                              // tracks coyote time
    private int inputBufferJump = 5;                            // amount of frames early a player can be with their jump input
    private float inputBufferJumpTimer = 0;                     // tracks jump buffer

    public Vector3 boxSize;
    private bool lastGrounded;

    // Visuals
    private Transform _spriteAnchor;
    private float initXScale;
    private float initYScale;

    private float scaleLerpFactor = 0.09f;
    private float jumpScaleX = 0.6f;
    private float jumpScaleY = 1.35f;
    private float landScaleX = 1.2f;
    private float landScaleY = 0.7f;

    private void Awake()
    {
        //_rb = GetComponent<Rigidbody2D>();        
        //_bc = GetComponent<BoxCollider2D>();
        _spriteAnchor = transform.Find("SpriteAnchor");
        _spriteAnimator = _spriteAnchor.Find("Sprite").GetComponent<Animator>();
        initXScale = getXScale();
        initYScale = getYScale();
    }

    private void setScale(Vector3 newScale)
    {
        _spriteAnchor.localScale = newScale;
    }

    private float getXScale()
    {
        return _spriteAnchor.localScale.x;
    }

    private float getYScale()
    {
        return _spriteAnchor.localScale.y;
    }

    private float getZScale()
    {
        return _spriteAnchor.localScale.z;
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

        if (jinput) {
            inputBufferJumpTimer = inputBufferJump;
        }
        
        if (hinput != 0) {
            updateHeading();
        }

        // Jumping and Ground/Air checks
        if (IsGrounded()) {
            if (_rb.velocity.y <= 0) {
                coyoteTimer = coyoteTimeMax + Time.deltaTime;    // +1 time unit to account for the decrement at the end of the step
            }
        } else {
            if (_rb.velocity.y < 0) {
                playerJump = false;
            }

            if (playerJump && !jinputHold) {
                _rb.AddForce(playerJumpSlowForce);
            }
        }

        // note that we don't actually care if the player is grounded - only if the coyoteTimer is still active
        if (coyoteTimer > 0 && inputBufferJumpTimer > 0) {
            initiateJump();
        }

        if (IsGrounded() && !lastGrounded) {
            float _xs = landScaleX * initXScale * Mathf.Sign(getXScale());
            float _ys = landScaleY * Mathf.Sign(getYScale());
            setScale(new Vector3(_xs, _ys, getZScale()));
        }

        if (_rb.velocity.y <= -maxFallSpeed) {
            _rb.velocity = new Vector2(_rb.velocity.x, -maxFallSpeed);
        }

        lastGrounded = IsGrounded() && _rb.velocity.y <= 0;

        // Visuals
        _spriteAnimator.SetFloat("vspeed", _rb.velocity.y);
        _spriteAnimator.SetBool("grounded", IsGrounded());
        _spriteAnimator.SetBool("moving", hinput != 0);

        float _xscale = Mathf.Lerp(getXScale(), initXScale * Mathf.Sign(getXScale()), scaleLerpFactor);
        float _yscale = Mathf.Lerp(getYScale(), initYScale, scaleLerpFactor);
        setScale(new Vector3(_xscale, _yscale, getZScale()));


        // Time management
        coyoteTimer = decrementTimer(coyoteTimer, 60);
        inputBufferJumpTimer = decrementTimer(inputBufferJumpTimer, 60);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(groundCheck.position, 0.1f);
    }

    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.1f, groundLayer);
    }

    private float decrementTimer(float timer) {
        if (timer > 0) {
            timer -= Time.deltaTime;
        }
        return timer;
    }

    private float decrementTimer(float timer, float multiplier) {
        if (timer > 0)
        {
            timer -= Time.deltaTime * multiplier;
        }
        return timer;
    }

    private void initiateJump() {
        _rb.velocity = new Vector2(_rb.velocity.x, jumpingPower);
        playerJump = true;
        coyoteTimer = 0;

        inputBufferJumpTimer = 0;
        float _xscale = jumpScaleX * initXScale * Mathf.Sign(getXScale());
        float _yscale = jumpScaleY * getYScale();
        setScale(new Vector3(_xscale, _yscale, getZScale()));
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


    private void OnTriggerEnter2D(Collider2D other)
    {
        StateManager _manager = FindObjectOfType<StateManager>();
        if (other.CompareTag("RoomTransition"))
        {
            // get the gamemanager and make it restart the game
            _manager.finishLevel();

        } else if (other.CompareTag("DeathBox")) {
            _manager.setStateLoss();

        }
    }

}
