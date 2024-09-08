using System.Collections;
using System.Linq;
using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class Mate_Behavior : MonoBehaviour
{
    public float speed;
    public Animator animatior;
    Vector3 lastpos;
    private Vector3 prevPosition = Vector3.zero;
    public GameObject Weapons;
    public GameObject World;
    public float X;
    public float Y;
    public float BewegungsängerungsZeit;
    public int Zyklus;
    private bool waiting, waiting1;
    Quaternion rot;
    Vector2 movement; 
    Rigidbody2D rb;
    public int Z;
    public bool EnemyContact, EnemyContact2 = false;
    public GameObject Enemy;
    private Vector2 movementdir;
    private float wait;
    public int schleifenx, schleifeny, schleifenz, schleifeni;
    public int newrandom;
    public bool Jiggle;
    public bool Shoot;
    public bool runaway;
    public GameObject Mate;
    public int randomizerweapon, randomizerweaponmax;
    public float angle;
    private float rotationoffset = 270f;
    public float missaim;
    private bool steping;
    public GameObject Footsteps;
    public GameObject Footsteps2;
    public GameObject Currloot;
    public GameObject EntryPoint;
    public bool looting;
    public bool lootable, lootbutton;
    public bool Wallinfront;
    public bool inhouse;
    public float rightsum, leftsum, topsum, bottomsum; //summe der rechten, linken, oberen und unteren nerven distances
    public float[] sums = new float[4];
    public float Player_Distance;
    public bool spawn_steps;
    public GameObject Player, Mate_Name_Text, Healthbar;
    private Mate_Optimazation Mate_optimazation;
    private Rigidbody2D Mate_Name_Text_rb, Mate_Healthbar_rb;
    private RectTransform Mate_Name_Text_transform;
    private Mate_Shoot Mate_shoot;
    private Mate_Inventory inv;
    private Animator anim;
    public bool forceflee;
    public Transform[] HousePoints;
    private Mate_Health mate_health;
    private Mate_Inventory mate_inv;
    public bool nowfollowing;
    public bool movenormal;
    public float followspeedoffset = -.5f;
    private float Mate_Extra_Distance;
    private Mate_PlayerContact mate_playercontact;
    public bool AvoidingWall;

    void Awake() {
        BewegungsängerungsZeit = 15f;
        Mate_optimazation = this.gameObject.GetComponent<Mate_Optimazation>();
        Mate_Name_Text_rb = Mate_Name_Text.GetComponent<Rigidbody2D>();
        Mate_Name_Text_transform = Mate_Name_Text.GetComponent<RectTransform>();
        Mate_shoot = Mate.GetComponent<Mate_Shoot>();
        Mate_Healthbar_rb = Healthbar.GetComponent<Rigidbody2D>();
        inv = Mate.GetComponent<Mate_Inventory>();
        anim = Mate.GetComponent<Animator>();
        mate_health = Mate.GetComponent<Mate_Health>();
        mate_inv = Mate.GetComponent<Mate_Inventory>();
        mate_playercontact = Mate.GetComponentInChildren<Mate_PlayerContact>();
    }

    void Start()
    {
        Player = GameObject.Find("Player");
        World = GameObject.Find("World");
        animatior = gameObject.GetComponent<Animator>();        
        rb = GetComponent<Rigidbody2D>();
        StartCoroutine(FootstepGen());
        StartCoroutine(CheckforPlayerDistance());
    }

    void Update()
    {
        Mate_Name_Text_rb.rotation = 0;
        Mate_Name_Text_transform.position = Mate.transform.position + new Vector3(0, 1.5f, 0);

        Mate_Healthbar_rb.rotation = 0;
        Healthbar.transform.position = Mate.transform.position + new Vector3(.5f, 0, 0);

        if(transform.position != lastpos){
            steping = true;
        }else{
            steping = false;
        }
        lastpos = transform.position;

        if(Mate_optimazation.is_object_in_range == true){
            StartCoroutine(CheckforY());
            StartCoroutine(EnemyContactfunc());
            StartCoroutine(Looting());
        }

        //Wall in front of Mate
        if(mate_playercontact.WallDetected == true && !inhouse){
            AvoidWall();
            AvoidingWall = true;
        }else AvoidingWall = false;

        if(Enemy == null && !looting && !EnemyContact && !AvoidingWall) Mate_Follow_Player();
    }

    void FixedUpdate() 
    {
        Vector2 movementdir =  new Vector2(X,Y);
        if(movenormal == true){
            transform.Translate(movementdir * speed * Time.fixedDeltaTime, Space.World);
        }else{
            transform.Translate(movementdir * speed * Time.fixedDeltaTime, Space.Self);
        }

        //Animation
        if(transform.position != lastpos){
            animatior.SetBool("isrunning", true);
        }else{
            animatior.SetBool("isrunning", false);
        }
        lastpos = transform.position;
    }

    private void AvoidWall() {
        // Wenn rechts eine Wand erkannt wird, aber links frei ist, drehe leicht nach links
        if (mate_playercontact.WallDetectedRight && !mate_playercontact.WallDetectedLeft)
        {
            X = Mathf.Lerp(X, -1, 0.1f);  // Leichte Drehung nach links
        }
        // Wenn links eine Wand erkannt wird, aber rechts frei ist, drehe leicht nach rechts
        else if (mate_playercontact.WallDetectedLeft && !mate_playercontact.WallDetectedRight)
        {
            X = Mathf.Lerp(X, 1, 0.1f);  // Leichte Drehung nach rechts
        }
        // Wenn auf beiden Seiten eine Wand erkannt wird, und die rechte Seite mehr Platz hat
        else if (rightsum > leftsum)
        {
            X = Mathf.Lerp(X, -1, 0.1f);  // Leichte Drehung nach links
        }
        // Wenn auf beiden Seiten eine Wand erkannt wird, und die linke Seite mehr Platz hat
        else if (leftsum > rightsum)
        {
            X = Mathf.Lerp(X, 1, 0.1f);  // Leichte Drehung nach rechts
        }
    }



    private void Mate_Follow_Player(){
        float distanceToPlayer = Vector2.Distance(transform.position, Player.transform.position);

        //Check ob Mate in einem Haus ist und wenn ja, dann folge dem Spieler noch näher
        if(inhouse) Mate_Extra_Distance = -3.5f;
        else Mate_Extra_Distance = 0f;

        if(distanceToPlayer >= 10f && !EnemyContact && EntryPoint == null && !looting){
            //10f Weit weg

            Vector2 lookDir = new Vector2(Player.transform.position.x, Player.transform.position.y) - rb.position;
            angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg + rotationoffset; // Apply rotation offset
            rb.rotation = angle;

            //Wenn followspeedoffset auf -.5f, dann +.002f pro frame bis 0 erreicht wird damit er sich nicht zu schnell bewegt
            if(followspeedoffset < 0){
                followspeedoffset += .002f;
            }
            X = 0;
            Y = 1.05f + followspeedoffset;

            nowfollowing = true;
            movenormal = false;
        }else if(distanceToPlayer < 10f && distanceToPlayer > 5f + Mate_Extra_Distance && !EnemyContact && EntryPoint == null && !looting){
            //5f Weit weg
            Vector2 lookDir = new Vector2(Player.transform.position.x, Player.transform.position.y) - rb.position;
            angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg + rotationoffset; // Apply rotation offset
            rb.rotation = angle;
            //Wenn followspeedoffset auf -.5f, dann +.002f pro frame bis 0 erreicht wird damit er sich nicht zu schnell bewegt
            if(followspeedoffset < 0){
                followspeedoffset += .002f;
            }
            X = 0;
            Y = 1.05f + followspeedoffset;
            nowfollowing = true;
            movenormal = false;
        }else if(distanceToPlayer < 5f + Mate_Extra_Distance && !EnemyContact && EntryPoint == null && !looting){
            followspeedoffset = -.5f;
            nowfollowing = false;
            movenormal = true;
            X = 0;
            Y = 0;
        }else nowfollowing = false;
        // else if (distanceToPlayer < 5f){
        //     Debug.Log("Reached close enough to Player");
        //     nowfollowing = false;
        //     movenormal = true;
        //     X = 0;
        //     Y = 0;
        // } 
        // else{
        //     Debug.Log("Maintaining position");
        //     nowfollowing = false;
        //     movenormal = true;
        //     X = 0;
        //     Y = 0;
        // }
    }

    private IEnumerator CheckforY(){
        if(Y == 0.95f && EnemyContact == false && !looting && !waiting1 && !inhouse && EntryPoint == null && !nowfollowing || Y == -0.95f && EnemyContact == false && !looting && !waiting1 && !inhouse && EntryPoint == null && !nowfollowing){ //Macht Mate crazy wenn er weg läuft
            waiting1 = true;
            yield return new WaitForSeconds(.2f);
            X = Random.Range(-1f,1f);
            Y = Random.Range(-1f,1f);
            waiting1 = false;
        }
    }

    private IEnumerator House_Orientation(){
        if(!looting && !EnemyContact && EntryPoint != null && Currloot == null){
            if(!EnemyContact && Currloot == null && !looting) movenormal = false;
            try{
                Vector2 lookDir = new Vector2(EntryPoint.transform.position.x, EntryPoint.transform.position.y) - rb.position; 
                angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg + 270f;
                rb.rotation = angle;
            }catch{}

            if(EntryPoint != null && !EnemyContact && !looting){
                X = 0f;
                Y = 0.95f;
            }
        }else if(!looting && !EnemyContact && EntryPoint == null && Currloot == null){
            if (!looting && Currloot == null && !EnemyContact) movenormal = true;
            X = 0;
            Y = 0;

            yield return new WaitForSeconds(.1f);
        }
    }

    private IEnumerator FootstepGen(){
        begin:
        while(steping){
            if(spawn_steps){
                Instantiate(Footsteps,new Vector3(transform.position.x, transform.position.y, 113f), transform.rotation);
                yield return new WaitForSeconds(.13f);
                if(!steping) break;
                Instantiate(Footsteps2, new Vector3(transform.position.x, transform.position.y,113f), transform.rotation);
            }
            yield return new WaitForSeconds(.13f);
        }
        if(!steping){
            yield return new WaitForSeconds(.13f);
            goto begin;
        }
    }

    public IEnumerator CheckforPlayerDistance(){
        while(true){
            Player_Distance = Vector2.Distance(new Vector2(Player.transform.position.x, Player.transform.position.y), new Vector2(this.gameObject.transform.position.x, this.gameObject.transform.position.y));
            if(Player_Distance < 200f){
                spawn_steps = true;
            }else spawn_steps = false;
            yield return new WaitForSeconds(.5f);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if(collision.gameObject.tag == "Zone"){
            X *= -1;
            Y *= -1;
        }
    }

    public void Weapon_Follow_Player(){
        Weapons.transform.position = transform.position;
        Weapons.transform.rotation = transform.rotation;
    }

    public IEnumerator Mate_Random_Move(){
        for(; ; Zyklus++){
            X =  Random.Range(-1f,1f);
            Y = Random.Range(-1f,1f);
            if(Zyklus == 10) BewegungsängerungsZeit = 15f;
            yield return new WaitForSeconds(BewegungsängerungsZeit);
        }
    }

    private IEnumerator Looting(){
        if(looting && Currloot != null && !EnemyContact){
            if(schleifeni == 0) schleifeni++;
            if(schleifeni == 1){
                yield return new WaitForSeconds(.4f);
            }

            if (looting && Currloot != null && !EnemyContact) movenormal = false;

            try{
                Vector2 lookDir = new Vector2(Currloot.transform.position.x, Currloot.transform.position.y) - rb.position;
                angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg + rotationoffset; // Apply rotation offset
                rb.rotation = angle;
            }catch{}

            X = 0;
            Y = 0.95f;
        }else if (looting && Currloot == null){
            yield return new WaitForSeconds(.4f);
            X = 0;
            Y = 0;
            looting = false;
        } 
    }
    
    public IEnumerator EnemyContactfunc(){
        //Bei Spieler sichutung Augen Kontakt halten
        if(EnemyContact == true && Enemy != null){
            if(Z == 1 && waiting == false){
                waiting = true;
                yield return new WaitForSeconds(1.5f);
                Z++;
                waiting = false;
            }

            Vector2 lookDir = new Vector2(Enemy.transform.position.x, Enemy.transform.position.y) - rb.position;
            angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg + rotationoffset + missaim;
            rb.rotation = angle;

            //Laufe weg wenn low hp 
            if(Enemy != null && mate_health.health <= 50 || mate_inv.lootcount == 0 || forceflee){
                runaway = true;
                yield return new WaitForSeconds(.4f);
                Y = 0.95f;
                X = 0f;
                rotationoffset = 90f;
                //stop shooting
                Shoot = false;
            }else{
                runaway = false;
                rotationoffset = 270f;
            }

            //Verfolge Gegner.
            if(Enemy != null && Vector2.Distance(transform.position,Enemy.transform.position) >= 10.5f && runaway == false){
                X = 0f;
                Y = 0.95f;
                schleifenx = 0;
                schleifenz = 0;
                Jiggle = false;
            }

            //Bewegung stoppen wenn Gegner nah genug ist.
            if(Enemy != null && Vector2.Distance(transform.position,Enemy.transform.position) <= 9.5f && runaway == false){
                //Jiggle Movement:
                Jiggle = true;
                if(schleifenx <= 1) schleifenx++;
                if(schleifenx == 1){
                while(schleifenx < 300000 && Jiggle == true){
                    X = Random.Range(-1f,1f);
                    Y = Random.Range(-0.15f,0.1f);
                    yield return new WaitForSeconds(Random.Range(1f,2f));
                }
                }
            }
            //identisch zu dem if davor
            if(Enemy != null && Vector2.Distance(transform.position,Enemy.transform.position) <= 9.5f && runaway == false){
                if(schleifenz <= 1) schleifenz++;
                if(schleifenz == 1){
                    while(Jiggle == true){
                    //Mate Rotation modifizieren damit er schlechter aimt(Alle paar sekunden).
                    float randomizer = Random.Range(-12f,12f);
                    //Round up to no decimal
                    randomizer = Mathf.Round(randomizer);
                    while(missaim != randomizer){
                        if(missaim < randomizer){
                            missaim += 1f;
                        }else if(missaim > randomizer){
                            missaim -= 1f;
                        }
                        yield return new WaitForSecondsRealtime(.1f);
                    }
                    yield return new WaitForSecondsRealtime(2.25f);
                    }
                }
            }

            if(EnemyContact && Enemy != null) movenormal = false;
            //Random Waffe raus hohlen und dann schießen.
            if(!runaway){ //Nur wenn er nicht wegläuft schiessen sonst schießt er ins nichts
            StartCoroutine(Mate_shoot.Shoot()); //Schießen
            if(schleifeny <= 1 && inv.lootcount > 0) schleifeny++;
            if(schleifeny == 1){
                inv.DecidenextWeapon();
                //Animator soll den Mate in Waffen haltung versetz
                anim.SetBool("Weaponactive", true);
            }
            }
        }else{
            if(!nowfollowing && !looting && EntryPoint == null){
            //Sonst immer in die lauf richtung schauen.
            if(X != 0 || Y != 0){
            transform.up = new Vector2(X,Y);
            }
            Jiggle = false;
            if(!nowfollowing)movenormal = true;
            schleifenx = 0;
            schleifeny = 0;
            schleifenz = 0;
            Shoot = false;
            runaway = false;
            missaim = 0;
            Enemy = null;
        }
        }
    }
    
}
