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
    [SerializeField] private float jumpForce = 10f;

    private int currentHealth;
    private string poolName;



    public void Initialize(string poolName)
    {
        this.poolName = poolName;
        ResetMeteor();
    }

    private void ResetMeteor()
    {
        currentHealth = maxHealth;
        UpdateUI();
        rb.velocity = Vector2.right; // di chuyển ngang
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
            if (posX > 0)
                rb.AddForce(Vector2.left * 8f, ForceMode2D.Impulse);
            else
                rb.AddForce(Vector2.right * 8f, ForceMode2D.Impulse);
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

    private void TakeDamage(int damage)
    {
        currentHealth -= damage;
        UpdateUI();

        if (currentHealth <= 0)
        {
            PoolManager.Instance.Despawn(poolName, gameObject); // trả về pool thay vì Destroy
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