using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;

public class Weapon_Destroy : MonoBehaviour
{
    public GameObject player;
    private Inventory_Handler inv_handler;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player"); 
        //player.GetComponent<Inventory_Handler>();
    }
    private void OnTriggerEnter2D(Collider2D collision){
        if(collision.gameObject.tag == "Player" && collision.GetComponent<Inventory_Handler>().lootcount < 3){
            Destroy(this.gameObject);
            Debug.Log("1");
        }else if(collision.gameObject.tag == "Bot"){
            try{
                //ebug.Log("2");
                if(collision.GetComponent<Bot_Inventory>().lootcount < 3){
                    Destroy(this.gameObject);
                }
            }catch{
                //Debug.Log("3");
                Debug.Log(collision.GetComponent<Mate_Inventory>().lootcount);
                if(collision.GetComponent<Mate_Inventory>().lootcount < 3){
                    Destroy(this.gameObject);
                }
            }
        }
    }
}