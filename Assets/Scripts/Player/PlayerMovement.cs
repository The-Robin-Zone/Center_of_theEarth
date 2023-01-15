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
    private SpriteRenderer _spriteRenderer;
    [SerializeField] private LayerMask groundLayerMask;

    public Transform groundCheck;
    public LayerMask groundLayer;

    public float moveSpeed = 4.25f;
    public float jumpingPower = 6.5f;

    private float hinput;
    private bool jinput;
    private bool jinputHold;

    public float groundCheckDistance = 0.2f;
    public bool playerJump = false;
    private Vector2 playerJumpSlowForce = new Vector2(0, -4f);
    public float maxFallSpeed = 10;

    // Time
    private int coyoteTimeMax = 7;                              // amount of frames late a player can be with their jump and still jump after leaving the ground
    private float coyoteTimer = 0;                              
    private int inputBufferJump = 5;                            // amount of frames early a player can be with their jump input
    private float inputBufferJumpTimer = 0;                     
    private float invincibilityBaseTime = 60 * 2.5f;            // amount of frames of invinicibility the player receives on getting hit
    private float invincibilityTimer = 0;

    public bool died = false;
    public bool finishedLevel = false;

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

    // Sound
    public AudioSource jumpSound;
    public AudioSource takeDamageSound;
    public float soundvol = 0.05f;

    private void Awake()
    {
        //_rb = GetComponent<Rigidbody2D>();        
        //_bc = GetComponent<BoxCollider2D>();
        _spriteAnchor = transform.Find("SpriteAnchor");
        _spriteAnimator = _spriteAnchor.Find("Sprite").GetComponent<Animator>();
        _spriteRenderer = _spriteAnchor.Find("Sprite").GetComponent<SpriteRenderer>();
        _spriteRenderer.color = Color.white;
        initXScale = getXScale();
        initYScale = getYScale();

        StateManager _manager = GameObject.Find("GameManager").GetComponent<StateManager>();
        _manager.pauseButton.GetComponent<pausemenu>().scope = GameObject.Find("ScopeCenter"); //getComponent<pausemenu>().scope = GameObject.Find("ScopeCenter");

        Global_Variables.gameFrozen = false;
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

        if (IsGrounded() && _rb.velocity.y <= 0 && !lastGrounded) {
            float _xs = landScaleX * initXScale * Mathf.Sign(getXScale());
            float _ys = landScaleY * Mathf.Sign(getYScale());
            setScale(new Vector3(_xs, _ys, getZScale()));
        }

        if (_rb.velocity.y <= -maxFallSpeed) {
            _rb.velocity = new Vector2(_rb.velocity.x, -maxFallSpeed);
        }

        lastGrounded = IsGrounded();

        // Visuals
        _spriteAnimator.SetFloat("vspeed", _rb.velocity.y);
        _spriteAnimator.SetBool("grounded", IsGrounded());
        _spriteAnimator.SetBool("moving", hinput != 0);

        if (invincibilityTimer > 0 && ((int)(invincibilityTimer)) % 10 == 0)
        {
            _spriteRenderer.color = Color.red;
        } else
        {
            _spriteRenderer.color = Color.white;
        }


        float _xscale = Mathf.Lerp(getXScale(), initXScale * Mathf.Sign(getXScale()), scaleLerpFactor);
        float _yscale = Mathf.Lerp(getYScale(), initYScale, scaleLerpFactor);
        setScale(new Vector3(_xscale, _yscale, getZScale()));


        // Time management
        coyoteTimer = decrementTimer(coyoteTimer, 60);
        inputBufferJumpTimer = decrementTimer(inputBufferJumpTimer, 60);
        invincibilityTimer = decrementTimer(invincibilityTimer, 60);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(groundCheck.position, groundCheckDistance);
    }

    public bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, groundCheckDistance, groundLayer);
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

        jumpSound.Play();
        jumpSound.volume = soundvol;
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

    private void takeDamage(int damage)
    {
        takeDamageSound.Play();
        takeDamageSound.volume = soundvol;

        Global_Variables.life -= damage;
        Debug.Log(Global_Variables.life);
        if (Global_Variables.life <= 0 && !died)
        {
            died = true;
            StateManager _manager = FindObjectOfType<StateManager>();
            _manager.setStateLoss();
        }
    }

    private int getKeyInt(KeyCode key) {
        return Input.GetKey(key) ? 1 : 0; 
    }

    private void giveInvincibility()
    {
        invincibilityTimer = invincibilityBaseTime;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        StateManager _manager = FindObjectOfType<StateManager>();
        if (other.CompareTag("RoomTransition") && !finishedLevel)
        {
            finishedLevel = true;
            if (_manager.currentLevel != 1)
            {
                _manager.finishLevel();
            } else
            {
                // otherwise go straight into the next level
                _manager.startNextLevel();
            }

        } else if (other.CompareTag("DeathBox")) {
            _manager.setStateLoss();

        } else if (other.CompareTag("Enemy"))
        {
            if (invincibilityTimer <= 0)
            {
                // check if the player has invincibility; if not, take damage and give them invincibility
                takeDamage(1);
                giveInvincibility();
            }
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            if (invincibilityTimer <= 0)
            {
                // check if the player has invincibility; if not, take damage and give them invincibility
                takeDamage(1);
                giveInvincibility();
            }
        }
    }

}
