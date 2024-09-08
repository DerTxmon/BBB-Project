using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffect_Manager : MonoBehaviour
{
    public static float OverallVolume;
    public AudioSource[] AllAudioSources;
    public AudioClip NightAmbience, BahnhofDurchsage;
    public AudioSource WorldwideAudioSource;
    public AudioSource BahnhofAudioSource;
    public AudioSource CityAmbience, SchräbeAmbience;

    void Awake(){
        OverallVolume = Menu_Handler.loadeddata.SFX_Volume;
        InitallAudioSources();
    }
    public void Start()
    {
        StartCoroutine(StartBahnhofLoop());
    }

    public IEnumerator SwitchToNightAmbience(){
        //Fade In Night Ambience
        WorldwideAudioSource.clip = NightAmbience;
        WorldwideAudioSource.loop = true;
        WorldwideAudioSource.volume = 0;
        WorldwideAudioSource.Play();
        for (float i = 0; i < 1; i += 0.01f){
            WorldwideAudioSource.volume = i * OverallVolume;
            //Parallel to this, fade out the city ambience and schräbe ambience
            CityAmbience.volume = (1 - i) * OverallVolume;
            SchräbeAmbience.volume = (1 - i) * OverallVolume;
            yield return new WaitForSeconds(0.1f); //Fade in over 1 second
        }
    }
    public IEnumerator SwitchToDayAmbience(){
        //Fade Out Night Ambience
        for (float i = 1; i > 0; i -= 0.01f){
            WorldwideAudioSource.volume = i * OverallVolume;
            //Parallel to this, fade in the city ambience and schräbe ambience
            CityAmbience.volume = (1 + i) * OverallVolume;
            SchräbeAmbience.volume = (1 + i) * OverallVolume;
            yield return new WaitForSeconds(0.1f); //Fade out over 1 second
        }
        WorldwideAudioSource.Stop();
    }
    public IEnumerator StartBahnhofLoop(){
        while(true){
            yield return new WaitForSeconds(BahnhofDurchsage.length * 2);
            BahnhofAudioSource.Play();
        }
    }
    private void InitallAudioSources(){
        AllAudioSources = FindObjectsOfType<AudioSource>();
        foreach(AudioSource audioSource in AllAudioSources){
            audioSource.volume = OverallVolume;
        }
    }
}
