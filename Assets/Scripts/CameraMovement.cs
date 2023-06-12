using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public float delay;
    
    public Transform player;

    private void FixedUpdate() {
        if(player==null) return;
        Vector3 _player = new Vector3(player.position.x,player.position.y,this.transform.position.z);
        this.transform.position=Vector3.Lerp
        (this.transform.position,_player,delay);
    }
}
