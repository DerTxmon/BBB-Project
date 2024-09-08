using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public class Dropped_Item_Optimization : MonoBehaviour{
    public GameObject Item;
    public GameObject Player;
    private Animator animator;
    void Awake()
    {
        Item = gameObject;
        animator = Item.GetComponent<Animator>();
        Player = GameObject.Find("Player");
    }

    void Start(){
        StartCoroutine(CheckforRender());
    }

    private IEnumerator CheckforRender(){
        while(true){
            if(Vector2.Distance(Player.transform.position, Item.transform.position) > 20){
                animator.enabled = false;
            }else{
                animator.enabled = true;
            }
            yield return new WaitForSeconds(2f);
        }
    }
}
