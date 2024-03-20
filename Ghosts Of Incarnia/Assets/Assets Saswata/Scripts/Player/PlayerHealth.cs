using System;
using System.Collections;
using System.Collections.Generic;
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
    private KnockBack knockBack;
    private Flash flash;
    EnemyHealth enemyHealth;
    //[SerializeField] private GameObject AISword;

    protected override void Awake(){
        base.Awake();
        flash = GetComponent<Flash>();
        knockBack = GetComponent<KnockBack>();
        enemyHealth = GetComponent<EnemyHealth>();
    }
    private void Start(){
        currentHealth = maxHealth;
        UpdateHealthSlider();

    }
    
    public void HealPlayer(){
        if(currentHealth<maxHealth){
            currentHealth+=10;
           UpdateHealthSlider();
        }
        
    }
    public void TakeDamage(int damageAmt){
        if (ShakeManager.Instance != null)
        {
            ShakeManager.Instance.ShakeScreen();
        }
        canTakeDamage = false;
        currentHealth-=damageAmt;
        StartCoroutine(flash.FlashRoutine());
       
        StartCoroutine(DamageRecoveryRoutine());
        UpdateHealthSlider();
        DetectDeath();
    }
    private IEnumerator DamageRecoveryRoutine(){
        yield return new WaitForSeconds(damageRecoveryTime);
        canTakeDamage = true;
    }
    private void DetectDeath()
    {
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Debug.Log("Player Died");

            // Destroy the player game object
            

            
            StartCoroutine(SceneTransitionRoutine());
        }
    }
    void UpdateHealthSlider(){
        if(healthSlider == null){
            healthSlider= GameObject.Find("HealthSlider").GetComponent<Slider>();
        }
        healthSlider.maxValue = maxHealth;
        healthSlider.value = currentHealth;
    }
    private IEnumerator SceneTransitionRoutine()
    {
        yield return new WaitForSeconds(0.5f);

        // Load the "PlayerDied" scene
        SceneManager.LoadScene("PlayerDied");
        Destroy(gameObject);
        // Call DeathButton from DeathScript after a delay (if needed)
        StartCoroutine(SimulateButtonClick());
    }
    private IEnumerator SimulateButtonClick()
    {
        // Wait for a short delay before simulating the button click
        yield return new WaitForSeconds(0.5f);

        // Find the DeathScript instance and call DeathButton
        DeathScript.Instance.DeathButton();
    }

}
