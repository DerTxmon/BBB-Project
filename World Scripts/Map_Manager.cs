using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Rendering.Universal;

public class Map_Manager : MonoBehaviour
{
    public int Playercount;
    public float SurvivedTime;
    private GameObject[] x;
    private int y = 1;
    public GameObject[] EntryPoints;
    public float Timespeed;
    public Light2D GlobalLight;
    public GameObject[] Laternen;
    public GameObject[] Waffen_Spawns;
    private GameObject[] Ammo_Spawns;
    [SerializeField] private GameObject Glock;
    [SerializeField] private GameObject M4;
    [SerializeField] private GameObject Sniper;
    [SerializeField] private GameObject AK;
    [SerializeField] private GameObject Small_Ammo;
    [SerializeField] private GameObject Mid_Ammo;
    [SerializeField] private GameObject Big_Ammo;
    [SerializeField] private GameObject Mp7;
    [SerializeField] private GameObject Shotgun;
    private float minus;
    public GameObject Player;   
    public static bool MateIsAlive; 
    public Light2D SichtLicht;
    public bool Day;
    public SoundEffect_Manager SFX_Manager;

    void Awake(){
        //Zeig schonmal die eingestellen Bots an damit das script im hintergrund schonmal zählen kann
        try{
            GameObject.Find("PlayerCount").GetComponent<Text>().text = Menu_Handler.Menu_Bots.ToString(); //try wegen Map manager wird schon im Dropoff geladen
        }catch{}
        //Registriere alle Laternen in eine Array
        Laternen = GameObject.FindGameObjectsWithTag("Laternen");
        //Mache alle Lichter aus
        TurnOffLights();
        //Rotiert durch Tag und nacht durch
        StartCoroutine(WorldTime());
        Waffen_Spawns = GameObject.FindGameObjectsWithTag("Waffen_Spawn"); //für SpawnItems()
        SpawnItems();
        StartCoroutine(Counting());
        if(Menu_Handler.DuoMode){
            MateIsAlive = true;
        }
    }
    void Start()
    {
        StartCoroutine(PlayerCheck());
    }

    private IEnumerator Counting(){
        while(true){
            yield return new WaitForSeconds(1f);
            if(Time.timeScale == 1) SurvivedTime += 1; //Zeit zählen wenn nicht im Map menü oder im endscreen 
        }
    }

    private bool lightsOn = false;

    private IEnumerator WorldTime() {
        while (true) {
            // Es wird Nacht
            while (GlobalLight.intensity > .25f) {
                GlobalLight.intensity -= .0075f * Time.deltaTime;

                // Sichtlicht-Intensität logarithmisch anpassen und gegen Ende beschleunigen
                float t = (0.9f - GlobalLight.intensity) / (0.9f - 0.25f);
                SichtLicht.intensity = Mathf.Lerp(0f, 0.6f, 1 - Mathf.Log10(1 + 9 * (1 - t)));  // Umgekehrte logarithmische Skalierung

                // Mach Lichter AN, wenn Abend (.5f) erreicht wird und Lichter noch nicht an sind
                if (GlobalLight.intensity < .5f && !lightsOn) {
                    StartCoroutine(SFX_Manager.SwitchToNightAmbience());
                    TurnOnLights();
                    lightsOn = true;
                }
                yield return new WaitForSeconds(Timespeed);
            }

            // Es wird Tag
            while (GlobalLight.intensity < .9f) {
                GlobalLight.intensity += .015f * Time.deltaTime;

                // Sichtlicht-Intensität logarithmisch anpassen und gegen Ende beschleunigen
                float t = (GlobalLight.intensity - 0.25f) / (0.9f - 0.25f);
                SichtLicht.intensity = Mathf.Lerp(0.6f, 0f, 1 - Mathf.Log10(1 + 9 * (1 - t)));  // Umgekehrte logarithmische Skalierung

                // Mach Lichter AUS, wenn Morgen (.5f) erreicht wird und Lichter noch an sind
                if (GlobalLight.intensity > .5f && lightsOn) {
                    StartCoroutine(SFX_Manager.SwitchToDayAmbience());
                    TurnOffLights();
                    lightsOn = false;
                }
                yield return new WaitForSeconds(Timespeed);
            }
        }
    }

    private void TurnOnLights(){
        Day = false;
        foreach(GameObject i in Laternen){
            i.GetComponentInChildren<Light2D>().enabled = true;
        }
    }

    private void TurnOffLights(){
        Day = true;
        foreach(GameObject i in Laternen){
            i.GetComponentInChildren<Light2D>().enabled = false;
        }
    }

