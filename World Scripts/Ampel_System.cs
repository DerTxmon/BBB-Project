using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Ampel_System : MonoBehaviour
{
    public Light2D Gruppe_1_Green_Ampel_1, Gruppe_1_Green_Ampel_2;
    public Light2D Gruppe_1_Red_Ampel_1, Gruppe_1_Red_Ampel_2;
    public Light2D Gruppe_2_Red_Ampel_1, Gruppe_2_Red_Ampel_2;
    public Light2D Gruppe_2_Green_Ampel_1, Gruppe_2_Green_Ampel_2;
    //Sprites
    public SpriteRenderer Gruppe_1_Ampel_1_Sprite, Gruppe_1_Ampel_2_Sprite;
    public SpriteRenderer Gruppe_2_Ampel_1_Sprite, Gruppe_2_Ampel_2_Sprite;

    void Start(){
        //Greens
        Gruppe_1_Green_Ampel_1 = Gruppe_1_Green_Ampel_1.GetComponent<Light2D>();
        Gruppe_1_Green_Ampel_2 = Gruppe_1_Green_Ampel_2.GetComponent<Light2D>();
        Gruppe_2_Green_Ampel_1 = Gruppe_2_Green_Ampel_1.GetComponent<Light2D>();
        Gruppe_2_Green_Ampel_2 = Gruppe_2_Green_Ampel_2.GetComponent<Light2D>();
        //Reds
        Gruppe_1_Red_Ampel_1 = Gruppe_1_Red_Ampel_1.GetComponent<Light2D>();
        Gruppe_1_Red_Ampel_2 = Gruppe_1_Red_Ampel_2.GetComponent<Light2D>();
        Gruppe_2_Red_Ampel_1 = Gruppe_2_Red_Ampel_1.GetComponent<Light2D>();
        Gruppe_2_Red_Ampel_2 = Gruppe_2_Red_Ampel_2.GetComponent<Light2D>();
        StartCoroutine(AmpelSystem());
    }

    private IEnumerator AmpelSystem(){
        while(true){
            //UnRed Group 1
            Gruppe_1_Red_Ampel_1.enabled = false;
            Gruppe_1_Red_Ampel_2.enabled = false;
            //Green Group 1
            Gruppe_1_Green_Ampel_1.enabled = true;
            Gruppe_1_Green_Ampel_2.enabled = true;
            //Red Group 2
            Gruppe_2_Red_Ampel_1.enabled = true;
            Gruppe_2_Red_Ampel_2.enabled = true;
            //UnGreen Group 2
            Gruppe_2_Green_Ampel_1.enabled = false;
            Gruppe_2_Green_Ampel_2.enabled = false;
            //Set Sprites to right color
            Gruppe_1_Ampel_1_Sprite.color = Color.green;
            Gruppe_1_Ampel_2_Sprite.color = Color.green;
            Gruppe_2_Ampel_1_Sprite.color = Color.red;
            Gruppe_2_Ampel_2_Sprite.color = Color.red;
            yield return new WaitForSecondsRealtime(8);
            //Red Group 1
            Gruppe_1_Red_Ampel_1.enabled = true;
            Gruppe_1_Red_Ampel_2.enabled = true;
            //UnGreen Group 1
            Gruppe_1_Green_Ampel_1.enabled = false;
            Gruppe_1_Green_Ampel_2.enabled = false;
            //UnRed Group 2
            Gruppe_2_Red_Ampel_1.enabled = false;
            Gruppe_2_Red_Ampel_2.enabled = false;
            //Green Group 2
            Gruppe_2_Green_Ampel_1.enabled = true;
            Gruppe_2_Green_Ampel_2.enabled = true;
            //Set Sprites to right color
            Gruppe_1_Ampel_1_Sprite.color = Color.red;
            Gruppe_1_Ampel_2_Sprite.color = Color.red;
            Gruppe_2_Ampel_1_Sprite.color = Color.green;
            Gruppe_2_Ampel_2_Sprite.color = Color.green;
            yield return new WaitForSecondsRealtime(8);
        }
    }

}
