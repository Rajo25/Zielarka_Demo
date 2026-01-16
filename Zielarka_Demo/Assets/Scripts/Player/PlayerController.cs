using UnityEngine;

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
    private bool _isGrounded = true;
        
    private void FixedUpdate()
    {
        Movement();
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

    void Update()
    {
        _moveInput = Input.GetAxis("Horizontal");
    }
}
