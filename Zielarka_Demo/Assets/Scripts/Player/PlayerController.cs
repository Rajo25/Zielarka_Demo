using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{
    public Rigidbody2D rb;
    private bool _facingRight = true;
    private RigidbodyConstraints2D _defaultConstraints;
    [Header("Movement")]
    public float moveSpeed = 10f;
    public float acceleration = 10f;
    public float deceleration = 10f;
    public float velPower = 10f;
    private float _moveInput;
    public float frictionAmount = 0.2f;
   
    [Header("Ground Check")]
    private bool _isGrounded;
    public LayerMask whatIsGround;
    public Vector2 groundCheckOffset;
    public Vector2 groundCheckRadius;
    
    [Header("Wall Check")]
    private bool _isRightWall;
    private bool _isLeftWall;
    public LayerMask whatIsWall;
    public Vector2 rightWallCheckOffset;
    public Vector2 rightWallCheckRadius;
    public Vector2 leftWallCheckOffset;
    public Vector2 leftWallCheckRadius;
    
    [Header("Jump")]
    private float _jumpInput;
    public float jumpForce = 10f;
    public float coyoteTimeWindow = 10f;
    private float _coyoteTime;
    public float jumpBufferWindow = 10f;
    private float _jumpBuffer;
    private bool _isJumping;
    public float jumpCutMultiplier = 10f;
    public float fallMultiplier = 10f;
    public float maxFallSpeed = -20f;
    public float fallAcceleration = 5f;
    private float _currentFallMultiplier = 1f;
    [Header("Apex Hang")]
    public float apexThreshold = 0.5f;
    public float apexHangMultiplier = 0.4f;

    void Start()
    {
        _defaultConstraints = rb.constraints;
    }
    private void FixedUpdate()
    {
        Movement();
        if (_jumpBuffer > 0 && _coyoteTime > 0 && !_isJumping)
        {
            Jump();
        }
        if (Mathf.Abs(rb.linearVelocity.y) < apexThreshold)
        {
            rb.AddForce(
                Physics2D.gravity * ((apexHangMultiplier - 1) * rb.mass),
                ForceMode2D.Force
            );
        }
        else if (rb.linearVelocity.y < 0)
        {
            _currentFallMultiplier = Mathf.Lerp(
                _currentFallMultiplier,
                fallMultiplier,
                fallAcceleration * Time.fixedDeltaTime
            );

            rb.AddForce(
                Physics2D.gravity * ((_currentFallMultiplier - 1) * rb.mass),
                ForceMode2D.Force
            );
        }
        else
        {
            _currentFallMultiplier = 1f;
        }
        
        if (rb.linearVelocity.y < maxFallSpeed)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, maxFallSpeed);
        }

        if (_moveInput > 0 && !_facingRight)
        {
            Flip();
        }

        if (_moveInput < 0 && _facingRight)
        {
            Flip();
        }

        WallCling();
    }

    private void Movement()
    {
        float targetSpeed = _moveInput * moveSpeed * 100 * Time.deltaTime;
        float speedDiff = targetSpeed - rb.linearVelocity.x;
        float accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? acceleration : deceleration;
        float movement = Mathf.Pow(Mathf.Abs(speedDiff) * accelRate, velPower) * Mathf.Sign(speedDiff);

        rb.AddForce(Vector2.right * (movement * Time.deltaTime));
        
        //adds artificial friction
        if (_isGrounded && Mathf.Abs(_moveInput) < 0.01f)
        {
            float amount = Mathf.Min(Mathf.Abs(rb.linearVelocity.x), Mathf.Abs(frictionAmount));
            amount *=Mathf.Sign(rb.linearVelocity.x);
            rb.AddForce(Vector2.right * -amount,  ForceMode2D.Impulse);
        }
    }

    private void Jump()
    {
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        _coyoteTime = 0;
        _jumpBuffer = 0;
        _isJumping = true;
    }

    public void OnJump()
    {
        _jumpBuffer = jumpBufferWindow;
    }

    private void OnJumpUp()
    {
        if (rb.linearVelocity.y > 0 && _isJumping)
        {
            rb.AddForce(Vector2.down * (rb.linearVelocity.y * jumpCutMultiplier), ForceMode2D.Impulse);
        }
        
        _coyoteTime = 0;
    }
    void Update()
    {
        if (Input.GetButtonDown("Jump"))
        {
            _jumpBuffer = jumpBufferWindow;
        }
        if (Input.GetButtonUp("Jump"))
        {
            OnJumpUp();
        }
        _moveInput = Input.GetAxis("Horizontal");
        _isGrounded = Physics2D.OverlapBox(
            (Vector2)transform.position + groundCheckOffset, 
            groundCheckRadius, 0, whatIsGround);
        if (_isGrounded && rb.linearVelocity.y <= 0)
        {
            _coyoteTime = coyoteTimeWindow;
            _isJumping = false;
        }

        _coyoteTime -= Time.deltaTime;
        _jumpBuffer -= Time.deltaTime;
        
        _isRightWall = Physics2D.OverlapBox(
            (Vector2)transform.position + rightWallCheckOffset, 
            rightWallCheckRadius, 0, whatIsWall);
        
        _isLeftWall = Physics2D.OverlapBox(
            (Vector2)transform.position + leftWallCheckOffset, 
            leftWallCheckRadius, 0, whatIsWall);
    }

    void WallCling()
    {
        if (
            !_isGrounded &&
            !Input.GetButton("Jump") &&
            (
                (_isLeftWall && _moveInput < 0) ||
                (_isRightWall && _moveInput > 0)
            )
        )
        {
            rb.constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);
        }
        else
        {
            rb.constraints = _defaultConstraints;
        }
    }

    void Flip()
    {
        Vector2 currentScale = gameObject.transform.localScale;
        currentScale.x *= -1;
        gameObject.transform.localScale = currentScale;

        _facingRight = !_facingRight;
    }
    
    private void OnDrawGizmos()
    {
        Gizmos.color = _isGrounded ? Color.green : Color.red;

        Vector2 boxPosition = (Vector2)transform.position + groundCheckOffset;
        Gizmos.DrawWireCube(boxPosition, groundCheckRadius);
        
        Gizmos.color = _isRightWall ? Color.green : Color.red;

        Vector2 rightBoxPosition = (Vector2)transform.position + rightWallCheckOffset;
        Gizmos.DrawWireCube(rightBoxPosition, rightWallCheckRadius);
        
        Gizmos.color = _isLeftWall ? Color.green : Color.red;
        
        Vector2 leftBoxPosition = (Vector2)transform.position + leftWallCheckOffset;
        Gizmos.DrawWireCube(leftBoxPosition, leftWallCheckRadius);
    }
}
