using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempBuildScript : MonoBehaviour
{
    float timeScale;
    public GameObject pauseScreen;

    // Start is called before the first frame update
    void Start()
    {
        timeScale = 1;
    }

    // Update is called once per frame
    void Update()
    {
        // Debug.Log(1/Time.deltaTime);
        
        if(Input.GetKeyDown(KeyCode.Escape)){
            Pause();
        }

        if(Input.GetKeyDown(KeyCode.Delete)){
            Application.Quit();
        }
    }

    private void Pause(){
        bool isPaused = Time.timeScale == 0;

        if(!isPaused){
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }else{
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        timeScale = isPaused ? 1 : 0;
        Time.timeScale = timeScale;

        pauseScreen.SetActive(!isPaused);
    }
}
