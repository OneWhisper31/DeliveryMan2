using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Civil{
public class CivilChooser : MonoBehaviour
{

    public AnimatorOverrideController[] civilians;

    private void Awake() {//change the skins of the civilians with RNG

        int random = Random.Range(0,4);
        switch (random){
            case 0:
            GetComponent<Animator>().runtimeAnimatorController=civilians[0];
            Destroy(this);                            
            
            break;
            case 1:
            GetComponent<Animator>().runtimeAnimatorController=civilians[1];
            Destroy(this);                
            
            break;
            case 2:
            GetComponent<Animator>().runtimeAnimatorController=civilians[2];
            Destroy(this);

            break;
            case 3:
            Destroy(this);
            
            break;
            default:break;
        }
    }

}}
