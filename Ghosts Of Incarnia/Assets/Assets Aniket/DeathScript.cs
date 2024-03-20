using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DeathScript : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject GoBack;
    public static DeathScript Instance;
    private void Awake(){
        Instance = this;
    }
    public void DeathButton(){
        GoBack.SetActive(true);
    }
    public void GoBackButton(){
        SceneManager.LoadScene("UI");
    }
}