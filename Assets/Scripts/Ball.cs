
using UnityEngine;

public class Ball : MonoBehaviour
{
    [Header("Speed Params")]
    [Tooltip("Start speed at level1")]
    public float baseSpeed = 8f;
    [Tooltip("Speed factor by level (+5%)")]
    public float levelSpeedFactor = 1.05f;
    [Tooltip("Speed up after each collision")]
    public float collisionBoost = 0.2f;
    [Tooltip("Min Speed")]
    public float minSpeed = 4f;
    [Header("Manage the loss of the ball")]
    public float minY = -5.5f;
    [Tooltip("Max Speed")]
    public float maxSpeed = 15f;
    Rigidbody2D rb;
    private float currentSpeed;
    private Vector2 initialDirection = Vector2.down;

    void Start(){
        rb = GetComponent<Rigidbody2D>();
        InitializeSpeed();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (transform.position.y < minY){
            ResetBall();
        }
        if (rb.linearVelocity.magnitude > maxSpeed){
            rb.linearVelocity = rb.linearVelocity.normalized * maxSpeed;
            currentSpeed = maxSpeed;
        }
            
    }

    private void OnCollisionEnter2D(Collision2D collision){
        Brick brick = collision.collider.GetComponent<Brick>();
        
        if (brick != null){
            brick.onHit();
        }
        if(collision.gameObject.CompareTag("Paddle") || collision.gameObject.CompareTag("Walls")){
            currentSpeed = Mathf.Clamp(currentSpeed + collisionBoost, minSpeed, maxSpeed);
            rb.linearVelocity = rb.linearVelocity.normalized * currentSpeed;
        }
        if(collision.gameObject.CompareTag("Paddle")){
            SoundManager.PlaySound(SoundType.PADDLE);
        }
        if(collision.gameObject.CompareTag("Walls")){
            SoundManager.PlaySound(SoundType.WALL);
        }
        
    }

    private void InitializeSpeed(){
        int lvl = GameContext.Instance != null ? GameContext.Instance.LevelToLoad : 1;
        currentSpeed = baseSpeed * Mathf.Pow(levelSpeedFactor, lvl - 1);
        currentSpeed = Mathf.Clamp(currentSpeed, minSpeed, maxSpeed);
        rb.linearVelocity = initialDirection * currentSpeed;
    }

    private void ResetBall(){
        transform.position = Vector3.zero;
        InitializeSpeed();
        if (GameManager.Instance != null){
                GameManager.Instance.LooseLife();
                SoundManager.PlaySound(SoundType.LOOSE_LIFE);
            }
    }
}
