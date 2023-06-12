using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Menus{
public class Buttons : MonoBehaviour
{
    public BoxCollider2D coll;
    
    private void OnMouseEnter() {//si pasas el mouse por arriba
        this.transform.localScale=.9f*Vector3.one;
        coll.size=14.26f*Vector2.right+4.3f*Vector2.up;
    }
    private void OnMouseExit() {//si sacas el mouse del lugar
        this.transform.localScale=Vector3.one;
        coll.size=13.26f*Vector2.right+3.3f*Vector2.up;
    }
}}
