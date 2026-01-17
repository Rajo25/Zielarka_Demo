using UnityEngine;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{
    public Rigidbody2D rb;
    [Header("Movement")]
    public float moveSpeed = 10f;
    public float acceleration = 10f;
    public float deceleration = 10f;
    public float velPower = 10f;
    private float _moveInput;
    public float frictionAmount = 0.2f;
   
    [Header("GroundCheck")]
    private bool _isGrounded;
    public LayerMask whatIsGround;
    public Vector2 groundCheckOffset;
    public Vector2 groundCheckRadius;
    
    [Header("Jump")]
    private float _jumpInput;
    public float jumpForce = 10f;
    public float coyoteTimeWindow = 10f;
    private float _coyoteTime;
    public float jumpBufferWindow = 10f;
    private float _jumpBuffer;
    private bool _isJumping;
    public float jumpCutMultiplier = 10f;
    private bool _jumpInputReleased;
        
    private void FixedUpdate()
    {
        Movement();
        if (_jumpBuffer > 0 && _coyoteTime > 0 && !_isJumping)
        {
            Jump();
        }
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
        _jumpInputReleased = false;
    }

    public void OnJump()
    {
        _jumpBuffer = jumpBufferWindow;
    }

    public void OnJumpUp()
    {
        if (rb.linearVelocity.y > 0 && _isJumping)
        {
            rb.AddForce(Vector2.down * (rb.linearVelocity.y * jumpCutMultiplier), ForceMode2D.Impulse);
        }

        _jumpInputReleased = true;
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
    }
    
    private void OnDrawGizmos()
    {
        Gizmos.color = _isGrounded ? Color.green : Color.red;

        Vector2 boxPosition = (Vector2)transform.position + groundCheckOffset;
        Gizmos.DrawWireCube(boxPosition, groundCheckRadius);
    }
}
