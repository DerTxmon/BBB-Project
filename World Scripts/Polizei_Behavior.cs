using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Polizei_Behavior : MonoBehaviour
{
    public Light2D Red, Blue;
    void Start(){        
        Red.intensity = 0;
        Blue.intensity = 0;
        //Wait between .2 and .5 seconds before starting the flashing
        StartCoroutine(WaitAndStart());
        
    }

    private IEnumerator WaitAndStart(){
        yield return new WaitForSecondsRealtime(Random.Range(0.2f, 1f));
        StartCoroutine(Flashing());
    }

    private IEnumerator Flashing(){
        //Realistic Police Light Flashing
        //Flash 2x Red 2x Blue
        while(true){
            Red.intensity = 1;
            yield return new WaitForSecondsRealtime(0.1f);
            Red.intensity = 0;
            yield return new WaitForSecondsRealtime(0.1f);
            Red.intensity = 1;
            yield return new WaitForSecondsRealtime(0.1f);
            Red.intensity = 0;
            yield return new WaitForSecondsRealtime(0.1f);
            Blue.intensity = 1;
            yield return new WaitForSecondsRealtime(0.1f);
            Blue.intensity = 0;
            yield return new WaitForSecondsRealtime(0.1f);
            Blue.intensity = 1;
            yield return new WaitForSecondsRealtime(0.1f);
            Blue.intensity = 0;
            yield return new WaitForSecondsRealtime(0.1f);
        }
    }
}
