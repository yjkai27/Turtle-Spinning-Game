using System.Collections;
using UnityEngine;

public class CrabBehavior : MonoBehaviour
{
    public float moveSpeed = 5f;
    public SpriteRenderer spriteRenderer;

    public Sprite[] moveSpritesDownwards;
    public Sprite[] moveSpritesUpwards;
    public Sprite[] attackSpritesDownwards;
    public Sprite[] attackSpritesUpwards;
    public float spriteChangeRate = 0.25f;
    public float attackDistance = 1.0f;

    private GameObject turtle;
    private bool isAttacking = false;
    private bool inAttackRange = false;

    private MyProjectGameManager gameManager;

    private void Awake()
    {
        turtle = GameObject.Find("blast5");
        gameManager = FindObjectOfType<MyProjectGameManager>();
        if (turtle == null)
        {
            Debug.LogError("Turtle named 'blast5' was not found in the scene.");
        }
    }

    private void Start()
    {
        if (turtle != null)
        {
            StartCoroutine(MoveAndAttack());
            StartCoroutine(AnimateMovement());
        }
    }

    private IEnumerator MoveAndAttack()
    {
        while (true)
        {
            if (gameManager.gameOverPanel.activeSelf)
            {
                yield break; 
            }

            if (turtle == null)
            {
                Debug.LogError("Turtle reference lost. Stopping crab behavior.");
                yield break;
            }

            if (!isAttacking)
            {
                Vector3 targetDirection = (turtle.transform.position - transform.position).normalized;
                transform.position += targetDirection * moveSpeed * Time.deltaTime;

                float distanceToTurtle = Vector3.Distance(transform.position, turtle.transform.position);
                inAttackRange = distanceToTurtle < attackDistance;

                if (inAttackRange)
                {
                    yield return StartCoroutine(Attack());
                }
            }

            yield return null;
        }
    }

    private IEnumerator AnimateMovement()
    {
        while (true)
        {
            bool movingDownwards = transform.position.y > turtle.transform.position.y;
            Sprite[] currentMoveSprites = movingDownwards ? moveSpritesDownwards : moveSpritesUpwards;

            foreach (Sprite sprite in currentMoveSprites)
            {
                if (isAttacking) break;

                spriteRenderer.sprite = sprite;
                yield return new WaitForSeconds(spriteChangeRate);
            }

            yield return null;
        }
    }

    private IEnumerator Attack()
    {
        isAttacking = true;

        bool attackingDownwards = transform.position.y > turtle.transform.position.y;
        Sprite[] currentAttackSprites = attackingDownwards ? attackSpritesDownwards : attackSpritesUpwards;

        bool damageApplied = false;

        foreach (Sprite attackSprite in currentAttackSprites)
        {
            spriteRenderer.sprite = attackSprite;
            yield return new WaitForSeconds(spriteChangeRate);

            if (inAttackRange && !turtle.GetComponent<PlayerMovement>().IsSpinning() && !damageApplied)
            {
                turtle.SendMessage("TakeDamage", 1, SendMessageOptions.DontRequireReceiver);
                damageApplied = true;
            }
        }

        isAttacking = false;
    }


    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }

    private int hitCount = 0;
    private const int MaxHits = 5;

    public void TakeDamage(int damage)
    {
        hitCount += damage;
        if (hitCount >= MaxHits)
        {
            Destroy(gameObject);
            FindObjectOfType<ScoreManager>().AddScore(1);
        }
    }
}
