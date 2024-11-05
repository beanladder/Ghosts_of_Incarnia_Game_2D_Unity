using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerHealth : Singleton<PlayerHealth>
{
    public int maxHealth;
    private Slider healthSlider;
    public float knockBackThurst = 10f;
    public float damageRecoveryTime = 1f;
    public int currentHealth;
    bool canTakeDamage = true;
    private Flash flash;
    private readonly int outOfBoundsDamage = 51; // Damage dealt if player enters "OFB" zone

    protected override void Awake()
    {
        base.Awake();
        flash = GetComponent<Flash>();
    }

    private void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthSlider();
    }

    public void HealPlayer()
    {
        if (currentHealth < maxHealth)
        {
            currentHealth += 10;
            UpdateHealthSlider();
        }
    }

    public void TakeDamage(int damageAmt)
    {
        currentHealth -= damageAmt;
        StartCoroutine(flash.FlashRoutine());
        UpdateHealthSlider();
        DetectDeath();
    }

    private void DetectDeath()
    {
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Debug.Log("Player Died");
            StartCoroutine(SceneTransitionRoutine());
        }
    }

    void UpdateHealthSlider()
    {
        if (healthSlider == null)
        {
            healthSlider = GameObject.Find("HealthSlider").GetComponent<Slider>();
        }
        healthSlider.maxValue = maxHealth;
        healthSlider.value = currentHealth;
    }

    private IEnumerator SceneTransitionRoutine()
    {
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene("PlayerDied");
        Destroy(gameObject);
    }

    // Call TakeDamage when player enters "OFB" tagged area
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("OFB")) // Assuming "OFB" is the tag for Out of Bounds areas
        {
            TakeDamage(outOfBoundsDamage); // Deal 51 damage to instantly kill the player
        }
    }
}
