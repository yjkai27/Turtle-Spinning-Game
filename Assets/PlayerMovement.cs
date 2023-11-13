using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float spinMoveSpeed = 200f;
    public float bounceBackForce = 5f;
    public Rigidbody2D rb;
    public SpriteRenderer spriteRenderer;
    public float spinDuration = 30f;

    public Sprite spriteUp;
    public Sprite spriteDown;
    public Sprite spriteLeft;
    public Sprite spriteRight;

    public Sprite spriteUpLeft;
    public Sprite spriteUpRight;
    public Sprite spriteDownLeft;
    public Sprite spriteDownRight;

    public Sprite[] spinningSprites;
    public float spinningSpeed = 0.1f;
    public float spinCooldownTime = 2f;

    private Vector2 moveDirection;
    private bool isSpinning = false;
    private bool canSpin = true;

    private MyProjectGameManager gameManager;

    void Start()
    {
        gameManager = FindObjectOfType<MyProjectGameManager>();
    }

    void Update()
    {
        if (gameManager.gameOverPanel.activeSelf)
        {
            return; 
        }

        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        moveDirection = new Vector2(moveX, moveY).normalized;

        if (Input.GetKeyDown(KeyCode.Space) && !isSpinning && canSpin)
        {
            StartCoroutine(Spin());
        }
        else if (!isSpinning)
        {
            UpdateSpriteDirection(moveX, moveY);
        }
    }

    void FixedUpdate()
    {
        float currentSpeed = isSpinning ? spinMoveSpeed : moveSpeed;
        rb.MovePosition(rb.position + moveDirection * currentSpeed * Time.fixedDeltaTime);
    }

    private float collisionCooldown = 0.2f;
    private float lastCollisionTime;

    private void OnCollisionEnter2D(Collision2D collision)
{
    if (collision.gameObject.CompareTag("crab") && Time.time > lastCollisionTime + collisionCooldown)
    {
        lastCollisionTime = Time.time;

        if (isSpinning)
        {
            collision.gameObject.SendMessage("TakeDamage", 1, SendMessageOptions.DontRequireReceiver);
        }
        else
        {
            TakeDamage(1);
        }

        Vector2 collisionDirection = collision.transform.position - transform.position;
        rb.AddForce(-collisionDirection.normalized * bounceBackForce, ForceMode2D.Impulse);
    }
}


    void UpdateSpriteDirection(float moveX, float moveY)
    {
        if (moveX > 0 && moveY > 0)
            spriteRenderer.sprite = spriteUpRight;
        else if (moveX < 0 && moveY > 0)
            spriteRenderer.sprite = spriteUpLeft;
        else if (moveX < 0 && moveY < 0)
            spriteRenderer.sprite = spriteDownLeft;
        else if (moveX > 0 && moveY < 0)
            spriteRenderer.sprite = spriteDownRight;
        else if (moveX > 0)
            spriteRenderer.sprite = spriteRight;
        else if (moveX < 0)
            spriteRenderer.sprite = spriteLeft;
        else if (moveY > 0)
            spriteRenderer.sprite = spriteUp;
        else if (moveY < 0)
            spriteRenderer.sprite = spriteDown;
    }

    private IEnumerator Spin()
    {
        isSpinning = true;
        canSpin = false;
        rb.angularVelocity = spinMoveSpeed;

        float spinStartTime = Time.time;
        while (isSpinning && (Time.time - spinStartTime) < spinDuration)
        {
            for (int i = 0; i < spinningSprites.Length; i++)
            {
                if (!isSpinning)
                {
                    break;
                }

                spriteRenderer.sprite = spinningSprites[i];
                yield return new WaitForSeconds(spinningSpeed);
            }
            if (!Input.GetKey(KeyCode.Space))
            {
                break;
            }
        }

        rb.angularVelocity = 0;
        isSpinning = false;
        spriteRenderer.sprite = spriteDown;
        UpdateSpriteDirection(moveDirection.x, moveDirection.y);

        yield return new WaitForSeconds(spinCooldownTime);
        canSpin = true;
    }

    public bool IsSpinning()
    {
        return isSpinning;
    }

    public void TakeDamage(int damage)
    {
        FindObjectOfType<HealthManager>().TakeDamage(damage);
    }
}
