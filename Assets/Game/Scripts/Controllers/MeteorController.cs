using UnityEngine;
using TMPro;
using Game.Services;

public class MeteorController : MonoBehaviour, IPoolable
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private int maxHealth = 3;
    [SerializeField] private TMP_Text textHealth;
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
        if (other.CompareTag("Missile"))
        {
            TakeDamage(1);
            PoolMangager.Instance.Despawn("Missiles", other.gameObject); // trả missile về pool
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
            PoolMangager.Instance.Despawn(poolName, gameObject); // trả về pool thay vì Destroy
        }
    }

    private void UpdateUI()
    {
        if (textHealth != null)
            textHealth.text = currentHealth.ToString();
    }
}
