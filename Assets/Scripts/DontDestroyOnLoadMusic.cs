using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DontDestroyOnLoadMusic : MonoBehaviour
{
    public AudioClip[] clips;
    AudioSource audioSource;
    float currentTrack;

    private void Awake() {
        var music = FindObjectOfType<DontDestroyOnLoadMusic>().gameObject;
        if(music&&music!=this.gameObject){
            Destroy(this.gameObject);
            return;
        }
        else
            DontDestroyOnLoad(this.gameObject);    
        audioSource = GetComponent<AudioSource>();
        audioSource.PlayOneShot(ChooseNewTheme(true));
        //SceneManager.LoadScene("Level 01");
    }

    private void Update() {
        if(!audioSource.isPlaying)//si ya no hay pistas para reproduciendo, que elija una nueva cancion
            audioSource.PlayOneShot(ChooseNewTheme(false));
    }

    AudioClip ChooseNewTheme(bool isOnAwake){
        int random = Random.Range(0, clips.Length);//si esta en el awake que elija cualquier tema
        if(!isOnAwake&&random==currentTrack)//si no esta en el awake, que se fija q no eligio el mismo de antes
            return ChooseNewTheme(false);
        else{
            currentTrack=random;//setea el currenttrack y
            return clips[random];//devuelve el clip
        }
    }
}
