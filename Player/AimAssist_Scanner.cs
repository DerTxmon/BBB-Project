using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimAssist_Scanner : MonoBehaviour
{
    public Movement movement;
    private void OnTriggerEnter2D(Collider2D collision) {
        if(collision.gameObject.tag == "Bot"){
            movement.AimAssist(collision.gameObject.transform);
            movement.AimAssistActive = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision) {
        if(collision.gameObject.tag == "Bot"){
            movement.StopAssist();
            movement.AimAssistActive = false;
        }
    }
}