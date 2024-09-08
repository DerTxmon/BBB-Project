using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Schiebetür_Oeffnen : MonoBehaviour
{
    public GameObject Schiebetür_oben;
    public GameObject Schiebetür_unten;
    public float targetScaleSchiebeOben = -914f;
    public float startScaleSchiebeOben = -1255f;
    public float targetScaleSchiebeUnten = 992f;
    public float startScaleSchiebeUnten = 1333f;
    public float Wiederstand_speed;
    private void OnTriggerEnter2D(Collider2D collision){
        StartCoroutine(Oeffnen(collision));
    }
    private IEnumerator Oeffnen(Collider2D collision){
        if(collision.gameObject.tag == "Player" || collision.gameObject.tag == "Bot"){
            while(Schiebetür_oben.transform.localScale.x != -914f || Schiebetür_unten.transform.localScale.x != 992){
                //Schiebetür öffnen
                if(Schiebetür_oben.transform.localScale.x != -914f)
                Schiebetür_oben.transform.localScale = new Vector2(Schiebetür_oben.transform.localScale.x + 1f, Schiebetür_oben.transform.localScale.y);
                if(Schiebetür_unten.transform.localScale.x != 992)
                Schiebetür_unten.transform.localScale = new Vector2(Schiebetür_unten.transform.localScale.x - 1f, Schiebetür_unten.transform.localScale.y);
                yield return new WaitForSeconds(Wiederstand_speed * Time.deltaTime);
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision){
        StartCoroutine(Schliessen(collision));
    }
    private IEnumerator Schliessen(Collider2D collision){
        if(collision.gameObject.tag == "Player" || collision.gameObject.tag == "Bot"){
            while(Schiebetür_oben.transform.localScale.x != -1255f && Schiebetür_unten.transform.localScale.x != 1333f){
                //Schiebetür schließen
                if(Schiebetür_oben.transform.localScale.x != -1255f)
                Schiebetür_oben.transform.localScale = new Vector2(Schiebetür_oben.transform.localScale.x - 1f, Schiebetür_oben.transform.localScale.y);
                if(Schiebetür_unten.transform.localScale.x != 1333f)
                Schiebetür_unten.transform.localScale = new Vector2(Schiebetür_unten.transform.localScale.x + 1f, Schiebetür_unten.transform.localScale.y);
                yield return new WaitForSeconds(Wiederstand_speed * Time.deltaTime);
            }
        }
    }
}
