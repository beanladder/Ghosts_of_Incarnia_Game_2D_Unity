using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destructible : MonoBehaviour
{
    [SerializeField] GameObject destroyVFX;

    //[SerializeField] AudioSource soundFX;

    [SerializeField] float damageRadius = 5f;
    [SerializeField] int damageAmount = 10;
    [SerializeField] AudioClip destroySound;


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.GetComponent<DamegeSource>() || other.gameObject.GetComponent<Projectile>())
        {

            // GetComponent<PickupSpawner>().DropItems();
            Instantiate(destroyVFX, transform.position, Quaternion.identity);

           

            PlayDestroySound();
            Instantiate(destroyVFX, transform.position, Quaternion.identity);
            DamageNearbyEntities();

            Destroy(gameObject);
        }
    }
    private void PlayDestroySound()
    {
        if (destroySound != null)
        {
            AudioSource.PlayClipAtPoint(destroySound, transform.position);
        }
    }

    private void DamageNearbyEntities()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, damageRadius);

        foreach (Collider2D collider in colliders)
        {
            if (collider.CompareTag("Player"))
            {
                collider.GetComponent<PlayerHealth>().TakeDamage(10);
            }
            else if (collider.CompareTag("GhostTag"))
            {
                collider.GetComponent<EnemyHealth>().TakeDamage(10);
            }
        }
    }
}
