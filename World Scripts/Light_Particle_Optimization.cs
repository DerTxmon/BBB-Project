using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Light_Particle_Optimization : MonoBehaviour
{
    public Light2D Light;
    public ParticleSystem particle;
    public Transform player;
    public Map_Manager mapManager;
    // Start is called before the first frame update
    void Start(){
        player = GameObject.Find("Player").transform;
        mapManager = GameObject.Find("World").GetComponent<Map_Manager>();
        if(GetComponentInChildren<Light2D>() != null){
            Light = GetComponentInChildren<Light2D>();
            Light.enabled = false;
            StartCoroutine(CheckLight());
        }else if(GetComponent<ParticleSystem>() != null){
            particle = GetComponent<ParticleSystem>();
            StartCoroutine(CheckParticle());
        }
    }
    IEnumerator CheckLight(){
        yield return new WaitForSeconds(Random.Range(0, 2f));
        while(true){
            //Check for Distance to Player and if below 40f, enable light
            if(!mapManager.Day){
                if(Vector2.Distance(transform.position, player.position) < 52f){
                    Light.enabled = true;
                }else{
                    Light.enabled = false;
                } 
            }
            yield return new WaitForSeconds(1.8f);
        }
    }
    IEnumerator CheckParticle(){
        yield return new WaitForSeconds(Random.Range(0, 1f));
        while(true){
            //Check for Distance to Player and if below 40f, enable light
            if(Vector2.Distance(transform.position, player.position) < 65f){
                particle.Play();
            }else{
                particle.Pause();
            } 
            yield return new WaitForSeconds(1.8f);
        }
    }
}
