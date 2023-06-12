using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Credits : MonoBehaviour
{

    public Animator anim;


    private void OnEnable() {
        anim.SetBool("IsOpen",true);
    }

    public void IsClose(){
        anim.SetBool("IsOpen", false);
    }
    public void IsCloseLastFrame(){
        this.gameObject.SetActive(false);
    }


}
