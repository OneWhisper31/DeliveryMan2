using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


namespace Menus{
public class MenuButtons : MonoBehaviour
{
    // public SpriteRenderer startButton;    
    // public SpriteRenderer creditsButton;
    // public SpriteRenderer exitButton;

    private void Start() {
        Cursor.visible=true;
        Cursor.lockState=CursorLockMode.None;
    }

    void Update(){
        if(Input.GetButtonDown("Jump")){
            ButtonStart();
        }
        //si toca el espacio y es start se ejecuta la animacion
    }
    public void ButtonStart(){//Ejecutado por el onclick, ejecuta animacion
        SceneManager.LoadScene("Level");
        // PressedButton(startButton);
        // StartCoroutine(PressedStart());
    }
    public void ButtonCredits(){//Ejecutado por el onclick, ejecuta animacion
        //PressedButton(creditsButton);
        StartCoroutine(PressedCredits());
    }
    public void ButtonExit(){//Ejecutado por el onclick, ejecuta animacion
        //PressedButton(exitButton);
        StartCoroutine(PressedExit());
    }
    IEnumerator PressedStart(){

        yield return new WaitForSecondsRealtime(.1f);
        SceneManager.LoadScene("Level");
        //animacion como obanda y dspues carga el nivel
        
        yield break;
    }
    IEnumerator PressedCredits(){
        yield return new WaitForSecondsRealtime(.1f);
        // else if(button=="Credits"&&!alreadyPressed)
        //     credits.SetBool("Credits", !credits.GetBool("Credits"));
        // // 

        yield break;
    }
    IEnumerator PressedExit(){
        //animacion
        yield return new WaitForSecondsRealtime(.1f);
        Application.Quit();
        yield break;
    }
}
}
