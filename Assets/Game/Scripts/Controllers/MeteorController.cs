using Game.Core;
using Game.Events;
using Game.Services;
using TMPro;
using UnityEditor.EditorTools;
using UnityEngine;

public class MeteorController : MonoBehaviour, IPoolable
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private TMP_Text textHealth;
    [SerializeField] private int maxHealth = 10;
    private float jumpForce = 12f;

    [SerializeField] private MeteorSize meteorSize = MeteorSize.Large;

    private int currentHealth;
    private string poolName;

    private void Awake()
    {
        if (rb == null)
            rb = GetComponent<Rigidbody2D>();
        gameObject.tag = "Meteor";
    }

    public void Initialize(string poolName)
    {
        this.poolName = poolName;
        ResetMeteor();
    }

    private void ResetMeteor()
    {
        currentHealth = maxHealth;
        UpdateUI();

        rb.mass = 1f;         // Khối lượng hợp lý
        rb.drag = 0.2f;       // Giảm tốc chậm

    }

    public void OnCreate() { }
    public void OnSpawned()
    {
        ResetMeteor();
    }
    public void OnDespawned() { }

    private void OnTriggerEnter2D(Collider2D other)
    {
        //Debug.Log($"Meteor collided with {other.tag}");
        if (other.CompareTag("Missile"))
        {
            TakeDamage(1);
            PoolManager.Instance.Despawn("Missiles", other.gameObject); // trả missile về pool
        }

        if (other.CompareTag("Wall"))
        {
            float posX = transform.position.x;
            //if (posX > 0)
            //    rb.AddForce(Vector2.left * 8f, ForceMode2D.Impulse);
            //else
            //    rb.AddForce(Vector2.right * 8f, ForceMode2D.Impulse);
            if (posX > 0)
                rb.velocity = new Vector2(-Mathf.Abs(rb.velocity.x), rb.velocity.y);
            else
                rb.velocity = new Vector2(Mathf.Abs(rb.velocity.x), rb.velocity.y);

        }

        if (other.CompareTag("Ground"))
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }

        if (other.CompareTag("Player"))
        {
            Debug.Log("Meteor hit player");
            // Trigger player death event
            EventManager.Trigger(new PlayerDeathEvent(
                    GameManager.Instance.Lives,
                    DeathCause.MeteorHit
                ));

            // Destroy meteor sau khi hit player
            PoolManager.Instance.Despawn(poolName, gameObject);
            return;
        }
    }

    public void SetMeteorSize(MeteorSize size)
    {
        meteorSize = size;
        maxHealth = size == MeteorSize.Large ? 10 : size == MeteorSize.Medium ? 5 : 2;
        currentHealth = maxHealth;
        UpdateUI();
    }

    private void TakeDamage(int damage)
    {
        currentHealth -= damage;
        UpdateUI();

        if (currentHealth <= 0)
        {
            EventManager.Trigger(new ScoreChangeEvent(ScoreReason.MeteorDestroyed, 
                meteorSize == MeteorSize.Large ? 100 : 
                meteorSize == MeteorSize.Medium ? 50 : 25));
            if (meteorSize != MeteorSize.Small)
                MeteorSpawner.Instance?.SpawnSplitMeteors(transform.position, meteorSize);

            PoolManager.Instance.Despawn(poolName, gameObject);

            if (Random.Range(0f, 1f) < 0.3f) 
            {
                // Spawn a power-up
            }
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
    Large,
    Medium,
    Small
}