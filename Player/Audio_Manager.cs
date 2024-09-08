using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Audio_Manager : MonoBehaviour
{
    public AudioSource playerAudioSource;
    //Clips
    int randomClip;
    public AudioClip[] run_grass = new AudioClip[4];
    public AudioClip[] run_sand = new AudioClip[4];
    public AudioClip[] run_wood = new AudioClip[4];
    public AudioClip[] run_conreed = new AudioClip[4];
    public bool onGrass, onSand, onWood, onConreed;

    public void Start(){
        StartCoroutine(PlayRunOnGrass());
        
    }
    public IEnumerator PlayRunOnGrass(){
        while(true){
            if(onGrass){
                //Randomly select a clip from the run_grass array and play it and make sure its not the same clip as the previous one
                randomClip = Random.Range(0, run_grass.Length);
                playerAudioSource.clip = run_grass[randomClip];
                playerAudioSource.Play();
            }
            //Wait for clip length
            yield return new WaitForSeconds(run_grass[randomClip].length);
        }
    }
    public IEnumerator PlayRunOnSand(){
        while(onSand){
            //Randomly select a clip from the run_sand array and play it and make sure its not the same clip as the previous one
            int randomClip = Random.Range(0, run_sand.Length);
            playerAudioSource.clip = run_sand[randomClip];
            playerAudioSource.Play();
            yield return new WaitForSeconds(0.3f);
        }
    }
    public IEnumerator PlayRunOnWood(){
        while(onWood){
            //Randomly select a clip from the run_wood array and play it and make sure its not the same clip as the previous one
            int randomClip = Random.Range(0, run_wood.Length);
            playerAudioSource.clip = run_wood[randomClip];
            playerAudioSource.Play();
            yield return new WaitForSeconds(0.3f);
        }
    }
    public IEnumerator PlayRunOnConreed(){
        while(onConreed){
            //Randomly select a clip from the run_conreed array and play it and make sure its not the same clip as the previous one
            int randomClip = Random.Range(0, run_conreed.Length);
            playerAudioSource.clip = run_conreed[randomClip];
            playerAudioSource.Play();
            yield return new WaitForSeconds(0.3f);
        }
    }
    public void StopRunningAudio(){
        playerAudioSource.Stop();
        playerAudioSource.clip = null;
    }
}
