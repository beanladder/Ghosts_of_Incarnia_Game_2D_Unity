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
    }

    public void IncrementKeyPickupCount()
    {
        keyPickupCount++;
        UpdateKeyPickupText();

        if (keyPickupCount >= 6)
        {
            
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

    

    
}
