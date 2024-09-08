using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.U2D.Animation;
using System;
using UnityEngine.UI;

public class Movement : MonoBehaviour
{
    public float speed;
    public float carspeed;
    Rigidbody2D rb;
    public FixedJoystick joystick1;
    public FixedJoystick joystick2;
    public Animator animatior;
    Vector3 lastpos;
    public float rotationSpeed;
    private Vector3 prevPosition = Vector3.zero;
    public GameObject Weapons;
    public Vector3 moveVector;
    public GameObject World;
    public Vector2 movement; 
    public bool tap;
    public Touch touch;
    public Shoot Shoot;
    public GameObject Player;
    public bool crouch;
    public float RotateX, RotateY;
    private bool steping;
    public GameObject Footsteps;
    public GameObject Footsteps2;
    public int schleifenx;
    public bool lootable;
    public bool lootbutton;
    public string Player_Name_Ingame;
    public GameObject Name_Text;
    public GameObject PostProcessingVolume;
    public Sprite ShootRotationhandle;
    public SpriteLibraryAsset DefaultSkinLibrary, AgentSkinLibrary, BetaSkinLibrary, ClownSkinLibrary, AlienSkinLibrary, OttoSkinLibrary, ChrisSkinLibrary;
    private GameObject Joystick1handle;
    public Image Joystick2handle;
    private Shoot shoot;
    private bool shootwhilerotate;
    public Camera MainCam;
    public GameObject Camfollowthis;
    public Transform TargetTransform;
    public GameObject EnterableCar;
    public SpriteRenderer Kopf;
    public SpriteRenderer Arm_Rechts;
    public SpriteRenderer Arm_Links;
    public bool CarStearing;
    [SerializeField] private FixedJoystick joystickfahren;
    [SerializeField] private FixedJoystick joysticklenken;
    private Rigidbody2D CarRB;
    public GameObject CarCamPoint;
    public Audio_Manager audio_manager;
    public bool AimAssistActive;
    public bool isAiming; // To track if aim assist is active
    private Transform enemyTransform;
    public float assistStrength; // Stärke der Korrektur (kleiner Wert = weniger Korrektur, größerer Wert = mehr Korrektur)

    void Awake(){
        Camfollowthis = Player;
        //Skin Laden
        LoadPlayerSkin();
        Joystick1handle = joystick1.transform.Find("Handle").gameObject;
        shoot = GetComponent<Shoot>();
        if(Menu_Handler.localdata.VisionShootSwitch){
            shootwhilerotate = true;
            Joystick2handle.sprite = ShootRotationhandle; //Rotation Handle Auge wird zum Schuss Handle. Schuss handle ist default
        }else shootwhilerotate = false;
        //Setze die Target Transform sofort auf den Spieler
        TargetTransform = Player.transform;
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
    }

    public void LoadPlayerSkin(){
        if(Menu_Handler.loadeddata.SelectedSkin == "Default"){
            Player.GetComponent<SpriteLibrary>().spriteLibraryAsset = DefaultSkinLibrary;
        }else if(Menu_Handler.loadeddata.SelectedSkin == "Beta"){
            Player.GetComponent<SpriteLibrary>().spriteLibraryAsset = BetaSkinLibrary;
        }else if(Menu_Handler.loadeddata.SelectedSkin == "Alien"){
            Player.GetComponent<SpriteLibrary>().spriteLibraryAsset = AlienSkinLibrary;
        }else if(Menu_Handler.loadeddata.SelectedSkin == "Clown"){
            Player.GetComponent<SpriteLibrary>().spriteLibraryAsset = ClownSkinLibrary;
        }else if(Menu_Handler.loadeddata.SelectedSkin == "Otto"){
            Player.GetComponent<SpriteLibrary>().spriteLibraryAsset = OttoSkinLibrary;
        }else if(Menu_Handler.loadeddata.SelectedSkin == "Chris"){
            Player.GetComponent<SpriteLibrary>().spriteLibraryAsset = ChrisSkinLibrary;
        }else if(Menu_Handler.loadeddata.SelectedSkin == "Agent"){
            Player.GetComponent<SpriteLibrary>().spriteLibraryAsset = AgentSkinLibrary;
        }
    }

    void Start()
    {
        //Performance Settings from Menu
        if(Menu_Handler.loadeddata.performancemode == false){
            //-Low Performance settings-
            QualitySettings.vSyncCount = 30;
            Application.targetFrameRate = 30;
            //Disable Post Processing
            PostProcessingVolume.SetActive(false);
        }else{
            //-High Performance settings-
            QualitySettings.vSyncCount = 0;
            Application.targetFrameRate = 120;

            PostProcessingVolume.SetActive(true);
        }
       
        //Drop off ohne Dropoff Handler:
        //Random Position auf der Map
        Player.transform.position = new Vector3(UnityEngine.Random.Range(-266f, 266f), UnityEngine.Random.Range(-266f, 266f), 112.2f); //Map Range
        //Testing
        //Player.transform.position = new Vector3(-164, -248, 112.2f);
        Player_Name_Ingame = Menu_Handler.Player_Name;
        Name_Text.GetComponent<TextMeshPro>().text = Player_Name_Ingame;
        World = GameObject.Find("World");
        rb = GetComponent<Rigidbody2D>();
        animatior = gameObject.GetComponent<Animator>();
        Shoot = Player.GetComponent<Shoot>();
        StartCoroutine(FootstepGen());
    }

