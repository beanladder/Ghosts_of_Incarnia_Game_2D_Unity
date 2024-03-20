using System.Collections;
using TMPro;
using UnityEngine;

public class KeyPickupCounter : MonoBehaviour
{
    public static KeyPickupCounter Instance;
    public TextMeshProUGUI keyPickupText;
    public TextMeshProUGUI additionalText;
    public GameObject SpawnPoint; // Reference to the prefab with CapsuleCollider2D
    public int keyPickupCount = 0;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        SpawnPoint = AssetWarmup.Instance.centerObject;
        UpdateKeyPickupText();
        DisableCollider(); // Disable the collider at the start
    }

    public void IncrementKeyPickupCount()
    {
        keyPickupCount++;
        UpdateKeyPickupText();

        if (keyPickupCount >= 6)
        {
            EnableCollider(); // Enable the collider when key count reaches 6 or more
            StartCoroutine(DisableAdditionalTextAfterDelay(3f));
        }
    }

    private void UpdateKeyPickupText()
    {
        if (keyPickupText != null)
        {
            keyPickupText.text = keyPickupCount.ToString();

            if (keyPickupCount < 6)
            {
                additionalText.gameObject.SetActive(false);
            }

            // Enable the additionalText when key count exceeds 6
            if (keyPickupCount >= 6 && additionalText != null)
            {
                additionalText.gameObject.SetActive(true);
            }
        }
    }

    private IEnumerator DisableAdditionalTextAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (additionalText != null)
        {
            additionalText.gameObject.SetActive(false);
        }
    }

    private void EnableCollider()
    {
        if (SpawnPoint != null)
        {
            CapsuleCollider2D collider = SpawnPoint.GetComponent<CapsuleCollider2D>();
            if (collider != null)
            {
                collider.enabled = true;
            }
        }
    }

    private void DisableCollider()
    {
        if (SpawnPoint != null)
        {
            CapsuleCollider2D collider = SpawnPoint.GetComponent<CapsuleCollider2D>();
            if (collider != null)
            {
                collider.enabled = false;
            }
        }
    }
}
