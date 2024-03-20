using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class scripts : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject firstHint;
    public GameObject secondHint;
    public GameObject thirdHint; 
    public GameObject larrow;
    public GameObject rarrow;
    public GameObject Begin;

    int flag;
    void Start()
    {
        flag=0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
   public void RightArrow()
{
    firstHint.SetActive(false);
    secondHint.SetActive(true);
    
    flag++;
    // Check if the secondHint is active
    if (flag == 2)
    {
        secondHint.SetActive(false);
        thirdHint.SetActive(true);
    }
    if(thirdHint.activeSelf){
        rarrow.SetActive(false);
        Begin.SetActive(true);
    }
}

    public void BeginButton(){
        SceneManager.LoadScene("MapConvert");
    }
}