    void Update(){
        //Namen überm Kopf anzeigen
        //Name_Text.GetComponent<Rigidbody2D>().rotation = 0;
        //Name_Text.GetComponent<RectTransform>().position = new Vector2(this.gameObject.transform.position.x, this.gameObject.transform.position.y + 1.5f);
        MainCam.transform.position = new Vector3(Camfollowthis.transform.position.x, Camfollowthis.transform.position.y, -10f);
    }

    void FixedUpdate(){   
        float X = joystick1.Horizontal;
        float Y = joystick1.Vertical;

        Vector2 movementDir = new Vector2(X  * speed * Time.fixedDeltaTime, Y  * speed * Time.fixedDeltaTime);

        TargetTransform.Translate(movementDir, Space.World);

        if(transform.position != lastpos){
            animatior.SetBool("isrunning", true);
            //Animation speed
            float animspeed;
            animspeed = ((Vector2.Distance(Joystick1handle.transform.position, joystick1.transform.position)) / 2.37f) * 1.5f; //distanz vom halde vom center berechnen(max ist 2.37 um also auf eine range von 0-1 zu kommen teilen wir durch max)
            animatior.SetFloat("runspeed", animspeed);
            steping = true;
            audio_manager.onGrass = true; //Testing
        }else{
            animatior.SetBool("isrunning", false);
            animatior.SetFloat("runspeed", 1f);
            steping = false;
            audio_manager.onGrass = false;
        }
        lastpos = transform.position;

        //Rotation für rotationsstick setzen
        if(joystick2.Horizontal != 0f || joystick2.Vertical != 0f){
            RotateY = joystick2.Vertical;
            RotateX = joystick2.Horizontal;
            if(shootwhilerotate) shoot.shootbttn2 = true; //Wenn Spieler sich rotiert dann soll er auch schießen (bisschen ekhelig geschrieben)
        }else{
            RotateX = 0f;
            RotateY = 0f;
            //shoot.shootbttn = false;
            shoot.shootbttn2 = false; //Wenn bttn2 false ist kann spieler aber trotzdem noch bttn1 mit dem shoot button im ui auf true setzen
        }

        //in die richtung drehen in die man läuft falls roatations stick nicht bewegt wird
        if(X != 0 || Y != 0 && new Vector2(RotateX,RotateY) == Vector2.zero){
            transform.up = new Vector2(X,Y);
        }

        if(new Vector2(RotateX,RotateY) != Vector2.zero){
            transform.up = new Vector2(RotateX,RotateY);
        }
        // If aim assist is active, adjust the player's rotation
        if (AimAssistActive && enemyTransform != null)
        {
            Vector2 directionToEnemy = (enemyTransform.position - transform.position).normalized;
            float angleToEnemy = Mathf.Atan2(directionToEnemy.y, directionToEnemy.x) * Mathf.Rad2Deg;

            // Berechne den Unterschied zwischen der aktuellen Rotation und der Rotation zum Gegner
            float currentAngle = transform.eulerAngles.z;
            float targetAngle = angleToEnemy - 90f;
            float angleDifference = Mathf.DeltaAngle(currentAngle, targetAngle);

            // Begrenze den Einfluss des Aim-Assists
            float correctedAngle = Mathf.LerpAngle(currentAngle, currentAngle + angleDifference * assistStrength, rotationSpeed * Time.fixedDeltaTime);

            // Setze die Rotation mit der korrigierten Richtung
            transform.rotation = Quaternion.Euler(0, 0, correctedAngle);
        }
    }

    private IEnumerator FootstepGen(){
        begin:
        for( ;steping; ){
            Instantiate(Footsteps,new Vector3(transform.position.x, transform.position.y, 121f), transform.rotation);
            yield return new WaitForSeconds(.13f);
            if(!steping) break;
            Instantiate(Footsteps2, new Vector3(transform.position.x, transform.position.y, 121f), transform.rotation);
            yield return new WaitForSeconds(.13f);
        }
        if(!steping){
            yield return new WaitForSeconds(.13f);
            goto begin;
        }
    }

    public void EnterCar(){
        CarStearing = true;
        //Hide the Player (3 Körperteile)
        Kopf.enabled = false;
        Arm_Rechts.enabled = false;
        Arm_Links.enabled = false;
        //Collider
        this.gameObject.GetComponent<Collider2D>().enabled = false;
        //Hud Aus Schalten
        this.gameObject.GetComponent<UI_Handler>().DeactivateAllHud();
        //Wichtige Hud Elemente wieder Anschalten
        
        this.gameObject.GetComponent<UI_Handler>().ActivateCarHud();
        Camfollowthis = CarCamPoint;
        TargetTransform = EnterableCar.transform;
        CarRB = EnterableCar.GetComponent<Rigidbody2D>();
        //Camera Zoom
        StartCoroutine(GetComponent<Inventory_Handler>().CarZoomOut());
    }
    
    public void AimAssist(Transform EnemyPos)
    {
        AimAssistActive = true;
        enemyTransform = EnemyPos;
    }

    public void StopAssist()
    {
        AimAssistActive = false;
        enemyTransform = null; // Clear target when aim assist is stopped
    }

    public void enterlooting(){
        lootbutton = true;
    }

    public void exitlooting(){
        lootbutton = false;
    }
}