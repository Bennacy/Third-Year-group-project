using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TitleScreen : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Quit(){
        Application.Quit();
    }

    public void Load(string sceneName){
        GameManager.Instance.LoadScene(sceneName);
    }
}