    IEnumerator PlayerCheck(){
        yield return new WaitForSeconds(1f); //Warte bis die ersten bots spawnen damit das spiel nicht denkt das man sofort gewonnen hat. alle bots werden in der 1 sec gespawned
        bool won = false;
        while(true){
        try{
            x = GameObject.FindGameObjectsWithTag("Bot"); //Kostet leistung ist aber schnelli zu schreiben mann könnte aber alle gespawnen spieler beim spawnen in eine liste
            Playercount = x.Length; //packen und dann hier in einer schleife checken ob dieser slot in der array null ist oder nicht und daran dann checken wie viele spieler noch leben
            x = GameObject.FindGameObjectsWithTag("Player");
            Playercount += x.Length;
            GameObject.Find("PlayerCount").GetComponent<TextMeshProUGUI>().text = Playercount.ToString();
        }catch{}

        if(!Menu_Handler.DuoMode && Menu_Handler.loadeddata.TurnOffBots == false){ //Solo
                if(Playercount == 1 && won == false){ //Wenn nur noch ein spieler lebt nur einmal ausführen weil loop leuft weiter und würde dann immer wieder ausführen
                won = true;
                Win();
                break;
            }
        }else if(Menu_Handler.DuoMode && Menu_Handler.loadeddata.TurnOffBots == false){ //Duo
            if(Playercount == 2 && won == false){ //Wenn nur noch ein spieler lebt nur einmal ausführen weil loop leuft weiter und würde dann immer wieder ausführen
                won = true;
                Win();
                break;
            }
        }
        if(Playercount <= 5){ //Wenn nur noch 5 Spieler leben check alle .5sec nach playercount
            minus = -1.5f;
        }
        yield return new WaitForSeconds(2f + minus);
        }
    }

    public void  Win(){
        StartCoroutine(Player.GetComponent<UI_Handler>().EndScreen(true));
    }

    public static void DeactivateAllBots(){
        GameObject[] bots = GameObject.FindGameObjectsWithTag("Bot");
        foreach(GameObject i in bots){
            if(i.GetComponent<Bot_Behavior>() != null) i.GetComponent<Bot_Behavior>().enabled = false;
            else if(i.GetComponent<Mate_Behavior>()) i.GetComponent<Mate_Behavior>().enabled = false;
        }
    }

    private void SpawnItems(){
        //Waffen
        foreach(GameObject i in Waffen_Spawns){
            int rand = Random.Range(1,3); //(Kann nur zwischen 1 und 2 treffen) Rechne die chance ob überhaupt auf diesem spot irgendwas spawnen soll (50/50)
            if(rand == 1){
                //Rechne die chancen für die Spawnende Waffe aus
                rand = Random.Range(-1,150);//0-100

                if(rand < 41 && rand > -1){//0-40 (40%)
                //Glock
                    GameObject item = Instantiate(Glock, new Vector3(i.transform.position.x, i.transform.position.y, 120f), Quaternion.identity);
                    item.GetComponent<Weapon_Info>().Currentammo = 12;
                    item = Instantiate(Small_Ammo, new Vector3(i.transform.position.x + 0.3f, i.transform.position.y - 0.3f, 120f), Quaternion.identity);
                    item.GetComponent<Ammo_Info>().Ammo = 20;
                //Small Ammo
                    //GameObject item = 
                }else if(rand < 52 && rand > 40){ //41-51 (10%)
                //Sniper
                    GameObject item = Instantiate(Sniper, new Vector3(i.transform.position.x, i.transform.position.y, 120f), Quaternion.identity);
                    item.GetComponent<Weapon_Info>().Currentammo = 5;
                    item = Instantiate(Big_Ammo, new Vector3(i.transform.position.x + 0.3f, i.transform.position.y - 0.3f, 120f), Quaternion.identity);
                    item.GetComponent<Ammo_Info>().Ammo = 2;
                }else if(rand < 83 && rand > 51){ //52-82 (30%)
                //M4
                    GameObject item = Instantiate(M4, new Vector3(i.transform.position.x, i.transform.position.y, 120f), Quaternion.identity);
                    item.GetComponent<Weapon_Info>().Currentammo = 30;
                    item = Instantiate(Mid_Ammo, new Vector3(i.transform.position.x + 0.3f, i.transform.position.y - 0.3f, 120f), Quaternion.identity);
                    item.GetComponent<Ammo_Info>().Ammo = 20;
                }else if(rand < 104 && rand > 82){ //83-103 (20%)
                //AK
                    GameObject item = Instantiate(AK, new Vector3(i.transform.position.x, i.transform.position.y, 120f), Quaternion.identity);
                    item.GetComponent<Weapon_Info>().Currentammo = 25;
                    item = Instantiate(Mid_Ammo, new Vector3(i.transform.position.x + 0.3f, i.transform.position.y - 0.3f, 120f), Quaternion.identity);
                    item.GetComponent<Ammo_Info>().Ammo = 10;
                }else if(rand < 121 && rand > 103){ //104-120 (16%)
                //Mp7
                    GameObject item = Instantiate(Mp7, new Vector3(i.transform.position.x, i.transform.position.y, 120f), Quaternion.identity);
                    item.GetComponent<Weapon_Info>().Currentammo = 20;
                    item = Instantiate(Small_Ammo, new Vector3(i.transform.position.x + 0.3f, i.transform.position.y - 0.3f, 120f), Quaternion.identity);
                    item.GetComponent<Ammo_Info>().Ammo = 20;
                }else if(rand <= 150 && rand > 122){ //%?
                //Shotgun
                    GameObject item = Instantiate(Mp7, new Vector3(i.transform.position.x, i.transform.position.y, 120f), Quaternion.identity);
                    item.GetComponent<Weapon_Info>().Currentammo = 5;
                    item = Instantiate(Small_Ammo, new Vector3(i.transform.position.x + 0.3f, i.transform.position.y - 0.3f, 120f), Quaternion.identity);
                    item.GetComponent<Ammo_Info>().Ammo = 5;
                }
            }
        }
    }
}
