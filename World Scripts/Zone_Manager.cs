using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Zone_Manager : MonoBehaviour
{
    public static Zone_Manager instance;
    private GameObject Player;
    private Transform Zonetransform;
    private Transform toptransform;
    private Transform righttransform;
    private Transform lefttransform;
    private Transform bottomtransform;
    private Vector3 ZoneSize;
    private Vector3 ZonePosition;
    public float ZonenSchrumpfGeschwindigkeit;
    private Vector3 targetZoneSize;
    public bool TakingDamage;
    public float Zoneticktime = 1f;
    public bool InZone;
    public int currentzonedamage = 2;
    public int ZoneMoves = 0;
    public int TimeUntilZone;
    public TextMeshProUGUI ZoneTimerText;
    private void Awake() {
        instance = this;

        Player = GameObject.Find("Player");
        Zonetransform = GameObject.Find("Zone").transform;
        toptransform = GameObject.Find("zone_top").transform;
        lefttransform = GameObject.Find("zone_left").transform;
        righttransform = GameObject.Find("zone_right").transform;
        bottomtransform = GameObject.Find("zone_bottom").transform;
        targetZoneSize = new Vector3(10f,10f,0f);

        SetZoneSize(new Vector3(0f,0f, -9.199997f), new Vector3(640.8889f,640.8889f, 0f));
    }
    private void Start(){
        StartCoroutine(StartZone());
        StartCoroutine(ZoneDamage());
    }

    private IEnumerator StartZone(){
        while(TimeUntilZone > 0){
            yield return new WaitForSecondsRealtime(1f);
            TimeUntilZone--;
            //Show on Time using format ss:mm:hh
            int minutes = TimeUntilZone / 60;
            int seconds = TimeUntilZone % 60;
            ZoneTimerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }
        StartCoroutine(IncreaseZoneDMG());
        while(true){
            ZonenSchrumf();
            yield return new WaitForSeconds(0f);
        }
    }   

    void ZonenSchrumf(){
        Vector3 sizechangeVector = (targetZoneSize - ZoneSize).normalized;
        Vector3 newZoneSize = ZoneSize + sizechangeVector * Time.deltaTime * ZonenSchrumpfGeschwindigkeit;
        SetZoneSize(ZonePosition, newZoneSize);
    }

    private void SetZoneSize(Vector3 position, Vector3 size){
        ZonePosition = position;
        ZoneSize = size;

        //transform.position = position; vorerst unnötig da sich zone nicht bewegen muss
        Zonetransform.localScale = size;

        //Alle Zonen Teile werden auf Die Zonen Größe und Position zugeschnitten.
        toptransform.localScale = new Vector3(2000, 2000);
        toptransform.localPosition = new Vector3(0f, toptransform.localScale.y * .5f + size.y * .5f,-9.199997f);

        bottomtransform.localScale = new Vector3(2000, 2000);
        bottomtransform.localPosition = new Vector3(0f, -toptransform.localScale.y * .5f - size.y * .5f,-9.199997f);

        lefttransform.localScale = new Vector3(2000, size.y);
        lefttransform.localPosition = new Vector3(-lefttransform.localScale.x * .5f - size.x * .5f, 0f, -9.199997f);

        righttransform.localScale = new Vector3(2000, size.y);
        righttransform.localPosition = new Vector3(+lefttransform.localScale.x * .5f + size.x * .5f, 0f, -9.199997f);
    }

    IEnumerator IncreaseZoneDMG(){
        while(true){
            yield return new WaitForSeconds(15f);
            currentzonedamage += 2;
        }
    }
    IEnumerator ZoneDamage(){
        while (true){
            TakingDamage = true; //??
            if(InZone == true) Player.GetComponent<Player_Health>().Damage(currentzonedamage);
            TakingDamage = false; //??
            yield return new WaitForSeconds(Zoneticktime);
        }
    }
}