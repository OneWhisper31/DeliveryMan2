using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnim : MonoBehaviour
{
    Animator anim;

    private void Start() {
        anim=GetComponent<Animator>();
    }

    private void Update() {
        anim.SetFloat("Horizontal",Input.GetAxis("Horizontal"));
    }


}
