using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIInGame : MonoBehaviour
{
    private void OnEnable() {
        Time.timeScale=0;
        Cursor.visible=true;
        Cursor.lockState=CursorLockMode.None;
    }
    private void OnDisable() {
        Time.timeScale=1;
        Cursor.visible=false;
        Cursor.lockState=CursorLockMode.Locked;
    }

    public void onResetClicked(){
        SceneManager.LoadScene("Level");
    }
    public void onMenuClicked(){
        SceneManager.LoadScene("Menu");
    }

}
