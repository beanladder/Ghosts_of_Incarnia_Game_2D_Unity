using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Pausescript : MonoBehaviour
{
    public GameObject PauseMenu;
    public GameObject SettingPanel;
    public GameObject PauseMenuHolder;
    public bool isPaused;
    // Start is called before the first frame update
    void Start()
    {
        PauseMenu.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)){
            if(isPaused){
                ResumeGame();
            }
            else{
                PauseGame();
            }
        }
    }
    public void BackToMenuButton(){
        SceneManager.LoadScene("UI");
        ResumeGame();
        
    }
    public void PauseGame(){
        PauseMenu.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
    }
    public void ResumeGame(){
        PauseMenu.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }
    public void SettingsButton(){
        PauseMenuHolder.SetActive(false);
        SettingPanel.SetActive(true);
    }
    public void Back(){
        SettingPanel.SetActive(false);
        PauseMenuHolder.SetActive(true);
    }
}
