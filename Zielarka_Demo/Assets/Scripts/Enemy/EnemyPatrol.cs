using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    public GameObject pointA;
    public GameObject pointB;
    private Rigidbody2D _rb;
    private Transform _currentPoint;
    public float speed = 2;
    void Start()
    {
        transform.localScale = new Vector3(-1, 1, 1);
        _rb = GetComponent<Rigidbody2D>();
        _currentPoint = pointB.transform;
    }

    
    void Update()
    {
        Vector2 point = _currentPoint.position - transform.position;
        _rb.linearVelocity = _currentPoint == pointB.transform ? new Vector2(speed, 0) : new Vector2(-speed, 0);

        if (Vector2.Distance(transform.position, _currentPoint.position) < 0.5f && _currentPoint == pointB.transform)
        {
            Flip();
            _currentPoint =  pointA.transform;
        }
        if (Vector2.Distance(transform.position, _currentPoint.position) < 0.5f && _currentPoint == pointA.transform)
        {
            Flip();
            _currentPoint =  pointB.transform;
        }
        
    }

    private void Flip()
    {
        Vector3 localScale = transform.localScale;
        localScale.x *= -1;
        transform.localScale = localScale;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(pointA.transform.position, 0.5f);
        Gizmos.DrawWireSphere(pointB.transform.position, 0.5f);
        Gizmos.DrawLine(pointA.transform.position, pointB.transform.position);
    }
}
