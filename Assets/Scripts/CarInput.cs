using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarInput : MonoBehaviour
{

    public GameObject returnToMenu;
    public AudioClip motoIdle;
    public AudioClip motoDriving;
    CarMovement carMovement;

    AudioSource audioSource;

    void Start()
    {
        carMovement = GetComponent<CarMovement>();
        audioSource = GetComponent<AudioSource>();
        Cursor.visible=false;
        Cursor.lockState=CursorLockMode.Locked;
    }
    void Update()
    {
        Vector2 inputVector=Vector2.zero;

        inputVector.x=Input.GetAxis("Horizontal");
        inputVector.y=Input.GetAxis("Vertical");

        carMovement.SetInputVector(inputVector);

        if(Input.GetButtonDown("Cancel"))
            returnToMenu.SetActive(true);
        
        SoundSystem();
    }
    public void SoundSystem(){
        if(Input.GetAxisRaw("Vertical")!=0&&audioSource.clip!=motoDriving&&Time.timeScale==1){
            audioSource.clip=motoDriving;
            audioSource.volume=0.5f;            
            audioSource.Play();
        }
        else if(Input.GetAxisRaw("Vertical")==0&&audioSource.clip!=motoIdle&&Time.timeScale==1){
            audioSource.clip=motoIdle;
            audioSource.volume=0.7f;
            audioSource.Play();
        }
        if(Time.timeScale==0)
            audioSource.Pause();
    }
}
