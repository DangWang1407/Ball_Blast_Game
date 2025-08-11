using UnityEngine;
using TMPro;
using Game.Services;
using Game.Events;

public class MeteorController : MonoBehaviour, IPoolable
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private TMP_Text textHealth;
    [SerializeField] private float jumpForce = 10f;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private CircleCollider2D col;
    [SerializeField] private int splitCount = 2;

    private int currentHealth;
    private int maxHealth;
    private string poolName;
    private MeteorSize meteorSize = MeteorSize.Large;

    // Size configurations
    private static readonly Vector3[] sizes = {
        Vector3.one * 1.0f,    // Large
        Vector3.one * 0.6f,    // Medium  
        Vector3.one * 0.3f     // Small
    };

    private static readonly Color[] colors = {
        new Color(1f, 0.3f, 0.3f, 1f),  // Large - Red
        new Color(1f, 0.8f, 0.2f, 1f),  // Medium - Orange
        new Color(0.3f, 1f, 0.3f, 1f)   // Small - Green
    };

    private static readonly int[] healthBySize = { 3, 2, 1 }; // Large, Medium, Small

    public void Initialize(string poolName)
    {
        this.poolName = poolName;
        ResetMeteor();
    }

    public void SetMeteorSize(MeteorSize size)
    {
        meteorSize = size;
        int index = (int)size;

        // Set scale
        transform.localScale = sizes[index];

        // Set color
        if (spriteRenderer != null)
            spriteRenderer.color = colors[index];

        // Set health by size
        maxHealth = healthBySize[index];
        currentHealth = maxHealth;

        // Adjust collider size
        if (col != null)
            col.radius = 1.0f; // Unity auto-scales with transform

        UpdateUI();
    }

    private void ResetMeteor()
    {
        SetMeteorSize(meteorSize);
        rb.velocity = Vector2.right; // Default movement
    }

    public void OnCreate() { }

    public void OnSpawned()
    {
        ResetMeteor();
    }

    public void OnDespawned()
    {
        rb.velocity = Vector2.zero;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Missile"))
        {
            TakeDamage(1);
            PoolManager.Instance.Despawn("Missiles", other.gameObject);
        }

        if (other.CompareTag("Player"))
        {
            // Trigger player death event
            EventManager.Trigger(new PlayerDeathEvent(0, DeathCause.MeteorHit));

            // Destroy meteor after hitting player
            PoolManager.Instance.Despawn(poolName, gameObject);
            return;
        }

        if (other.CompareTag("Wall"))
        {
            float posX = transform.position.x;
            if (posX > 0)
                rb.AddForce(Vector2.left * 8f, ForceMode2D.Impulse);
            else
                rb.AddForce(Vector2.right * 8f, ForceMode2D.Impulse);
        }

        if (other.CompareTag("Ground"))
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }
    }

    private void TakeDamage(int damage)
    {
        currentHealth -= damage;
        UpdateUI();

        if (currentHealth <= 0)
        {
            SplitMeteor();
        }
    }

    private void SplitMeteor()
    {
        // Only split if not the smallest size
        if (meteorSize != MeteorSize.Small)
        {
            MeteorSize nextSize = meteorSize + 1;

            for (int i = 0; i < splitCount; i++)
            {
                // Random direction for split
                Vector2 splitDirection = Random.insideUnitCircle.normalized;
                Vector3 spawnPos = transform.position + (Vector3)splitDirection * 0.5f;

                GameObject smallMeteor = PoolManager.Instance.Spawn(poolName, spawnPos);
                if (smallMeteor != null)
                {
                    var controller = smallMeteor.GetComponent<MeteorController>();
                    controller.SetMeteorSize(nextSize);
                    controller.Initialize(poolName);

                    // Set velocity for split meteor
                    Rigidbody2D smallRb = smallMeteor.GetComponent<Rigidbody2D>();
                    smallRb.velocity = splitDirection * 4f;
                }
            }
        }

        // Trigger score event based on size
        int scoreValue = GetScoreBySize();
        EventManager.Trigger(new ScoreUpdateEvent(scoreValue, 0, ScoreReason.MeteorDestroyed));

        // Destroy current meteor
        PoolManager.Instance.Despawn(poolName, gameObject);
    }

    private int GetScoreBySize()
    {
        switch (meteorSize)
        {
            case MeteorSize.Large: return 20;
            case MeteorSize.Medium: return 50;
            case MeteorSize.Small: return 100;
            default: return 10;
        }
    }

    private void UpdateUI()
    {
        if (textHealth != null)
            textHealth.text = currentHealth.ToString();
    }
}

public enum MeteorSize
{
    Large = 0,
    Medium = 1,
    Small = 2
}