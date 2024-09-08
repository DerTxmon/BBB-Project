using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Shoot : MonoBehaviour
{
    public Inventory_Handler Inv;
    [SerializeField] private GameObject Player;
    public GameObject Bullet;
    public GameObject Feuerpunkt; 
    public bool isshooting, ishitting;
    public bool shootbttn = false;
    public bool shootbttn2;
    public bool shootbttnpressed;
    public bool reloadbttn = false;
    public bool inreload, autoshootbttn = false;
    [SerializeField] private Sprite Greenbutton, Redbutton;
    [SerializeField] private GameObject Impactanimation;
    [SerializeField] private LineRenderer[] lineRenderer;
    [SerializeField] private LineRenderer[] lineRendererDark;
    public LayerMask layerMask;
    private Color impactobjectcolor;
    public static int Damage_dealt;
    public Animator animator;
    public RectTransform KillfeedPos;
    public GameObject KillfeedEntry;
    //Killfeed Sprites
    public Sprite Killfeed_Glock_18;
    public Sprite Killfeed_M4;
    public Sprite Killfeed_AK_47;
    public Sprite Killfeed_Sniper;
    public Sprite Killfeed_Mp7;
    public Sprite Killfeed_Zone;
    public Sprite Killfeed_Faust;
    public Sprite Killfeed_Shotgun;
    TextMeshProUGUI MörderText, OpferText;
    Image Background, GrundImage;
    GameObject NewKillfeedEntry;
    UI_Handler uihandler;
    public AudioSource ShootingAudio;
    public AudioClip Glock_18_Shot, M4_Shot, Ak47_Shot, Sniper_Shot, Mp7_Shot, Shotgun_Shot;
    public AudioClip Glock_18_Reload, M4_Reload, Ak47_Reload, Sniper_Reload, Mp7_Reload, Shotgun_Reload;
    
    void Awake(){
        uihandler = Player.GetComponent<UI_Handler>();
    }
    void Start()
    {
        Inv = Player.GetComponent<Inventory_Handler>();
        Damage_dealt = 0;
    }

    // Update is called once per frame
    void Update(){
        if(shootbttn2 || !shootbttn2 && shootbttnpressed || autoshootbttn) shootbttn = true;
        else shootbttn = false;

        if(shootbttn == true && !isshooting && !inreload) StartCoroutine(shoot()); //Wird nur ausgeführt wenn spieler den letzten schuss zuende geführt hat
        if(reloadbttn == true && !isshooting && !inreload) StartCoroutine(reload());
    }
    private IEnumerator Hit(){
        if(!isshooting && !ishitting){
            ishitting = true;
            //Play Animation
            animator.SetBool("ishitting", true);
            //Raycast schuss
            RaycastHit2D hitinfo = Physics2D.Raycast(transform.position, transform.up, 1.5f, layerMask); //Glock schießt 50f weit
            if(hitinfo){
                if(hitinfo.collider.gameObject.tag == "Player"){
                    hitinfo.collider.GetComponent<Player_Health>().Damage(17);
                    Damage_dealt += 17;
                }else if(hitinfo.collider.gameObject.tag == "Bot"){
                    if(hitinfo.collider.GetComponent<Bot_Health>() != null){
                        if(hitinfo.collider.GetComponent<Bot_Health>().health <= 17) gameObject.GetComponent<Inventory_Handler>().Kills++;
                        hitinfo.collider.GetComponent<Bot_Health>().Damage(17, "Player", 7);
                    }if(hitinfo.collider.GetComponent<Mate_Health>() != null){
                        if(hitinfo.collider.GetComponent<Mate_Health>().health <= 17) gameObject.GetComponent<Inventory_Handler>().Kills++;
                        hitinfo.collider.GetComponent<Mate_Health>().Damage(17, "Player", 7);
                    }
                    Damage_dealt += 17;
                }
                
                HitAnimation(hitinfo);
            }
            //Wait
            yield return new WaitForSeconds(0.5f);
            animator.SetBool("ishitting", false);
            ishitting = false;
        }
    }

    private IEnumerator shoot(){
        if(Inv.Slot1_Selected == true){
        if(Inv.Slot1_Item == "" || Inv.Slot1_Item == null && !ishitting && !isshooting) StartCoroutine(Hit()); //Schlagen
        //Glock-18
        if(Inv.Glock_18_Selected == true && isshooting == false && Inv.slot1_mag_ammo > 0){
            ShootingAudio.clip = Glock_18_Shot;
            ShootingAudio.Play();
            isshooting = true;
            //Raycast schuss
            RaycastHit2D hitinfo = Physics2D.Raycast(Feuerpunkt.transform.position, Feuerpunkt.transform.up, 50f, layerMask); //Glock schießt 50f weit
            if(hitinfo){
                if(hitinfo.collider.gameObject.tag == "Player"){
                    hitinfo.collider.GetComponent<Player_Health>().Damage(15);
                    Damage_dealt += 15;
                }else if(hitinfo.collider.gameObject.tag == "Bot"){
                    if(hitinfo.collider.GetComponent<Bot_Health>().health <= 15){
                        gameObject.GetComponent<Inventory_Handler>().Kills++;
                        Killfeed(Menu_Handler.loadeddata.Saved_Player_Name, hitinfo.collider.GetComponent<TextMeshProUGUI>().text, 1); //Player name + Bot name + Weapon ID
                    } 
                    hitinfo.collider.GetComponent<Bot_Health>().Damage(15, "Player", 1);
                    Damage_dealt += 15;
                }
                
                HitAnimation(hitinfo);
                //Gray Line
                lineRenderer[1].SetPosition(0, new Vector3(Feuerpunkt.transform.position.x, Feuerpunkt.transform.position.y, 120f));
                lineRenderer[1].SetPosition(1, new Vector3(hitinfo.point.x, hitinfo.point.y, 120f));
                //Dark Line
                lineRendererDark[1].SetPosition(0, new Vector3(Feuerpunkt.transform.position.x, Feuerpunkt.transform.position.y, 120.01f));
                lineRendererDark[1].SetPosition(1, new Vector3(hitinfo.point.x, hitinfo.point.y, 120.01f));
            }else{
                lineRenderer[1].SetPosition(0, Feuerpunkt.transform.position);
                lineRenderer[1].SetPosition(1, Feuerpunkt.transform.position + Feuerpunkt.transform.up * 50f);
                lineRenderer[1].SetPosition(1, new Vector3(lineRenderer[1].GetPosition(1).x, lineRenderer[1].GetPosition(1).y, 120f)); //Set Z
                //Dark Line
                lineRendererDark[1].SetPosition(0, Feuerpunkt.transform.position);
                lineRendererDark[1].SetPosition(1, Feuerpunkt.transform.position + Feuerpunkt.transform.up * 50f);
                lineRendererDark[1].SetPosition(1, new Vector3(lineRendererDark[1].GetPosition(1).x, lineRendererDark[1].GetPosition(1).y, 120.01f)); //Set Z
            }
            lineRenderer[1].enabled = true;
            lineRendererDark[1].enabled = true;
            // StartCoroutine(FadeOutTracers(lineRenderer[1], lineRendererDark[1]));
            //Wait one frame
            yield return new WaitForSeconds(0.05f);
            lineRenderer[1].enabled = false;
            lineRendererDark[1].enabled = false;
            // ResetTracers(lineRenderer[1], lineRendererDark[1]);
            Inv.slot1_mag_ammo--;
            yield return new WaitForSeconds(1f); 
            isshooting = false;
        }
        //Reload Glock-18
        if(Inv.slot1_mag_ammo == 0 && !inreload && Inv.small_ammo > 0 && Inv.Glock_18_Selected == true && isshooting == false){
            inreload = true;
            //Animation 
            animator.SetFloat("ReloadSpeed", 1.3f); //Animation Dauert genau 2 Sekunden
            animator.SetBool("inreload", true);
            //Reload
            Inv.small_ammo -= 12;
            if(Inv.small_ammo < 0){
                Inv.small_ammo += 12;
                Inv.slot1_mag_ammo = Inv.small_ammo;
                Inv.small_ammo = 0;
                yield return new WaitForSeconds (2f);
                goto reloadend;
            }
            yield return new WaitForSeconds (2f);
            Inv.slot1_mag_ammo += 12;
            reloadend:
            animator.SetBool("inreload", false);
            inreload = false;
        }
        
        //Für Automatische Gewähre. (M4)
        if(Inv.M4_Selected == true && isshooting == false && Inv.slot1_mag_ammo > 0){
            isshooting = true;
            for( ; Inv.slot1_mag_ammo != 0 ; Inv.slot1_mag_ammo--){
                if(shootbttn == false) goto ende;
                //Raycast schuss
                RaycastHit2D hitinfo = Physics2D.Raycast(Feuerpunkt.transform.position, Feuerpunkt.transform.up, 50f, layerMask); //Glock schießt 50f weit
                if(hitinfo){
                    if(hitinfo.collider.gameObject.tag == "Player"){
                        hitinfo.collider.GetComponent<Player_Health>().Damage(30);
                        Damage_dealt += 30;
                    }else if(hitinfo.collider.gameObject.tag == "Bot"){
                        if(hitinfo.collider.GetComponent<Bot_Health>().health <= 30) gameObject.GetComponent<Inventory_Handler>().Kills++;
                        hitinfo.collider.GetComponent<Bot_Health>().Damage(30, "Player", 2);
                        Damage_dealt += 30;
                    }
                
                    HitAnimation(hitinfo);
                    //Gray Line
                    lineRenderer[1].SetPosition(0, new Vector3(Feuerpunkt.transform.position.x, Feuerpunkt.transform.position.y, 120f));
                    lineRenderer[1].SetPosition(1, new Vector3(hitinfo.point.x, hitinfo.point.y, 120f));
                    //Dark Line
                    lineRendererDark[1].SetPosition(0, new Vector3(Feuerpunkt.transform.position.x, Feuerpunkt.transform.position.y, 120.01f));
                    lineRendererDark[1].SetPosition(1, new Vector3(hitinfo.point.x, hitinfo.point.y, 120.01f));
                }else{
                    //Gray Line
                    lineRenderer[1].SetPosition(0, Feuerpunkt.transform.position);
                    lineRenderer[1].SetPosition(1, Feuerpunkt.transform.position + Feuerpunkt.transform.up * 50f);
                    lineRenderer[1].SetPosition(1, new Vector3(lineRenderer[1].GetPosition(1).x, lineRenderer[1].GetPosition(1).y, 120f));
                    //Dark Line
                    lineRendererDark[1].SetPosition(0, Feuerpunkt.transform.position);
                    lineRendererDark[1].SetPosition(1, Feuerpunkt.transform.position + Feuerpunkt.transform.up * 50f);
                    lineRendererDark[1].SetPosition(1, new Vector3(lineRendererDark[1].GetPosition(1).x, lineRendererDark[1].GetPosition(1).y, 120.01f));
                }
                lineRenderer[1].enabled = true;
                lineRendererDark[1].enabled = true;
                // StartCoroutine(FadeOutTracers(lineRenderer[1], lineRendererDark[1]));
                //Wait one frame
                yield return new WaitForSeconds(0.05f);
                lineRenderer[1].enabled = false;
                lineRendererDark[1].enabled = false;
                // ResetTracers(lineRenderer[1], lineRendererDark[1]);
                yield return new WaitForSecondsRealtime(0.3f);
            }
            ende:
            isshooting = false;
        }
        //Reload M4
        if(Inv.slot1_mag_ammo == 0 && !inreload && Inv.mid_ammo > 0 && Inv.M4_Selected == true && isshooting == false){
            inreload = true;
            //Animation 
            animator.SetFloat("ReloadSpeed", .9f); //Animation Dauert genau 3.27 Sekunden
            animator.SetBool("inreload", true);
            //Reload
            Inv.mid_ammo -= 25;
            if(Inv.mid_ammo < 0){
                Inv.mid_ammo += 25;
                Inv.slot1_mag_ammo = Inv.mid_ammo; // RELOAD ZU SCHNELL
                Inv.mid_ammo = 0;
                yield return new WaitForSeconds(3.27f);
                goto reloadend;
            }
            yield return new WaitForSeconds(3.27f);
            Inv.slot1_mag_ammo += 25;
            reloadend:
            animator.SetBool("inreload", false);
            inreload = false;
        }

        //Für Automatische Gewähre. (Ak47)
        if(Inv.Ak47_Selected == true && isshooting == false && Inv.slot1_mag_ammo > 0){
            isshooting = true;
            for( ; Inv.slot1_mag_ammo != 0 ; Inv.slot1_mag_ammo--){
                if(shootbttn == false) goto ende;
                //Raycast schuss
                RaycastHit2D hitinfo = Physics2D.Raycast(Feuerpunkt.transform.position, Feuerpunkt.transform.up, 50f, layerMask); //Glock schießt 50f weit
                if(hitinfo){
                    if(hitinfo.collider.gameObject.tag == "Player"){
                        hitinfo.collider.GetComponent<Player_Health>().Damage(35);
                        Damage_dealt += 35;
                    }else if(hitinfo.collider.gameObject.tag == "Bot"){
                        if(hitinfo.collider.GetComponent<Bot_Health>().health <= 35) gameObject.GetComponent<Inventory_Handler>().Kills++;
                        hitinfo.collider.GetComponent<Bot_Health>().Damage(35, "Player", 3);
                        Damage_dealt += 35;
                    }
                
                    HitAnimation(hitinfo);
                    //Gray Line
                    lineRenderer[1].SetPosition(0, new Vector3(Feuerpunkt.transform.position.x, Feuerpunkt.transform.position.y, 120f));
                    lineRenderer[1].SetPosition(1, new Vector3(hitinfo.point.x, hitinfo.point.y, 120f));
                    //Dark Line
                    lineRendererDark[1].SetPosition(0, new Vector3(Feuerpunkt.transform.position.x, Feuerpunkt.transform.position.y, 120.01f));
                    lineRendererDark[1].SetPosition(1, new Vector3(hitinfo.point.x, hitinfo.point.y, 120.01f));
                }else{
                    lineRenderer[1].SetPosition(0, Feuerpunkt.transform.position);
                    lineRenderer[1].SetPosition(1, Feuerpunkt.transform.position + Feuerpunkt.transform.up * 50f);
                    lineRenderer[1].SetPosition(1, new Vector3(lineRenderer[1].GetPosition(1).x, lineRenderer[1].GetPosition(1).y, 120f));
                    //Dark Line
                    lineRendererDark[1].SetPosition(0, Feuerpunkt.transform.position);
                    lineRendererDark[1].SetPosition(1, Feuerpunkt.transform.position + Feuerpunkt.transform.up * 50f);
                    lineRendererDark[1].SetPosition(1, new Vector3(lineRendererDark[1].GetPosition(1).x, lineRendererDark[1].GetPosition(1).y, 120.01f));
                }
                lineRenderer[1].enabled = true;
                lineRendererDark[1].enabled = true;
                // StartCoroutine(FadeOutTracers(lineRenderer[1], lineRendererDark[1]));
                //Wait one frame
                yield return new WaitForSeconds(0.05f);
                lineRenderer[1].enabled = false;
                lineRendererDark[1].enabled = false;
                // ResetTracers(lineRenderer[1], lineRendererDark[1]);
                yield return new WaitForSecondsRealtime(0.3f);
            }
            ende:
            isshooting = false;
        }
        //Reload Ak47
        if(Inv.slot1_mag_ammo == 0 && !inreload && Inv.mid_ammo > 0 && Inv.Ak47_Selected == true && isshooting == false){
            inreload = true;
            //Animation 
            animator.SetFloat("ReloadSpeed", .9f); //Animation Dauert genau 3.27 Sekunden
            animator.SetBool("inreload", true);
            //Reload
            Inv.mid_ammo -= 25;
            if(Inv.mid_ammo < 0){
                Inv.mid_ammo += 25;
                Inv.slot1_mag_ammo = Inv.mid_ammo;
                Inv.mid_ammo = 0;
                yield return new WaitForSeconds (3.27f);
                goto reloadend;
            }
            yield return new WaitForSeconds (3.27f);
            Inv.slot1_mag_ammo += 25;
            reloadend:
            animator.SetBool("inreload", false);
            inreload = false;
        }

        //Sniper
        if(Inv.Sniper_Selected == true && isshooting == false && Inv.slot1_mag_ammo > 0){
            isshooting = true;
            ShootingAudio.clip = Sniper_Shot;
            ShootingAudio.Play();
            //Raycast schuss
            RaycastHit2D hitinfo = Physics2D.Raycast(Feuerpunkt.transform.position, Feuerpunkt.transform.up, 150f); //Glock schießt 50f weit
            if(hitinfo){
                if(hitinfo.collider.gameObject.tag == "Player"){
                        hitinfo.collider.GetComponent<Player_Health>().Damage(150);
                        Damage_dealt += 150;
                    }else if(hitinfo.collider.gameObject.tag == "Bot"){
                        if(hitinfo.collider.GetComponent<Bot_Health>().health <= 150) gameObject.GetComponent<Inventory_Handler>().Kills++;
                        hitinfo.collider.GetComponent<Bot_Health>().Damage(150, "Player", 4);
                        Damage_dealt += 150;
                    }
                
                HitAnimation(hitinfo);
                lineRenderer[1].SetPosition(0, new Vector3(Feuerpunkt.transform.position.x, Feuerpunkt.transform.position.y, 120f));
                lineRenderer[1].SetPosition(1, new Vector3(hitinfo.point.x, hitinfo.point.y, 120f));
                //Dark Line
                lineRendererDark[1].SetPosition(0, new Vector3(Feuerpunkt.transform.position.x, Feuerpunkt.transform.position.y, 120.01f));
                lineRendererDark[1].SetPosition(1, new Vector3(hitinfo.point.x, hitinfo.point.y, 120.01f));
            }else{
                lineRenderer[1].SetPosition(0, Feuerpunkt.transform.position);
                lineRenderer[1].SetPosition(1, Feuerpunkt.transform.position + Feuerpunkt.transform.up * 150f);
                lineRenderer[1].SetPosition(1, new Vector3(lineRenderer[1].GetPosition(1).x, lineRenderer[1].GetPosition(1).y, 120f));
                //Dark Line
                lineRendererDark[1].SetPosition(0, Feuerpunkt.transform.position);
                lineRendererDark[1].SetPosition(1, Feuerpunkt.transform.position + Feuerpunkt.transform.up * 150f);
                lineRendererDark[1].SetPosition(1, new Vector3(lineRendererDark[1].GetPosition(1).x, lineRendererDark[1].GetPosition(1).y, 120.01f));
            }
            lineRenderer[1].enabled = true;
            lineRendererDark[1].enabled = true;
            // StartCoroutine(FadeOutTracers(lineRenderer[1], lineRendererDark[1]));
            //Wait one frame
            yield return new WaitForSeconds(0.2f);
            lineRenderer[1].enabled = false;
            lineRendererDark[1].enabled = false;
            // ResetTracers(lineRenderer[1], lineRendererDark[1]);
            Inv.slot1_mag_ammo--;
            yield return new WaitForSeconds(4f); 
            isshooting = false;
        }

        //Reload Sniper
        if(Inv.slot1_mag_ammo == 0 && !inreload && Inv.big_ammo > 0 && Inv.Sniper_Selected == true && isshooting == false){
            inreload = true;
            //Animation 
            animator.SetFloat("ReloadSpeed", .5f); //Animation Dauert ~ 5 Sekunden
            animator.SetBool("inreload", true);
            //Reload
            Inv.big_ammo -= 5;
            if(Inv.big_ammo < 0){
                Inv.big_ammo += 5;
                Inv.slot1_mag_ammo = Inv.big_ammo;
                Inv.big_ammo = 0;
                yield return new WaitForSeconds (6f);
                goto reloadend;
            }
            Inv.slot1_mag_ammo += 5;
            yield return new WaitForSeconds (6f);
            reloadend:
            animator.SetBool("inreload", false);
            inreload = false;
        }
        //Mp7
        if(Inv.Mp7_Selected && Inv.slot1_mag_ammo > 0 && !isshooting){
            isshooting = true;
            for( ; Inv.slot1_mag_ammo != 0 ; Inv.slot1_mag_ammo--){
                if(shootbttn == false) goto ende;
                //Raycast schuss
                RaycastHit2D hitinfo = Physics2D.Raycast(Feuerpunkt.transform.position, Feuerpunkt.transform.up, 50f, layerMask); //Glock schießt 50f weit
                if(hitinfo){
                    if(hitinfo.collider.gameObject.tag == "Player"){
                        hitinfo.collider.GetComponent<Player_Health>().Damage(15);
                        Damage_dealt += 15;
                    }else if(hitinfo.collider.gameObject.tag == "Bot"){
                        if(hitinfo.collider.GetComponent<Bot_Health>().health <= 15) gameObject.GetComponent<Inventory_Handler>().Kills++;
                        hitinfo.collider.GetComponent<Bot_Health>().Damage(15, "Player", 5);
                        Damage_dealt += 15;
                    }
                
                    //Einschuss Animation (Blut)
                    if(hitinfo.collider.gameObject.tag == "Player" || hitinfo.collider.gameObject.tag == "Bot"){
                    Color impactobjectcolor = Color.red;
                    GameObject animation = Instantiate(Impactanimation, hitinfo.point, Quaternion.identity);
                    ParticleSystem.MainModule main = animation.GetComponent<ParticleSystem>().main;
                    main.startColor = impactobjectcolor;
                }else{ //Die Farbe des getroffenem Object wird genommen und eine alternative Inpact Animation wird mit der farbe des objects abgespielt 
                    int randint = Random.Range(1,3);//Es wird Random eine Farbe zwischen Grau und Braun gewählt und als Animations Farbe genutzt
                    if(randint == 1)impactobjectcolor = Color.gray; //Graue Farbe
                    else ColorUtility.TryParseHtmlString("#985000", out impactobjectcolor); //Braune Farbe
                    GameObject animation = Instantiate(Impactanimation, hitinfo.point, Quaternion.identity);
                    ParticleSystem.MainModule main = animation.GetComponent<ParticleSystem>().main;
                    main.startColor = impactobjectcolor;
                }
                    lineRenderer[1].SetPosition(0, new Vector3(Feuerpunkt.transform.position.x, Feuerpunkt.transform.position.y, 120f));
                    lineRenderer[1].SetPosition(1, new Vector3(hitinfo.point.x, hitinfo.point.y, 120f));
                    //Dark Line
                    lineRendererDark[1].SetPosition(0, new Vector3(Feuerpunkt.transform.position.x, Feuerpunkt.transform.position.y, 120.01f));
                    lineRendererDark[1].SetPosition(1, new Vector3(hitinfo.point.x, hitinfo.point.y, 120.01f));
                }else{
                    lineRenderer[1].SetPosition(0, Feuerpunkt.transform.position);
                    lineRenderer[1].SetPosition(1, Feuerpunkt.transform.position + Feuerpunkt.transform.up * 50f);
                    lineRenderer[1].SetPosition(1, new Vector3(lineRenderer[1].GetPosition(1).x, lineRenderer[1].GetPosition(1).y, 120f));
                    //Dark Line
                    lineRendererDark[1].SetPosition(0, Feuerpunkt.transform.position);
                    lineRendererDark[1].SetPosition(1, Feuerpunkt.transform.position + Feuerpunkt.transform.up * 50f);
                    lineRendererDark[1].SetPosition(1, new Vector3(lineRendererDark[1].GetPosition(1).x, lineRendererDark[1].GetPosition(1).y, 120.01f));
                }
                lineRenderer[1].enabled = true;
                lineRendererDark[1].enabled = true;
                // StartCoroutine(FadeOutTracers(lineRenderer[1], lineRendererDark[1]));
                //Wait one frame
                yield return new WaitForSeconds(0.05f);
                lineRenderer[1].enabled = false;
                lineRendererDark[1].enabled = false;
                // ResetTracers(lineRenderer[1], lineRendererDark[1]);
                yield return new WaitForSecondsRealtime(0.09f);
            }
            ende:
            isshooting = false;
        }
        //Reload Mp7
        if(Inv.slot1_mag_ammo == 0 && !inreload && Inv.small_ammo > 0 && Inv.Mp7_Selected && !isshooting){
            inreload = true;
            //Animation 
            animator.SetFloat("ReloadSpeed", 1f);
            animator.SetBool("inreload", true);
            //Reload
            Inv.small_ammo -= 20;
            if(Inv.small_ammo < 0){
                Inv.small_ammo += 20;
                Inv.slot1_mag_ammo = Inv.small_ammo;
                Inv.small_ammo = 0;
                yield return new WaitForSeconds(2.5f);
                goto reloadend;
            }
            yield return new WaitForSeconds(2.5f);
            Inv.slot1_mag_ammo += 20;
            reloadend:
            animator.SetBool("inreload", false);
            inreload = false;
        }
        //Shotgun
        if(Inv.Shotgun_Selected == true && isshooting == false && Inv.slot1_mag_ammo > 0){
            isshooting = true;
            ShootingAudio.clip = Shotgun_Shot;
            ShootingAudio.Play();
            //new
            //Raycast schüsse (5 Stück)
            RaycastHit2D[] Shotgunhits = new RaycastHit2D[10];
            Vector2[] Directions = new Vector2[10];
            for(int i = 0; i < 10; i++){
                //Berechne Random Winkel
                float raycastAngle = Random.Range(0f, 360f); //Desired angle offset in degrees from the initial direction
                //Check ob auch im Richtigen Bereich
                if(!(raycastAngle >= 0f && raycastAngle <= 25f || raycastAngle >= 335f && raycastAngle <= 360f)){ //reroll until it is in the right range
                    while(!(raycastAngle >= 0f && raycastAngle <= 25f || raycastAngle >= 335f && raycastAngle <= 360f)){
                        raycastAngle = Random.Range(0f, 360f);
                    }
                }
                // Cast the raycast from a position with the initial direction
                Vector2 raycastOrigin = Feuerpunkt.transform.position;
                Vector2 raycastDirection = Feuerpunkt.transform.up; // Initial direction of the ray
                // Apply the angle offset to the raycast direction
                Quaternion rotation = Quaternion.Euler(raycastAngle, raycastAngle, raycastAngle);
                raycastDirection = rotation * raycastDirection;
                Directions[i] = raycastDirection;
                Shotgunhits[i] = Physics2D.Raycast(raycastOrigin, raycastDirection, 10f, layerMask);
            }
            //Damage
            foreach(RaycastHit2D hitinfoshotgun in Shotgunhits){
                if(hitinfoshotgun){
                    if(hitinfoshotgun.collider.gameObject.tag == "Bot"){
                        if(hitinfoshotgun.collider.GetComponent<Bot_Health>().health <= 9){
                            gameObject.GetComponent<Inventory_Handler>().Kills++;
                            Killfeed(Menu_Handler.loadeddata.Saved_Player_Name, hitinfoshotgun.collider.GetComponentInChildren<TextMeshPro>().text, 8); //Player name + Bot name + Weapon ID
                        } 
                        hitinfoshotgun.collider.GetComponent<Bot_Health>().Damage(9, "Player", 8);
                        Damage_dealt += 5;
                    }
                    HitAnimation(hitinfoshotgun);
                }
            }
            //Visuelle Darstellung
            int x = 0;
            foreach(RaycastHit2D hitinfoshotgun in Shotgunhits){
                if(hitinfoshotgun){
                    lineRenderer[x].SetPosition(0, new Vector3(Feuerpunkt.transform.position.x, Feuerpunkt.transform.position.y, 120f));
                    lineRenderer[x].SetPosition(1, new Vector3(hitinfoshotgun.point.x, hitinfoshotgun.point.y, 120f));
                    //Dark Line
                    lineRendererDark[x].SetPosition(0, new Vector3(Feuerpunkt.transform.position.x, Feuerpunkt.transform.position.y, 120.01f));
                    lineRendererDark[x].SetPosition(1, new Vector3(hitinfoshotgun.point.x, hitinfoshotgun.point.y, 120.01f));
                }else{
                    lineRenderer[x].SetPosition(0, Feuerpunkt.transform.position);
                    lineRenderer[x].SetPosition(1, new Vector2(Feuerpunkt.transform.position.x, Feuerpunkt.transform.position.y) + Directions[x] * 10f);
                    lineRenderer[x].SetPosition(1, new Vector3(lineRenderer[x].GetPosition(1).x, lineRenderer[x].GetPosition(1).y, 120f));
                    //Dark Line
                    lineRendererDark[x].SetPosition(0, Feuerpunkt.transform.position);
                    lineRendererDark[x].SetPosition(1, new Vector2(Feuerpunkt.transform.position.x, Feuerpunkt.transform.position.y) + Directions[x] * 10f);
                    lineRendererDark[x].SetPosition(1, new Vector3(lineRendererDark[x].GetPosition(1).x, lineRendererDark[x].GetPosition(1).y, 120.01f));
                }
                x++;
            }
            //Alle einmal anzeigen
            for(int i = 0; i < 10; i++){
                lineRenderer[i].enabled = true;
                lineRendererDark[i].enabled = true;
                // StartCoroutine(FadeOutTracers(lineRenderer[i], lineRendererDark[i]));
            }

            //Wait one frame
            yield return new WaitForSeconds(0.05f);
            
            //Alle wieder ausblenden
            for(int i = 0; i < 10; i++){
                lineRenderer[i].enabled = false;
                lineRendererDark[i].enabled = false;
                // ResetTracers(lineRenderer[1], lineRendererDark[1]);
            }
            Inv.slot1_mag_ammo--;
            yield return new WaitForSeconds(1f); 
            isshooting = false;
        }
        //Reload Sotgun
        if(Inv.slot1_mag_ammo == 0 && !inreload && Inv.shotgun_ammo > 0 && Inv.Shotgun_Selected == true && isshooting == false){
            inreload = true;
            //Animation 
            animator.SetBool("inreloadShotgun", true);
            //Reload
            while(Inv.Shotgun_Selected && Inv.slot1_mag_ammo < 5 && Inv.shotgun_ammo > 0 && Inv.Slot1_Selected){
                yield return new WaitForSeconds(1.2f);
                Inv.shotgun_ammo--;
                Inv.slot1_mag_ammo++;
            }
            animator.SetBool("inreloadShotgun", false);
            inreload = false;
        }
    }

    if(Inv.Slot2_Selected == true){
        if(Inv.Slot2_Item == "" || Inv.Slot2_Item == null && !ishitting && !isshooting) StartCoroutine(Hit());
         //Glock-18
        if(Inv.Glock_18_Selected == true && isshooting == false && Inv.slot2_mag_ammo > 0){
            ShootingAudio.clip = Glock_18_Shot;
            ShootingAudio.Play();
            isshooting = true;
            //Raycast schuss
            RaycastHit2D hitinfo = Physics2D.Raycast(Feuerpunkt.transform.position, Feuerpunkt.transform.up, 50f, layerMask); //Glock schießt 50f weit
            if(hitinfo){
                if(hitinfo.collider.gameObject.tag == "Player"){
                    hitinfo.collider.GetComponent<Player_Health>().Damage(15);
                    Damage_dealt += 15;
                }else if(hitinfo.collider.gameObject.tag == "Bot"){
                    if(hitinfo.collider.GetComponent<Bot_Health>().health <= 15) gameObject.GetComponent<Inventory_Handler>().Kills++;
                    hitinfo.collider.GetComponent<Bot_Health>().Damage(15, "Player", 1);
                }
                
                HitAnimation(hitinfo);
                lineRenderer[1].SetPosition(0, new Vector3(Feuerpunkt.transform.position.x, Feuerpunkt.transform.position.y, 120f));
                lineRenderer[1].SetPosition(1, new Vector3(hitinfo.point.x, hitinfo.point.y, 120f));
                //Dark Line
                lineRendererDark[1].SetPosition(0, new Vector3(Feuerpunkt.transform.position.x, Feuerpunkt.transform.position.y, 120.01f));
                lineRendererDark[1].SetPosition(1, new Vector3(hitinfo.point.x, hitinfo.point.y, 120.01f));
            }else{
                lineRenderer[1].SetPosition(0, Feuerpunkt.transform.position);
                lineRenderer[1].SetPosition(1, Feuerpunkt.transform.position + Feuerpunkt.transform.up * 50f);
                lineRenderer[1].SetPosition(1, new Vector3(lineRenderer[1].GetPosition(1).x, lineRenderer[1].GetPosition(1).y, 120f));
                //Dark Line
                lineRendererDark[1].SetPosition(0, Feuerpunkt.transform.position);
                lineRendererDark[1].SetPosition(1, Feuerpunkt.transform.position + Feuerpunkt.transform.up * 50f);
                lineRendererDark[1].SetPosition(1, new Vector3(lineRendererDark[1].GetPosition(1).x, lineRendererDark[1].GetPosition(1).y, 120.01f));
            }
            lineRenderer[1].enabled = true;
            lineRendererDark[1].enabled = true;
            // StartCoroutine(FadeOutTracers(lineRenderer[1], lineRendererDark[1]));
            //Wait one frame
            yield return new WaitForSeconds(0.05f);
            lineRenderer[1].enabled = false;
            lineRendererDark[1].enabled = false;
            // ResetTracers(lineRenderer[1], lineRendererDark[1]);
            Inv.slot2_mag_ammo--;
            yield return new WaitForSeconds(1f); 
            isshooting = false;
        }
        //Reload Glock-18
        if(Inv.slot2_mag_ammo == 0 && inreload == false && Inv.small_ammo > 0 && Inv.Glock_18_Selected == true && isshooting == false){
            inreload = true;
            //Animation 
            animator.SetFloat("ReloadSpeed", 1.3f); //Animation Dauert genau 2 Sekunden
            animator.SetBool("inreload", true);
            //Reload
            Inv.small_ammo -= 12;
            if(Inv.small_ammo < 0){
                Inv.small_ammo += 12;
                Inv.slot2_mag_ammo = Inv.small_ammo;
                Inv.small_ammo = 0;
                yield return new WaitForSeconds (2f);
                goto reloadend;
            }
            yield return new WaitForSeconds (2f);
            Inv.slot2_mag_ammo += 12;
            reloadend:
            inreload = false;
            animator.SetBool("inreload", false);
        }
        
        //Für Automatische Gewähre. (M4)
        if(Inv.M4_Selected == true && isshooting == false && Inv.slot2_mag_ammo > 0){
            isshooting = true;
            for( ; Inv.slot2_mag_ammo != 0 ; Inv.slot2_mag_ammo--){
                if(shootbttn == false) goto ende;
                //Raycast schuss
                RaycastHit2D hitinfo = Physics2D.Raycast(Feuerpunkt.transform.position, Feuerpunkt.transform.up, 50f, layerMask); //Glock schießt 50f weit
                if(hitinfo){
                    if(hitinfo.collider.gameObject.tag == "Player"){
                        hitinfo.collider.GetComponent<Player_Health>().Damage(30);
                        Damage_dealt += 30;
                    }else if(hitinfo.collider.gameObject.tag == "Bot"){
                        if(hitinfo.collider.GetComponent<Bot_Health>().health <= 30) gameObject.GetComponent<Inventory_Handler>().Kills++;
                        hitinfo.collider.GetComponent<Bot_Health>().Damage(30, "Player", 2);
                        Damage_dealt += 30;
                    }
                
                    HitAnimation(hitinfo);
                    lineRenderer[1].SetPosition(0, new Vector3(Feuerpunkt.transform.position.x, Feuerpunkt.transform.position.y, 120f));
                    lineRenderer[1].SetPosition(1, new Vector3(hitinfo.point.x, hitinfo.point.y, 120f));
                    //Dark Line
                    lineRendererDark[1].SetPosition(0, new Vector3(Feuerpunkt.transform.position.x, Feuerpunkt.transform.position.y, 120.01f));
                    lineRendererDark[1].SetPosition(1, new Vector3(hitinfo.point.x, hitinfo.point.y, 120.01f));
                }else{
                    lineRenderer[1].SetPosition(0, Feuerpunkt.transform.position);
                    lineRenderer[1].SetPosition(1, Feuerpunkt.transform.position + Feuerpunkt.transform.up * 50f);
                    lineRenderer[1].SetPosition(1, new Vector3(lineRenderer[1].GetPosition(1).x, lineRenderer[1].GetPosition(1).y, 120f));
                    //Dark Line
                    lineRendererDark[1].SetPosition(0, Feuerpunkt.transform.position);
                    lineRendererDark[1].SetPosition(1, Feuerpunkt.transform.position + Feuerpunkt.transform.up * 50f);
                    lineRendererDark[1].SetPosition(1, new Vector3(lineRendererDark[1].GetPosition(1).x, lineRendererDark[1].GetPosition(1).y, 120.01f));
                }
                lineRenderer[1].enabled = true;
                lineRendererDark[1].enabled = true;
                // StartCoroutine(FadeOutTracers(lineRenderer[1], lineRendererDark[1]));
                //Wait one frame
                yield return new WaitForSeconds(0.05f);
                lineRenderer[1].enabled = false;
                lineRendererDark[1].enabled = false;
                // ResetTracers(lineRenderer[1], lineRendererDark[1]);
                yield return new WaitForSecondsRealtime(0.3f);
            }
            ende:
            isshooting = false;
        }
        //Reload M4
        if(Inv.slot2_mag_ammo == 0 && inreload == false && Inv.mid_ammo > 0 && Inv.M4_Selected == true && isshooting == false){
            inreload = true;
            //Animation 
            animator.SetFloat("ReloadSpeed", .9f); //Animation Dauert genau 3.27 Sekunden
            animator.SetBool("inreload", true);
            //Reload
            Inv.mid_ammo -= 25;
            if(Inv.mid_ammo < 0){
                Inv.mid_ammo += 25;
                Inv.slot2_mag_ammo = Inv.mid_ammo; // RELOAD ZU SCHNELL
                Inv.mid_ammo = 0;
                yield return new WaitForSeconds (3.27f);
                goto reloadend;
            }
            yield return new WaitForSeconds (3.27f);
            Inv.slot2_mag_ammo += 25;
            reloadend:
            animator.SetBool("inreload", false);
            inreload = false;
        }

        //Für Automatische Gewähre. (Ak47)
        if(Inv.Ak47_Selected == true && isshooting == false && Inv.slot2_mag_ammo > 0){
            isshooting = true;
            for( ; Inv.slot2_mag_ammo != 0 ; Inv.slot2_mag_ammo--){
                if(shootbttn == false) goto ende;
                //Raycast schuss
                RaycastHit2D hitinfo = Physics2D.Raycast(Feuerpunkt.transform.position, Feuerpunkt.transform.up, 50f, layerMask); //Glock schießt 50f weit
                if(hitinfo){
                    if(hitinfo.collider.gameObject.tag == "Player"){
                        hitinfo.collider.GetComponent<Player_Health>().Damage(35);
                        Damage_dealt += 35;
                    }else if(hitinfo.collider.gameObject.tag == "Bot"){
                        if(hitinfo.collider.GetComponent<Bot_Health>().health <= 35) gameObject.GetComponent<Inventory_Handler>().Kills++;
                        hitinfo.collider.GetComponent<Bot_Health>().Damage(35, "Player", 3);
                        Damage_dealt += 35;
                    }
                
                    HitAnimation(hitinfo);
                    lineRenderer[1].SetPosition(0, new Vector3(Feuerpunkt.transform.position.x, Feuerpunkt.transform.position.y, 120f));
                    lineRenderer[1].SetPosition(1, new Vector3(hitinfo.point.x, hitinfo.point.y, 120f));
                    //Dark Line
                    lineRendererDark[1].SetPosition(0, new Vector3(Feuerpunkt.transform.position.x, Feuerpunkt.transform.position.y, 120.01f));
                    lineRendererDark[1].SetPosition(1, new Vector3(hitinfo.point.x, hitinfo.point.y, 120.01f));
                }else{
                    lineRenderer[1].SetPosition(0, Feuerpunkt.transform.position);
                    lineRenderer[1].SetPosition(1, Feuerpunkt.transform.position + Feuerpunkt.transform.up * 50f);
                    lineRenderer[1].SetPosition(1, new Vector3(lineRenderer[1].GetPosition(1).x, lineRenderer[1].GetPosition(1).y, 120f));
                    //Dark Line
                    lineRendererDark[1].SetPosition(0, Feuerpunkt.transform.position);
                    lineRendererDark[1].SetPosition(1, Feuerpunkt.transform.position + Feuerpunkt.transform.up * 50f);
                    lineRendererDark[1].SetPosition(1, new Vector3(lineRendererDark[1].GetPosition(1).x, lineRendererDark[1].GetPosition(1).y, 120.01f));
                }
                lineRenderer[1].enabled = true;
                lineRendererDark[1].enabled = true;
                // StartCoroutine(FadeOutTracers(lineRenderer[1], lineRendererDark[1]));
                //Wait one frame
                yield return new WaitForSeconds(0.05f);
                lineRenderer[1].enabled = false;
                lineRendererDark[1].enabled = false;
                // ResetTracers(lineRenderer[1], lineRendererDark[1]);
                yield return new WaitForSecondsRealtime(0.3f);
            }
            ende:
            isshooting = false;
        }
        //Reload Ak47
        if(Inv.slot2_mag_ammo == 0 && inreload == false && Inv.mid_ammo > 0 && Inv.Ak47_Selected == true && isshooting == false){
            inreload = true;
            //Animation 
            animator.SetFloat("ReloadSpeed", .9f); //Animation Dauert genau 3.27 Sekunden
            animator.SetBool("inreload", true);
            //Reload
            Inv.mid_ammo -= 25;
            if(Inv.mid_ammo < 0){
                Inv.mid_ammo += 25;
                Inv.slot2_mag_ammo = Inv.mid_ammo;
                Inv.mid_ammo = 0;
                yield return new WaitForSeconds (3.27f);
                goto reloadend;
            }
            yield return new WaitForSeconds (3.27f);
            Inv.slot2_mag_ammo += 25;
            reloadend:
            animator.SetBool("inreload", false);
            inreload = false;
        }

        //Sniper
        if(Inv.Sniper_Selected == true && isshooting == false && Inv.slot2_mag_ammo > 0){
            isshooting = true;
            ShootingAudio.clip = Sniper_Shot;
            ShootingAudio.Play();
            //Raycast schuss
            RaycastHit2D hitinfo = Physics2D.Raycast(Feuerpunkt.transform.position, Feuerpunkt.transform.up, 150f); //Glock schießt 50f weit
            if(hitinfo){
                if(hitinfo.collider.gameObject.tag == "Player"){
                    hitinfo.collider.GetComponent<Player_Health>().Damage(150);
                    Damage_dealt += 150;
                }else if(hitinfo.collider.gameObject.tag == "Bot"){
                    if(hitinfo.collider.GetComponent<Bot_Health>().health <= 150) gameObject.GetComponent<Inventory_Handler>().Kills++;
                    hitinfo.collider.GetComponent<Bot_Health>().Damage(150, "Player", 4);
                    Damage_dealt += 150;
                }
                
                HitAnimation(hitinfo);
                lineRenderer[1].SetPosition(0, new Vector3(Feuerpunkt.transform.position.x, Feuerpunkt.transform.position.y, 120f));
                lineRenderer[1].SetPosition(1, new Vector3(hitinfo.point.x, hitinfo.point.y, 120f));
                //Dark Line
                lineRendererDark[1].SetPosition(0, new Vector3(Feuerpunkt.transform.position.x, Feuerpunkt.transform.position.y, 120.01f));
                lineRendererDark[1].SetPosition(1, new Vector3(hitinfo.point.x, hitinfo.point.y, 120.01f));
            }else{
                lineRenderer[1].SetPosition(0, Feuerpunkt.transform.position);
                lineRenderer[1].SetPosition(1, Feuerpunkt.transform.position + Feuerpunkt.transform.up * 150f);
                lineRenderer[1].SetPosition(1, new Vector3(lineRenderer[1].GetPosition(1).x, lineRenderer[1].GetPosition(1).y, 120f));
                //Dark Line
                lineRendererDark[1].SetPosition(0, Feuerpunkt.transform.position);
                lineRendererDark[1].SetPosition(1, Feuerpunkt.transform.position + Feuerpunkt.transform.up * 50f);
                lineRendererDark[1].SetPosition(1, new Vector3(lineRendererDark[1].GetPosition(1).x, lineRendererDark[1].GetPosition(1).y, 120.01f));                
            }
            lineRenderer[1].enabled = true;
            lineRendererDark[1].enabled = true;
            // StartCoroutine(FadeOutTracers(lineRenderer[1], lineRendererDark[1]));
            //Wait one frame
            yield return new WaitForSeconds(0.2f);
            lineRenderer[1].enabled = false;
            lineRendererDark[1].enabled = false;
            // ResetTracers(lineRenderer[1], lineRendererDark[1]);
            Inv.slot2_mag_ammo--;
            yield return new WaitForSeconds(4f); 
            isshooting = false;
        }

        //Reload Sniper
        if(Inv.slot2_mag_ammo == 0 && inreload == false && Inv.big_ammo > 0 && Inv.Sniper_Selected == true && isshooting == false){
            inreload = true;
            //Animation 
            animator.SetFloat("ReloadSpeed", .5f); //Animation Dauert ~ 5 Sekunden
            animator.SetBool("inreload", true);
            //Reload
            Inv.big_ammo -= 5;
            if(Inv.big_ammo < 0){
                Inv.big_ammo += 5;
                Inv.slot2_mag_ammo = Inv.big_ammo;
                Inv.big_ammo = 0;
                yield return new WaitForSeconds (6f);
                goto reloadend;
            }
            Inv.slot2_mag_ammo += 5;
            yield return new WaitForSeconds (6f);
            reloadend:
            animator.SetBool("inreload", false);
            inreload = false;
        }
        //Mp7
        if(Inv.Mp7_Selected && Inv.slot2_mag_ammo > 0 && !isshooting){
            isshooting = true;
            for( ; Inv.slot2_mag_ammo != 0 ; Inv.slot2_mag_ammo--){
                if(shootbttn == false) goto ende;
                //Raycast schuss
                RaycastHit2D hitinfo = Physics2D.Raycast(Feuerpunkt.transform.position, Feuerpunkt.transform.up, 50f, layerMask); //Glock schießt 50f weit
                if(hitinfo){
                    if(hitinfo.collider.gameObject.tag == "Player"){
                        hitinfo.collider.GetComponent<Player_Health>().Damage(15);
                        Damage_dealt += 15;
                    }else if(hitinfo.collider.gameObject.tag == "Bot"){
                        if(hitinfo.collider.GetComponent<Bot_Health>().health <= 15) gameObject.GetComponent<Inventory_Handler>().Kills++;
                        hitinfo.collider.GetComponent<Bot_Health>().Damage(15, "Player", 5);
                        Damage_dealt += 15;
                    }
                
                    HitAnimation(hitinfo);
                    lineRenderer[1].SetPosition(0, new Vector3(Feuerpunkt.transform.position.x, Feuerpunkt.transform.position.y, 120f));
                    lineRenderer[1].SetPosition(1, new Vector3(hitinfo.point.x, hitinfo.point.y, 120f));
                    //Dark Line
                    lineRendererDark[1].SetPosition(0, new Vector3(Feuerpunkt.transform.position.x, Feuerpunkt.transform.position.y, 120.01f));
                    lineRendererDark[1].SetPosition(1, new Vector3(hitinfo.point.x, hitinfo.point.y, 120.01f));
                }else{
                    lineRenderer[1].SetPosition(0, Feuerpunkt.transform.position);
                    lineRenderer[1].SetPosition(1, Feuerpunkt.transform.position + Feuerpunkt.transform.up * 50f);
                    lineRenderer[1].SetPosition(1, new Vector3(lineRenderer[1].GetPosition(1).x, lineRenderer[1].GetPosition(1).y, 120f));
                    //Dark Line
                    lineRendererDark[1].SetPosition(0, Feuerpunkt.transform.position);
                    lineRendererDark[1].SetPosition(1, Feuerpunkt.transform.position + Feuerpunkt.transform.up * 50f);
                    lineRendererDark[1].SetPosition(1, new Vector3(lineRendererDark[1].GetPosition(1).x, lineRendererDark[1].GetPosition(1).y, 120.01f));
                }
                lineRenderer[1].enabled = true;
                lineRendererDark[1].enabled = true;
                // StartCoroutine(FadeOutTracers(lineRenderer[1], lineRendererDark[1]));
                //Wait one frame
                yield return new WaitForSeconds(0.05f);
                lineRenderer[1].enabled = false;
                lineRendererDark[1].enabled = false;
                // ResetTracers(lineRenderer[1], lineRendererDark[1]);
                yield return new WaitForSecondsRealtime(0.09f);
            }
            ende:
            isshooting = false;
        }
        //Reload Mp7
        if(Inv.slot2_mag_ammo == 0 && !inreload && Inv.small_ammo > 0 && Inv.Mp7_Selected && !isshooting){
            inreload = true;
            //Animation 
            animator.SetFloat("ReloadSpeed", 1f);
            animator.SetBool("inreload", true);
            //Reload
            Inv.small_ammo -= 20;
            if(Inv.small_ammo < 0){
                Inv.small_ammo += 20;
                Inv.slot2_mag_ammo = Inv.small_ammo;
                Inv.small_ammo = 0;
                yield return new WaitForSeconds(2.5f);
                goto reloadend;
            }
            yield return new WaitForSeconds(2.5f);
            Inv.slot2_mag_ammo += 20;
            reloadend:
            animator.SetBool("inreload", false);
            inreload = false;
        }
        //Shotgun
        if(Inv.Shotgun_Selected == true && isshooting == false && Inv.slot2_mag_ammo > 0){
            isshooting = true;
            ShootingAudio.clip = Shotgun_Shot;
            ShootingAudio.Play();
            //new
            //Raycast schüsse (5 Stück)
            RaycastHit2D[] Shotgunhits = new RaycastHit2D[10];
            Vector2[] Directions = new Vector2[10];
            for(int i = 0; i < 10; i++){
                //Berechne Random Winkel
                float raycastAngle = Random.Range(0f, 360f); //Desired angle offset in degrees from the initial direction
                //Check ob auch im Richtigen Bereich
                if(!(raycastAngle >= 0f && raycastAngle <= 25f || raycastAngle >= 335f && raycastAngle <= 360f)){ //reroll until it is in the right range
                    while(!(raycastAngle >= 0f && raycastAngle <= 25f || raycastAngle >= 335f && raycastAngle <= 360f)){
                        raycastAngle = Random.Range(0f, 360f);
                    }
                }
                // Cast the raycast from a position with the initial direction
                Vector2 raycastOrigin = Feuerpunkt.transform.position;
                Vector2 raycastDirection = Feuerpunkt.transform.up; // Initial direction of the ray
                // Apply the angle offset to the raycast direction
                Quaternion rotation = Quaternion.Euler(raycastAngle, raycastAngle, raycastAngle);
                raycastDirection = rotation * raycastDirection;
                Directions[i] = raycastDirection;
                Shotgunhits[i] = Physics2D.Raycast(raycastOrigin, raycastDirection, 10f, layerMask);
            }
            //Damage
            foreach(RaycastHit2D hitinfoshotgun in Shotgunhits){
                if(hitinfoshotgun){
                    if(hitinfoshotgun.collider.gameObject.tag == "Bot"){
                        if(hitinfoshotgun.collider.GetComponent<Bot_Health>().health <= 9){
                            gameObject.GetComponent<Inventory_Handler>().Kills++;
                            Killfeed(Menu_Handler.loadeddata.Saved_Player_Name, hitinfoshotgun.collider.GetComponentInChildren<TextMeshPro>().text, 8); //Player name + Bot name + Weapon ID
                        } 
                        hitinfoshotgun.collider.GetComponent<Bot_Health>().Damage(9, "Player", 8);
                        Damage_dealt += 5;
                    }
                    HitAnimation(hitinfoshotgun);
                }
            }
            //Visuelle Darstellung
            int x = 0;
            foreach(RaycastHit2D hitinfoshotgun in Shotgunhits){
                if(hitinfoshotgun){
                    lineRenderer[x].SetPosition(0, new Vector3(Feuerpunkt.transform.position.x, Feuerpunkt.transform.position.y, 120f));
                    lineRenderer[x].SetPosition(1, new Vector3(hitinfoshotgun.point.x, hitinfoshotgun.point.y, 120f));
                    //Dark Line
                    lineRendererDark[x].SetPosition(0, new Vector3(Feuerpunkt.transform.position.x, Feuerpunkt.transform.position.y, 120.01f));
                    lineRendererDark[x].SetPosition(1, new Vector3(hitinfoshotgun.point.x, hitinfoshotgun.point.y, 120.01f));
                }else{
                    lineRenderer[x].SetPosition(0, Feuerpunkt.transform.position);
                    lineRenderer[x].SetPosition(1, new Vector2(Feuerpunkt.transform.position.x, Feuerpunkt.transform.position.y) + Directions[x] * 10f);
                    lineRenderer[x].SetPosition(1, new Vector3(lineRenderer[x].GetPosition(1).x, lineRenderer[x].GetPosition(1).y, 120f));
                    //Dark Line
                    lineRendererDark[x].SetPosition(0, Feuerpunkt.transform.position);
                    lineRendererDark[x].SetPosition(1, new Vector2(Feuerpunkt.transform.position.x, Feuerpunkt.transform.position.y) + Directions[x] * 10f);
                    lineRendererDark[x].SetPosition(1, new Vector3(lineRendererDark[x].GetPosition(1).x, lineRendererDark[x].GetPosition(1).y, 120.01f));
                }
                x++;
            }
            //Alle einmal anzeigen
            for(int i = 0; i < 10; i++){
                lineRenderer[i].enabled = true;
                lineRendererDark[i].enabled = true;
                // StartCoroutine(FadeOutTracers(lineRenderer[i], lineRendererDark[i]));
            }

            //Wait one frame
            yield return new WaitForSeconds(0.05f);
            
            //Alle wieder ausblenden
            for(int i = 0; i < 10; i++){
                lineRenderer[i].enabled = false;
                lineRendererDark[i].enabled = false;
                // ResetTracers(lineRenderer[1], lineRendererDark[1]);
            }
            Inv.slot2_mag_ammo--;
            yield return new WaitForSeconds(1f); 
            isshooting = false;
        }
        //Reload Sotgun
        if(Inv.slot2_mag_ammo == 0 && !inreload && Inv.shotgun_ammo > 0 && Inv.Shotgun_Selected == true && isshooting == false){
            inreload = true;
            //Animation 
            animator.SetBool("inreloadShotgun", true);
            //Reload
            while(Inv.Shotgun_Selected && Inv.slot2_mag_ammo < 5 && Inv.shotgun_ammo > 0 && Inv.Slot2_Selected){
                yield return new WaitForSeconds(1.2f);
                Inv.shotgun_ammo--;
                Inv.slot2_mag_ammo++;
            }
            animator.SetBool("inreloadShotgun", false);
            inreload = false;
        }
    }
    

    if(Inv.Slot3_Selected == true){
        if(Inv.Slot3_Item == "" || Inv.Slot3_Item == null && !ishitting && !isshooting) StartCoroutine(Hit());
         //Glock-18
        if(Inv.Glock_18_Selected == true && isshooting == false && Inv.slot3_mag_ammo > 0){
            ShootingAudio.clip = Glock_18_Shot;
            ShootingAudio.Play();
            isshooting = true;
            //Raycast schuss
            RaycastHit2D hitinfo = Physics2D.Raycast(Feuerpunkt.transform.position, Feuerpunkt.transform.up, 50f, layerMask); //Glock schießt 50f weit
            if(hitinfo){
                if(hitinfo.collider.gameObject.tag == "Player"){
                    hitinfo.collider.GetComponent<Player_Health>().Damage(15);
                    Damage_dealt += 15;
                }else if(hitinfo.collider.gameObject.tag == "Bot"){
                    if(hitinfo.collider.GetComponent<Bot_Health>().health <= 15) gameObject.GetComponent<Inventory_Handler>().Kills++;
                    hitinfo.collider.GetComponent<Bot_Health>().Damage(15, "Player", 1);
                }
                
                HitAnimation(hitinfo);
                lineRenderer[1].SetPosition(0, new Vector3(Feuerpunkt.transform.position.x, Feuerpunkt.transform.position.y, 120f));
                lineRenderer[1].SetPosition(1, new Vector3(hitinfo.point.x, hitinfo.point.y, 120f));
                //Dark Line
                lineRendererDark[1].SetPosition(0, new Vector3(Feuerpunkt.transform.position.x, Feuerpunkt.transform.position.y, 120.01f));
                lineRendererDark[1].SetPosition(1, new Vector3(hitinfo.point.x, hitinfo.point.y, 120.01f));
            }else{
                lineRenderer[1].SetPosition(0, Feuerpunkt.transform.position);
                lineRenderer[1].SetPosition(1, Feuerpunkt.transform.position + Feuerpunkt.transform.up * 50f);
                lineRenderer[1].SetPosition(1, new Vector3(lineRenderer[1].GetPosition(1).x, lineRenderer[1].GetPosition(1).y, 120f));
                //Dark Line
                lineRendererDark[1].SetPosition(0, Feuerpunkt.transform.position);
                lineRendererDark[1].SetPosition(1, Feuerpunkt.transform.position + Feuerpunkt.transform.up * 50f);
                lineRendererDark[1].SetPosition(1, new Vector3(lineRendererDark[1].GetPosition(1).x, lineRendererDark[1].GetPosition(1).y, 120.01f));
            }
            lineRenderer[1].enabled = true;
            lineRendererDark[1].enabled = true;
            // StartCoroutine(FadeOutTracers(lineRenderer[1], lineRendererDark[1]));
            //Wait one frame
            yield return new WaitForSeconds(0.05f);
            lineRenderer[1].enabled = false;
            lineRendererDark[1].enabled = false;
            // ResetTracers(lineRenderer[1], lineRendererDark[1]);
            Inv.slot3_mag_ammo--;
            yield return new WaitForSeconds(1f); 
            isshooting = false;
        }
        //Reload Glock-18
        if(Inv.slot3_mag_ammo == 0 && inreload == false && Inv.small_ammo > 0 && Inv.Glock_18_Selected == true && isshooting == false){
            inreload = true;
            //Animation 
            animator.SetFloat("ReloadSpeed", 1.3f); //Animation Dauert genau 2 Sekunden
            animator.SetBool("inreload", true);
            //Reload
            Inv.small_ammo -= 12;
            if(Inv.small_ammo < 0){
                Inv.small_ammo += 12;
                Inv.slot3_mag_ammo = Inv.small_ammo;
                Inv.small_ammo = 0;
                yield return new WaitForSeconds (2f);
                goto reloadend;
            }
            yield return new WaitForSeconds (2f);
            Inv.slot3_mag_ammo += 12;
            reloadend:
            inreload = false;
            animator.SetBool("inreload", false);
        }
        
        //Für Automatische Gewähre. (M4)
        if(Inv.M4_Selected == true && isshooting == false && Inv.slot3_mag_ammo > 0){
            isshooting = true;
            for( ; Inv.slot3_mag_ammo != 0 ; Inv.slot3_mag_ammo--){
                if(shootbttn == false) goto ende;
                //Raycast schuss
                RaycastHit2D hitinfo = Physics2D.Raycast(Feuerpunkt.transform.position, Feuerpunkt.transform.up, 50f, layerMask); //Glock schießt 50f weit
                if(hitinfo){
                    if(hitinfo.collider.gameObject.tag == "Player"){
                        hitinfo.collider.GetComponent<Player_Health>().Damage(30);
                        Damage_dealt += 30;
                    }else if(hitinfo.collider.gameObject.tag == "Bot"){
                        if(hitinfo.collider.GetComponent<Bot_Health>().health <= 30) gameObject.GetComponent<Inventory_Handler>().Kills++;
                        hitinfo.collider.GetComponent<Bot_Health>().Damage(30, "Player", 2);
                        Damage_dealt += 30;
                    }
                
                    HitAnimation(hitinfo);
                    lineRenderer[1].SetPosition(0, new Vector3(Feuerpunkt.transform.position.x, Feuerpunkt.transform.position.y, 120f));
                    lineRenderer[1].SetPosition(1, new Vector3(hitinfo.point.x, hitinfo.point.y, 120f));
                    //Dark Line
                    lineRendererDark[1].SetPosition(0, new Vector3(Feuerpunkt.transform.position.x, Feuerpunkt.transform.position.y, 120.01f));
                    lineRendererDark[1].SetPosition(1, new Vector3(hitinfo.point.x, hitinfo.point.y, 120.01f));
                }else{
                    lineRenderer[1].SetPosition(0, Feuerpunkt.transform.position);
                    lineRenderer[1].SetPosition(1, Feuerpunkt.transform.position + Feuerpunkt.transform.up * 50f);
                    lineRenderer[1].SetPosition(1, new Vector3(lineRenderer[1].GetPosition(1).x, lineRenderer[1].GetPosition(1).y, 120f));
                    //Dark Line
                    lineRendererDark[1].SetPosition(0, Feuerpunkt.transform.position);
                    lineRendererDark[1].SetPosition(1, Feuerpunkt.transform.position + Feuerpunkt.transform.up * 50f);
                    lineRendererDark[1].SetPosition(1, new Vector3(lineRendererDark[1].GetPosition(1).x, lineRendererDark[1].GetPosition(1).y, 120.01f));
                }
                lineRenderer[1].enabled = true;
                lineRendererDark[1].enabled = true;
                // StartCoroutine(FadeOutTracers(lineRenderer[1], lineRendererDark[1]));
                //Wait one frame
                yield return new WaitForSeconds(0.05f);
                lineRenderer[1].enabled = false;
                lineRendererDark[1].enabled = false;
                // ResetTracers(lineRenderer[1], lineRendererDark[1]);
                yield return new WaitForSecondsRealtime(0.3f);
            }
            ende:
            isshooting = false;
        }
        //Reload M4
        if(Inv.slot3_mag_ammo == 0 && inreload == false && Inv.mid_ammo > 0 && Inv.M4_Selected == true && isshooting == false){
            inreload = true;
            //Animation 
            animator.SetFloat("ReloadSpeed", .9f); //Animation Dauert genau 3.27 Sekunden
            animator.SetBool("inreload", true);
            //Reload
            Inv.mid_ammo -= 25;
            if(Inv.mid_ammo < 0){
                Inv.mid_ammo += 25;
                Inv.slot3_mag_ammo = Inv.mid_ammo; // RELOAD ZU SCHNELL
                Inv.mid_ammo = 0;
                yield return new WaitForSeconds (3.27f);
                goto reloadend;
            }
            yield return new WaitForSeconds (3.27f);
            Inv.slot3_mag_ammo += 25;
            reloadend:
            animator.SetBool("inreload", false);
            inreload = false;
        }

        //Für Automatische Gewähre. (Ak47)
        if(Inv.Ak47_Selected == true && isshooting == false && Inv.slot3_mag_ammo > 0){
            isshooting = true;
            for( ; Inv.slot3_mag_ammo != 0 ; Inv.slot3_mag_ammo--){
                if(shootbttn == false) goto ende;
                //Raycast schuss
                RaycastHit2D hitinfo = Physics2D.Raycast(Feuerpunkt.transform.position, Feuerpunkt.transform.up, 50f, layerMask); //Glock schießt 50f weit
                if(hitinfo){
                    if(hitinfo.collider.gameObject.tag == "Player"){
                        hitinfo.collider.GetComponent<Player_Health>().Damage(35);
                        Damage_dealt += 35;
                    }else if(hitinfo.collider.gameObject.tag == "Bot"){
                        if(hitinfo.collider.GetComponent<Bot_Health>().health <= 35) gameObject.GetComponent<Inventory_Handler>().Kills++;
                        hitinfo.collider.GetComponent<Bot_Health>().Damage(35, "Player", 3);
                        Damage_dealt += 35;
                    }
                
                    HitAnimation(hitinfo);
                    lineRenderer[1].SetPosition(0, new Vector3(Feuerpunkt.transform.position.x, Feuerpunkt.transform.position.y, 120f));
                    lineRenderer[1].SetPosition(1, new Vector3(hitinfo.point.x, hitinfo.point.y, 120f));
                    //Dark Line
                    lineRendererDark[1].SetPosition(0, new Vector3(Feuerpunkt.transform.position.x, Feuerpunkt.transform.position.y, 120.01f));
                    lineRendererDark[1].SetPosition(1, new Vector3(hitinfo.point.x, hitinfo.point.y, 120.01f));
                }else{
                    lineRenderer[1].SetPosition(0, Feuerpunkt.transform.position);
                    lineRenderer[1].SetPosition(1, Feuerpunkt.transform.position + Feuerpunkt.transform.up * 50f);
                    lineRenderer[1].SetPosition(1, new Vector3(lineRenderer[1].GetPosition(1).x, lineRenderer[1].GetPosition(1).y, 120f));
                    //Dark Line
                    lineRendererDark[1].SetPosition(0, Feuerpunkt.transform.position);
                    lineRendererDark[1].SetPosition(1, Feuerpunkt.transform.position + Feuerpunkt.transform.up * 50f);
                    lineRendererDark[1].SetPosition(1, new Vector3(lineRendererDark[1].GetPosition(1).x, lineRendererDark[1].GetPosition(1).y, 120.01f));
                }
                lineRenderer[1].enabled = true;
                lineRendererDark[1].enabled = true;
                // StartCoroutine(FadeOutTracers(lineRenderer[1], lineRendererDark[1]));
                //Wait one frame
                yield return new WaitForSeconds(0.05f);
                lineRenderer[1].enabled = false;
                lineRendererDark[1].enabled = false;
                // ResetTracers(lineRenderer[1], lineRendererDark[1]);
                yield return new WaitForSecondsRealtime(0.3f);
            }
            ende:
            isshooting = false;
        }
        //Reload Ak47
        if(Inv.slot3_mag_ammo == 0 && inreload == false && Inv.mid_ammo > 0 && Inv.Ak47_Selected == true && isshooting == false){
            inreload = true;
            //Animation 
            animator.SetFloat("ReloadSpeed", .9f); //Animation Dauert genau 3.27 Sekunden
            animator.SetBool("inreload", true);
            //Reload
            Inv.mid_ammo -= 25;
            if(Inv.mid_ammo < 0){
                Inv.mid_ammo += 25;
                Inv.slot3_mag_ammo = Inv.mid_ammo;
                Inv.mid_ammo = 0;
                yield return new WaitForSeconds (3.27f);
                goto reloadend;
            }
            yield return new WaitForSeconds (3.27f);
            Inv.slot3_mag_ammo += 25;
            reloadend:
            animator.SetBool("inreload", false);
            inreload = false;
        }

        //Sniper
        if(Inv.Sniper_Selected == true && isshooting == false && Inv.slot3_mag_ammo > 0){
            isshooting = true;
            ShootingAudio.clip = Sniper_Shot;
            ShootingAudio.Play();
            //Raycast schuss
            RaycastHit2D hitinfo = Physics2D.Raycast(Feuerpunkt.transform.position, Feuerpunkt.transform.up, 150f); //Glock schießt 50f weit
            if(hitinfo){
                if(hitinfo.collider.gameObject.tag == "Player"){
                    hitinfo.collider.GetComponent<Player_Health>().Damage(150);
                    Damage_dealt += 150;
                }else if(hitinfo.collider.gameObject.tag == "Bot"){
                    if(hitinfo.collider.GetComponent<Bot_Health>().health <= 150) gameObject.GetComponent<Inventory_Handler>().Kills++;
                    hitinfo.collider.GetComponent<Bot_Health>().Damage(150, "Player", 4);
                    Damage_dealt += 150;
                }
                
                HitAnimation(hitinfo);
                lineRenderer[1].SetPosition(0, new Vector3(Feuerpunkt.transform.position.x, Feuerpunkt.transform.position.y, 120f));
                lineRenderer[1].SetPosition(1, new Vector3(hitinfo.point.x, hitinfo.point.y, 120f));
                //Dark Line
                lineRendererDark[1].SetPosition(0, new Vector3(Feuerpunkt.transform.position.x, Feuerpunkt.transform.position.y, 120.01f));
                lineRendererDark[1].SetPosition(1, new Vector3(hitinfo.point.x, hitinfo.point.y, 120.01f));
            }else{
                lineRenderer[1].SetPosition(0, Feuerpunkt.transform.position);
                lineRenderer[1].SetPosition(1, Feuerpunkt.transform.position + Feuerpunkt.transform.up * 150f);
                lineRenderer[1].SetPosition(1, new Vector3(lineRenderer[1].GetPosition(1).x, lineRenderer[1].GetPosition(1).y, 120f));
                //Dark Line
                lineRendererDark[1].SetPosition(0, Feuerpunkt.transform.position);
                lineRendererDark[1].SetPosition(1, Feuerpunkt.transform.position + Feuerpunkt.transform.up * 50f);
                lineRendererDark[1].SetPosition(1, new Vector3(lineRendererDark[1].GetPosition(1).x, lineRendererDark[1].GetPosition(1).y, 120.01f));
            }
            lineRenderer[1].enabled = true;
            lineRendererDark[1].enabled = true;
            // StartCoroutine(FadeOutTracers(lineRenderer[1], lineRendererDark[1]));
            //Wait one frame
            yield return new WaitForSeconds(0.2f);
            lineRenderer[1].enabled = false;
            lineRendererDark[1].enabled = false;
            // ResetTracers(lineRenderer[1], lineRendererDark[1]);
            Inv.slot3_mag_ammo--;
            yield return new WaitForSeconds(4f); 
            isshooting = false;
        }

        //Reload Sniper
        if(Inv.slot3_mag_ammo == 0 && inreload == false && Inv.big_ammo > 0 && Inv.Sniper_Selected == true && isshooting == false){
            inreload = true;
            //Animation 
            animator.SetFloat("ReloadSpeed", .5f); //Animation Dauert ~ 5 Sekunden
            animator.SetBool("inreload", true);
            //Reload
            Inv.big_ammo -= 5;
            if(Inv.big_ammo < 0){
                Inv.big_ammo += 5;
                Inv.slot3_mag_ammo = Inv.big_ammo;
                Inv.big_ammo = 0;
                yield return new WaitForSeconds (6f);
                goto reloadend;
            }
            Inv.slot3_mag_ammo += 5;
            yield return new WaitForSeconds (6f);
            reloadend:
            animator.SetBool("inreload", false);
            inreload = false;
        }
        //Mp7
        if(Inv.Mp7_Selected && Inv.slot3_mag_ammo > 0 && !isshooting){
            isshooting = true;
            for( ; Inv.slot3_mag_ammo != 0 ; Inv.slot3_mag_ammo--){
                if(shootbttn == false) goto ende;
                //Raycast schuss
                RaycastHit2D hitinfo = Physics2D.Raycast(Feuerpunkt.transform.position, Feuerpunkt.transform.up, 50f, layerMask); //Glock schießt 50f weit
                if(hitinfo){
                    if(hitinfo.collider.gameObject.tag == "Player"){
                        hitinfo.collider.GetComponent<Player_Health>().Damage(15);
                        Damage_dealt += 15;
                    }else if(hitinfo.collider.gameObject.tag == "Bot"){
                        if(hitinfo.collider.GetComponent<Bot_Health>().health <= 15) gameObject.GetComponent<Inventory_Handler>().Kills++;
                        hitinfo.collider.GetComponent<Bot_Health>().Damage(15, "Player", 5);
                        Damage_dealt += 15;
                    }
                
                    HitAnimation(hitinfo);
                    lineRenderer[1].SetPosition(0, new Vector3(Feuerpunkt.transform.position.x, Feuerpunkt.transform.position.y, 120f));
                    lineRenderer[1].SetPosition(1, new Vector3(hitinfo.point.x, hitinfo.point.y, 120f));
                    //Dark Line
                    lineRendererDark[1].SetPosition(0, new Vector3(Feuerpunkt.transform.position.x, Feuerpunkt.transform.position.y, 120.01f));
                    lineRendererDark[1].SetPosition(1, new Vector3(hitinfo.point.x, hitinfo.point.y, 120.01f));
                }else{
                    lineRenderer[1].SetPosition(0, Feuerpunkt.transform.position);
                    lineRenderer[1].SetPosition(1, Feuerpunkt.transform.position + Feuerpunkt.transform.up * 50f);
                    lineRenderer[1].SetPosition(1, new Vector3(lineRenderer[1].GetPosition(1).x, lineRenderer[1].GetPosition(1).y, 120f));
                    //Dark Line
                    lineRendererDark[1].SetPosition(0, Feuerpunkt.transform.position);
                    lineRendererDark[1].SetPosition(1, Feuerpunkt.transform.position + Feuerpunkt.transform.up * 50f);
                    lineRendererDark[1].SetPosition(1, new Vector3(lineRendererDark[1].GetPosition(1).x, lineRendererDark[1].GetPosition(1).y, 120.01f));
                }
                lineRenderer[1].enabled = true;
                lineRendererDark[1].enabled = true;
                // StartCoroutine(FadeOutTracers(lineRenderer[1], lineRendererDark[1]));
                //Wait one frame
                yield return new WaitForSeconds(0.05f);
                lineRenderer[1].enabled = false;
                lineRendererDark[1].enabled = false;
                // ResetTracers(lineRenderer[1], lineRendererDark[1]);
                yield return new WaitForSecondsRealtime(0.09f);
            }
            ende:
            isshooting = false;
        }
        //Reload Mp7
        if(Inv.slot3_mag_ammo == 0 && !inreload && Inv.small_ammo > 0 && Inv.Mp7_Selected && !isshooting){
            inreload = true;
            //Animation 
            animator.SetFloat("ReloadSpeed", 1f);
            animator.SetBool("inreload", true);
            //Reload
            Inv.small_ammo -= 20;
            if(Inv.small_ammo < 0){
                Inv.small_ammo += 20;
                Inv.slot3_mag_ammo = Inv.small_ammo;
                Inv.small_ammo = 0;
                yield return new WaitForSeconds(2.5f);
                goto reloadend;
            }
            yield return new WaitForSeconds(2.5f);
            Inv.slot3_mag_ammo += 20;
            reloadend:
            animator.SetBool("inreload", false);
            inreload = false;
        }
        //Shotgun
        if(Inv.Shotgun_Selected == true && isshooting == false && Inv.slot3_mag_ammo > 0){
            isshooting = true;
            ShootingAudio.clip = Shotgun_Shot;
            ShootingAudio.Play();
            //new
            //Raycast schüsse (5 Stück)
            RaycastHit2D[] Shotgunhits = new RaycastHit2D[10];
            Vector2[] Directions = new Vector2[10];
            for(int i = 0; i < 10; i++){
                //Berechne Random Winkel
                float raycastAngle = Random.Range(0f, 360f); //Desired angle offset in degrees from the initial direction
                //Check ob auch im Richtigen Bereich
                if(!(raycastAngle >= 0f && raycastAngle <= 25f || raycastAngle >= 335f && raycastAngle <= 360f)){ //reroll until it is in the right range
                    while(!(raycastAngle >= 0f && raycastAngle <= 25f || raycastAngle >= 335f && raycastAngle <= 360f)){
                        raycastAngle = Random.Range(0f, 360f);
                    }
                }
                // Cast the raycast from a position with the initial direction
                Vector2 raycastOrigin = Feuerpunkt.transform.position;
                Vector2 raycastDirection = Feuerpunkt.transform.up; // Initial direction of the ray
                // Apply the angle offset to the raycast direction
                Quaternion rotation = Quaternion.Euler(raycastAngle, raycastAngle, raycastAngle);
                raycastDirection = rotation * raycastDirection;
                Directions[i] = raycastDirection;
                Shotgunhits[i] = Physics2D.Raycast(raycastOrigin, raycastDirection, 10f, layerMask);
            }
            //Damage
            foreach(RaycastHit2D hitinfoshotgun in Shotgunhits){
                if(hitinfoshotgun){
                    if(hitinfoshotgun.collider.gameObject.tag == "Bot"){
                        if(hitinfoshotgun.collider.GetComponent<Bot_Health>().health <= 9){
                            gameObject.GetComponent<Inventory_Handler>().Kills++;
                            Killfeed(Menu_Handler.loadeddata.Saved_Player_Name, hitinfoshotgun.collider.GetComponentInChildren<TextMeshPro>().text, 8); //Player name + Bot name + Weapon ID
                        } 
                        hitinfoshotgun.collider.GetComponent<Bot_Health>().Damage(9, "Player", 8);
                        Damage_dealt += 5;
                    }
                    HitAnimation(hitinfoshotgun);
                }
            }
            //Visuelle Darstellung
            int x = 0;
            foreach(RaycastHit2D hitinfoshotgun in Shotgunhits){
                if(hitinfoshotgun){
                    lineRenderer[x].SetPosition(0, new Vector3(Feuerpunkt.transform.position.x, Feuerpunkt.transform.position.y, 120f));
                    lineRenderer[x].SetPosition(1, new Vector3(hitinfoshotgun.point.x, hitinfoshotgun.point.y, 120f));
                    //Dark Line
                    lineRendererDark[x].SetPosition(0, new Vector3(Feuerpunkt.transform.position.x, Feuerpunkt.transform.position.y, 120.01f));
                    lineRendererDark[x].SetPosition(1, new Vector3(hitinfoshotgun.point.x, hitinfoshotgun.point.y, 120.01f));
                }else{
                    lineRenderer[x].SetPosition(0, Feuerpunkt.transform.position);
                    lineRenderer[x].SetPosition(1, new Vector2(Feuerpunkt.transform.position.x, Feuerpunkt.transform.position.y) + Directions[x] * 10f);
                    lineRenderer[x].SetPosition(1, new Vector3(lineRenderer[x].GetPosition(1).x, lineRenderer[x].GetPosition(1).y, 120f));
                    //Dark Line
                    lineRendererDark[x].SetPosition(0, Feuerpunkt.transform.position);
                    lineRendererDark[x].SetPosition(1, new Vector2(Feuerpunkt.transform.position.x, Feuerpunkt.transform.position.y) + Directions[x] * 10f);
                    lineRendererDark[x].SetPosition(1, new Vector3(lineRendererDark[x].GetPosition(1).x, lineRendererDark[x].GetPosition(1).y, 120.01f));
                }
                x++;
            }
            //Alle einmal anzeigen
            for(int i = 0; i < 10; i++){
                lineRenderer[i].enabled = true;
                lineRendererDark[i].enabled = true;
                // StartCoroutine(FadeOutTracers(lineRenderer[i], lineRendererDark[i]));
            }

            //Wait one frame
            yield return new WaitForSeconds(0.05f);
            
            //Alle wieder ausblenden
            for(int i = 0; i < 10; i++){
                lineRenderer[i].enabled = false;
                lineRendererDark[i].enabled = false;
                // ResetTracers(lineRenderer[1], lineRendererDark[1]);
            }
            Inv.slot3_mag_ammo--;
            yield return new WaitForSeconds(1f); 
            isshooting = false;
        }
        //Reload Sotgun
        if(Inv.slot3_mag_ammo == 0 && !inreload && Inv.shotgun_ammo > 0 && Inv.Shotgun_Selected == true && isshooting == false){
            inreload = true;
            //Animation 
            animator.SetBool("inreloadShotgun", true);
            //Reload
            while(Inv.Shotgun_Selected && Inv.slot3_mag_ammo < 5 && Inv.shotgun_ammo > 0 && Inv.Slot3_Selected){
                yield return new WaitForSeconds(1.2f);
                Inv.shotgun_ammo--;
                Inv.slot3_mag_ammo++;
            }
            animator.SetBool("inreloadShotgun", false);
            inreload = false;
        }
    }
}
    public void HitAnimation(RaycastHit2D hitinfo){
        //Einschuss Animation (Blut)
        if(hitinfo.collider.gameObject.tag == "Player" || hitinfo.collider.gameObject.tag == "Bot"){
            Color impactobjectcolor = Color.red;
            GameObject animation = Instantiate(Impactanimation, hitinfo.point, Quaternion.identity);
            ParticleSystem.MainModule main = animation.GetComponent<ParticleSystem>().main;
            main.startColor = impactobjectcolor;
        }else{ //Die Farbe des getroffenem Object wird genommen und eine alternative Inpact Animation wird mit der farbe des objects abgespielt 
            int randint = Random.Range(1,3);//Es wird Random eine Farbe zwischen Grau und Braun gewählt und als Animations Farbe genutzt
            if(randint == 1)impactobjectcolor = Color.gray; //Graue Farbe
            else ColorUtility.TryParseHtmlString("#985000", out impactobjectcolor); //Braune Farbe
            GameObject animation = Instantiate(Impactanimation, hitinfo.point, Quaternion.identity);
            ParticleSystem.MainModule main = animation.GetComponent<ParticleSystem>().main;
            main.startColor = impactobjectcolor;
        }
    }
        
    private IEnumerator FadeOutTracers(LineRenderer lineRenderer, LineRenderer lineRendererDark){
        //Fade out both lines over 10 Frames
        for(int i = 0; i != 10; i++){
            lineRenderer.startColor = new Color(lineRenderer.startColor.r, lineRenderer.startColor.g, lineRenderer.startColor.b, lineRenderer.startColor.a - 0.1f);
            lineRenderer.endColor = new Color(lineRenderer.endColor.r, lineRenderer.endColor.g, lineRenderer.endColor.b, lineRenderer.endColor.a - 0.1f);
            lineRendererDark.startColor = new Color(lineRendererDark.startColor.r, lineRendererDark.startColor.g, lineRendererDark.startColor.b, lineRendererDark.startColor.a - 0.1f);
            lineRendererDark.endColor = new Color(lineRendererDark.endColor.r, lineRendererDark.endColor.g, lineRendererDark.endColor.b, lineRendererDark.endColor.a - 0.1f);
            yield return new WaitForEndOfFrame();
        }
    }
    
    private void ResetTracers(LineRenderer lineRenderer, LineRenderer lineRendererDark){
        //Setze beide wieder auf Alpha 1
        lineRenderer.startColor = new Color(lineRenderer.startColor.r, lineRenderer.startColor.g, lineRenderer.startColor.b, 1f);
        lineRenderer.endColor = new Color(lineRenderer.endColor.r, lineRenderer.endColor.g, lineRenderer.endColor.b, 1f);
    }
    //Shootbutton variablen
    public void shootbutton(){
        shootbttn = true;
        shootbttnpressed = true;
    }
    public void shootbuttonrelease(){
        if(!autoshootbttn){
            shootbttn = false;
        }
        shootbttnpressed = false;
    }

    //Autoshoot button
    public void Autoshoot(){
        if(!autoshootbttn){
            shootbttn = true;
            autoshootbttn = true;
            GameObject.Find("Autoshoot").GetComponent<Image>().sprite = Greenbutton;
        }else if(autoshootbttn){
            shootbttn = false;
            autoshootbttn = false;
            GameObject.Find("Autoshoot").GetComponent<Image>().sprite = Redbutton;
        }
    }

    public void CallKillfeedFromBot(string Opfer, string Mörder, int WeaponID){
        if(Mörder == "Player"){
            StartCoroutine(Killfeed(Menu_Handler.loadeddata.Saved_Player_Name, Opfer, WeaponID));
            //Kill Counter
            uihandler.UpdateKillCounter();
        }else if(Mörder == "Zone"){
            StartCoroutine(Killfeed(Mörder, Opfer, WeaponID));
        }else{
            StartCoroutine(Killfeed(Mörder, Opfer, WeaponID));
        }
    }
    public IEnumerator Killfeed(string Mörder, string Opfer, int Grund){
        //Schaue ob bereits ein Eintrrag im Killfeed existiert
        if(NewKillfeedEntry != null){
            //Wenn ja, dann lösche diesen
            Destroy(NewKillfeedEntry);
        }
        //Erschaffe den Killfeed eintrag im Hud
        NewKillfeedEntry = Instantiate(KillfeedEntry, new Vector3(0, 0, 0), Quaternion.identity);
        NewKillfeedEntry.transform.SetParent(KillfeedPos);
        NewKillfeedEntry.gameObject.GetComponent<RectTransform>().localPosition = Vector3.zero; //Einfach auf 0,0,0 setzen
        //schreibe alles in den Cache
        MörderText = NewKillfeedEntry.gameObject.transform.Find("Mörder").GetComponent<TextMeshProUGUI>();
        OpferText = NewKillfeedEntry.gameObject.transform.Find("Opfer").GetComponent<TextMeshProUGUI>();
        GrundImage = NewKillfeedEntry.gameObject.transform.Find("Grund").GetComponent<Image>();
        Background = NewKillfeedEntry.GetComponent<Image>();
        //Gebe dem Eintrag die richtigen Werte
        NewKillfeedEntry.gameObject.transform.Find("Mörder").GetComponent<TextMeshProUGUI>().text = Mörder;
        NewKillfeedEntry.gameObject.transform.Find("Opfer").GetComponent<TextMeshProUGUI>().text = Opfer;
        //Check ob Zone gekillt hat und wenn ja setze Mörder und Opfer gleich
        if(Mörder == "Zone"){
            NewKillfeedEntry.gameObject.transform.Find("Mörder").GetComponent<TextMeshProUGUI>().text = Opfer;
        }
        //Scale
        NewKillfeedEntry.gameObject.transform.localScale = new Vector3(1, 1, 1);
        //Grund
        if(Grund == 1){
            GrundImage.sprite = Killfeed_Glock_18;
        }else if(Grund == 2){
            GrundImage.sprite = Killfeed_M4;
        }else if(Grund == 3){
            GrundImage.sprite = Killfeed_AK_47;
        }else if(Grund == 4){
            GrundImage.sprite = Killfeed_Sniper;
        }else if(Grund == 5){
            GrundImage.sprite = Killfeed_Mp7;
        }else if(Grund == 6){
            GrundImage.sprite = Killfeed_Zone;
        }else if(Grund == 7){
            GrundImage.sprite = Killfeed_Faust;
        }else if(Grund == 8){
            GrundImage.sprite = Killfeed_Shotgun;
        }

        //Rein fliegen lassen (Animation)
        RectTransform Entrytransform = NewKillfeedEntry.gameObject.GetComponent<RectTransform>();
        Entrytransform.localPosition = new Vector2(Entrytransform.localPosition.x, Entrytransform.localPosition.y + 100f);
        for(int i = 0; i != 50; i++){
            Entrytransform.localPosition = new Vector2(Entrytransform.localPosition.x, Entrytransform.localPosition.y - 2f);
            yield return new WaitForEndOfFrame();
        }

        yield return new WaitForSeconds(5f);
        //Killfeed langsam ausblenden
        for(int i = 0; i != 500; i++){
            MörderText.color = new Color(MörderText.color.r, MörderText.color.g, MörderText.color.b, MörderText.color.a - 0.002f);
            OpferText.color = new Color(OpferText.color.r, OpferText.color.g, OpferText.color.b, OpferText.color.a - 0.002f);
            GrundImage.color = new Color(GrundImage.color.r, GrundImage.color.g, GrundImage.color.b, GrundImage.color.a - 0.002f);
            Background.color = new Color(Background.color.r, Background.color.g, Background.color.b, Background.color.a - 0.0008941176470588236f);
            yield return new WaitForEndOfFrame();
        }

        //Killfeed Eintrag löschen
        Destroy(NewKillfeedEntry);
        NewKillfeedEntry = null;
    }

    //Reloadbutton variablen
    public void reloadbutton(){
        reloadbttn = true;
    }
    public void reloadbuttonrelease(){
        reloadbttn = false;
    }

    public IEnumerator reload(){
        if(!inreload){
            if(Inv.Slot1_Selected){
                //Reload Glock-18
                if(Inv.slot1_mag_ammo != 12 && inreload == false && Inv.small_ammo > 0 && Inv.Glock_18_Selected == true && isshooting == false){
                    inreload = true;
                    //Animation 
                    animator.SetFloat("ReloadSpeed", 1.3f); //Animation Dauert genau 2 Sekunden
                    animator.SetBool("inreload", true);
                    //Reload
                    Inv.small_ammo -= 12;
                    if(Inv.small_ammo < 0){
                        Inv.small_ammo += 12;
                        Inv.slot1_mag_ammo = Inv.small_ammo;
                        Inv.small_ammo = 0;
                        yield return new WaitForSeconds (2f);
                        goto reloadend;
                    }
                    yield return new WaitForSeconds (2f);
                    Inv.slot1_mag_ammo += 12;
                    reloadend:
                    animator.SetBool("inreload", false);
                    inreload = false;
                }

                //Reload M4
                if(inreload == false && Inv.M4_Selected == true && isshooting == false && Inv.slot1_mag_ammo != 25){
                    inreload = true;
                    //Animation 
                    animator.SetFloat("ReloadSpeed", .9f); //Animation Dauert genau 3.27 Sekunden
                    animator.SetBool("inreload", true);
                    //Reload
                    yield return new WaitForSeconds(3.27f);
                    Inv.mid_ammo += Inv.slot1_mag_ammo;
                    if(Inv.mid_ammo >= 25){
                        Inv.slot1_mag_ammo = 25;
                        Inv.mid_ammo -= 25;
                    }else if(Inv.mid_ammo < 25){
                        Inv.slot1_mag_ammo = Inv.mid_ammo;
                        Inv.mid_ammo = 0;
                    }
                    inreload = false;
                    animator.SetBool("inreload", false);
                }

                //Reload AK-47
                if(inreload == false && Inv.Ak47_Selected == true && isshooting == false && Inv.slot1_mag_ammo != 25){
                    inreload = true;
                    //Animation 
                    animator.SetFloat("ReloadSpeed", .9f); //Animation Dauert genau 3.27 Sekunden
                    animator.SetBool("inreload", true);
                    //Reload
                    yield return new WaitForSeconds(3.27f);
                    Inv.mid_ammo += Inv.slot1_mag_ammo;
                    if(Inv.mid_ammo >= 25){
                        Inv.slot1_mag_ammo = 25;
                        Inv.mid_ammo -= 25;
                    }else if(Inv.mid_ammo < 25){
                        Inv.slot1_mag_ammo = Inv.mid_ammo;
                        Inv.mid_ammo = 0;
                    }
                    inreload = false;
                    animator.SetBool("inreload", false);
                }

                //Reload Sniper
                if(inreload == false && Inv.Sniper_Selected == true && isshooting == false && Inv.slot1_mag_ammo != 5){
                    inreload = true;
                    //Animation 
                    animator.SetFloat("ReloadSpeed", .5f); //Animation Dauert ~ 5 Sekunden
                    animator.SetBool("inreload", true);
                    //Reload
                    yield return new WaitForSeconds(6f);
                    Inv.big_ammo += Inv.slot1_mag_ammo;
                    if(Inv.big_ammo >= 5){
                        Inv.slot1_mag_ammo = 5;
                        Inv.big_ammo -= 5;
                    }else if(Inv.big_ammo < 5){
                        Inv.slot1_mag_ammo = Inv.big_ammo;
                        Inv.big_ammo = 0;
                    }
                    inreload = false;
                    animator.SetBool("inreload", false);
                }
                //Mp7
                if(Inv.slot1_mag_ammo == 0 && !inreload && Inv.small_ammo > 0 && Inv.Mp7_Selected && !isshooting){
                    inreload = true;
                    //Animation 
                    animator.SetFloat("ReloadSpeed", 1f);
                    animator.SetBool("inreload", true);
                    //Reload
                    Inv.small_ammo -= 20;
                    if(Inv.small_ammo < 0){
                        Inv.small_ammo += 20;
                        Inv.slot1_mag_ammo = Inv.small_ammo;
                        Inv.small_ammo = 0;
                        yield return new WaitForSeconds(2.5f);
                        goto reloadend;
                    }
                    yield return new WaitForSeconds(2.5f);
                    Inv.slot1_mag_ammo += 20;
                    reloadend:
                    animator.SetBool("inreload", false);
                    inreload = false;
                }
                //Reload Sotgun
                if(!inreload && Inv.shotgun_ammo > 0 && Inv.Shotgun_Selected == true && isshooting == false){
                    inreload = true;
                    //Animation 
                    animator.SetBool("inreloadShotgun", true);
                    //Reload
                    while(Inv.Shotgun_Selected && Inv.slot1_mag_ammo < 5 && Inv.shotgun_ammo > 0 && Inv.Slot1_Selected){
                        yield return new WaitForSeconds(1.2f);
                        Inv.shotgun_ammo--;
                        Inv.slot1_mag_ammo++;
                    }
                    animator.SetBool("inreloadShotgun", false);
                    inreload = false;
                }
            }
            else if(Inv.Slot2_Selected){
                //Reload Glock-18
                if(Inv.slot2_mag_ammo != 12 && inreload == false && Inv.small_ammo > 0 && Inv.Glock_18_Selected == true && isshooting == false){
                    inreload = true;
                    //Animation 
                    animator.SetFloat("ReloadSpeed", 1.3f); //Animation Dauert genau 2 Sekunden
                    animator.SetBool("inreload", true);
                    //Reload
                    Inv.small_ammo -= 12;
                    if(Inv.small_ammo < 0){
                        Inv.small_ammo += 12;
                        Inv.slot2_mag_ammo = Inv.small_ammo;
                        Inv.small_ammo = 0;
                        yield return new WaitForSeconds (2f);
                        goto reloadend;
                    }
                    yield return new WaitForSeconds (2f);
                    Inv.slot2_mag_ammo += 12;
                    reloadend:
                    inreload = false;
                    animator.SetBool("inreload", false);
                }

                //Reload M4
                if(inreload == false && Inv.M4_Selected == true && isshooting == false && Inv.slot2_mag_ammo != 25){
                    inreload = true;
                    //Animation 
                    animator.SetFloat("ReloadSpeed", .9f); //Animation Dauert genau 3.27 Sekunden
                    animator.SetBool("inreload", true);
                    //Reload
                    yield return new WaitForSeconds(3.27f);
                    Inv.mid_ammo += Inv.slot2_mag_ammo;
                    if(Inv.mid_ammo >= 25){
                        Inv.slot2_mag_ammo = 25;
                        Inv.mid_ammo -= 25;
                    }else if(Inv.mid_ammo < 25){
                        Inv.slot2_mag_ammo = Inv.mid_ammo;
                        Inv.mid_ammo = 0;
                    }
                    inreload = false;
                    animator.SetBool("inreload", false);
                }

                //Reload Ak
                if(inreload == false && Inv.Ak47_Selected == true && isshooting == false && Inv.slot2_mag_ammo != 25){
                    inreload = true;
                    //Animation 
                    animator.SetFloat("ReloadSpeed", .9f); //Animation Dauert genau 3.27 Sekunden
                    animator.SetBool("inreload", true);
                    //Reload
                    yield return new WaitForSeconds(3.27f);
                    Inv.mid_ammo += Inv.slot2_mag_ammo;
                    if(Inv.mid_ammo >= 25){
                        Inv.slot2_mag_ammo = 25;
                        Inv.mid_ammo -= 25;
                    }else if(Inv.mid_ammo < 25){
                        Inv.slot2_mag_ammo = Inv.mid_ammo;
                        Inv.mid_ammo = 0;
                    }
                    inreload = false;
                    animator.SetBool("inreload", false);
                }

                //Reload Sniper
                if(inreload == false && Inv.Sniper_Selected == true && isshooting == false && Inv.slot2_mag_ammo != 5){
                    inreload = true;
                    //Animation 
                    animator.SetFloat("ReloadSpeed", .5f); //Animation Dauert ~ 5 Sekunden
                    animator.SetBool("inreload", true);
                    //Reload
                    yield return new WaitForSeconds(6f);
                    Inv.big_ammo += Inv.slot2_mag_ammo;
                    if(Inv.big_ammo >= 5){
                        Inv.slot2_mag_ammo = 5;
                        Inv.big_ammo -= 5;
                    }else if(Inv.big_ammo < 5){
                        Inv.slot2_mag_ammo = Inv.big_ammo;
                        Inv.big_ammo = 0;
                    }
                    inreload = false;
                    animator.SetBool("inreload", false);
                }
                //Mp7
                if(Inv.slot2_mag_ammo == 0 && !inreload && Inv.small_ammo > 0 && Inv.Mp7_Selected && !isshooting){
                    inreload = true;
                    //Animation 
                    animator.SetFloat("ReloadSpeed", 1f);
                    animator.SetBool("inreload", true);
                    //Reload
                    Inv.small_ammo -= 20;
                    if(Inv.small_ammo < 0){
                        Inv.small_ammo += 20;
                        Inv.slot2_mag_ammo = Inv.small_ammo;
                        Inv.small_ammo = 0;
                        yield return new WaitForSeconds(2.5f);
                        goto reloadend;
                    }
                    yield return new WaitForSeconds(2.5f);
                    Inv.slot2_mag_ammo += 20;
                    reloadend:
                    animator.SetBool("inreload", false);
                    inreload = false;
                }
                //Reload Sotgun
                if(Inv.slot2_mag_ammo == 0 && !inreload && Inv.shotgun_ammo > 0 && Inv.Shotgun_Selected == true && isshooting == false){
                    inreload = true;
                    //Animation 
                    animator.SetBool("inreloadShotgun", true);
                    //Reload
                    while(Inv.Shotgun_Selected && Inv.slot2_mag_ammo < 5 && Inv.shotgun_ammo > 0 && Inv.Slot2_Selected){
                        yield return new WaitForSeconds(1.2f);
                        Inv.shotgun_ammo--;
                        Inv.slot2_mag_ammo++;
                    }
                    animator.SetBool("inreloadShotgun", false);
                    inreload = false;
                }
            }
            else if(Inv.Slot3_Selected){
                //Reload Glock-18
                if(inreload == false && Inv.Glock_18_Selected == true && isshooting == false && Inv.slot3_mag_ammo != 12 && Inv.slot3_mag_ammo != 0){
                    inreload = true;
                    //Animation 
                    animator.SetFloat("ReloadSpeed", 1.3f); //Animation Dauert genau 2 Sekunden
                    animator.SetBool("inreload", true);
                    //Reload
                    yield return new WaitForSeconds(2f);
                    Inv.small_ammo += Inv.slot3_mag_ammo;
                    if(Inv.small_ammo >= 12){
                        Inv.slot3_mag_ammo = 12;
                        Inv.small_ammo -= 12;
                    }else if(Inv.small_ammo < 12){
                        Inv.slot3_mag_ammo = Inv.small_ammo;
                        Inv.small_ammo = 0;
                    }
                    inreload = false;
                    animator.SetBool("inreload", false);
                }

                //Reload M4
                if(inreload == false && Inv.M4_Selected == true && isshooting == false && Inv.slot3_mag_ammo != 25){
                    inreload = true;
                    //Animation 
                    animator.SetFloat("ReloadSpeed", .9f); //Animation Dauert genau 3.27 Sekunden
                    animator.SetBool("inreload", true);
                    //Reload
                    yield return new WaitForSeconds(3.27f);
                    Inv.mid_ammo += Inv.slot3_mag_ammo;
                    if(Inv.mid_ammo >= 25){
                        Inv.slot3_mag_ammo = 25;
                        Inv.mid_ammo -= 25;
                    }else if(Inv.mid_ammo < 25){
                        Inv.slot3_mag_ammo = Inv.mid_ammo;
                        Inv.mid_ammo = 0;
                    }
                    inreload = false;
                    animator.SetBool("inreload", false);
                }

                //Reload Ak47
                if(inreload == false && Inv.Ak47_Selected == true && isshooting == false && Inv.slot3_mag_ammo != 25){
                    inreload = true;
                    //Animation 
                    animator.SetFloat("ReloadSpeed", .9f); //Animation Dauert genau 3.27 Sekunden
                    animator.SetBool("inreload", true);
                    //Reload
                    yield return new WaitForSeconds(3.27f);
                    Inv.mid_ammo += Inv.slot3_mag_ammo;
                    if(Inv.mid_ammo >= 25){
                        Inv.slot3_mag_ammo = 25;
                        Inv.mid_ammo -= 25;
                    }else if(Inv.mid_ammo < 25){
                        Inv.slot3_mag_ammo = Inv.mid_ammo;
                        Inv.mid_ammo = 0;
                    }
                    inreload = false;
                    animator.SetBool("inreload", false);
                }

                //Reload Sniper
                if(inreload == false && Inv.Sniper_Selected == true && isshooting == false && Inv.slot3_mag_ammo != 5){
                    inreload = true;
                    //Animation 
                    animator.SetFloat("ReloadSpeed", .5f); //Animation Dauert ~ 5 Sekunden
                    animator.SetBool("inreload", true);
                    //Reload
                    yield return new WaitForSeconds(6f);
                    Inv.big_ammo += Inv.slot3_mag_ammo;
                    if(Inv.big_ammo >= 5){
                        Inv.slot3_mag_ammo = 5;
                        Inv.big_ammo -= 5;
                    }else if(Inv.big_ammo < 5){
                        Inv.slot3_mag_ammo = Inv.big_ammo;
                        Inv.big_ammo = 0;
                    }
                    inreload = false;
                    animator.SetBool("inreload", false);
                }
                //Reload Mp7
                if(Inv.slot3_mag_ammo == 0 && !inreload && Inv.small_ammo > 0 && Inv.Mp7_Selected && !isshooting){
                    inreload = true;
                    //Animation 
                    animator.SetFloat("ReloadSpeed", 1f);
                    animator.SetBool("inreload", true);
                    //Reload
                    Inv.small_ammo -= 20;
                    if(Inv.small_ammo < 0){
                        Inv.small_ammo += 20;
                        Inv.slot3_mag_ammo = Inv.small_ammo;
                        Inv.small_ammo = 0;
                        yield return new WaitForSeconds(2.5f);
                        goto reloadend;
                    }
                    yield return new WaitForSeconds(2.5f);
                    Inv.slot3_mag_ammo += 20;
                    reloadend:
                    animator.SetBool("inreload", false);
                    inreload = false;
                }
                if(Inv.slot3_mag_ammo == 0 && !inreload && Inv.shotgun_ammo > 0 && Inv.Shotgun_Selected == true && isshooting == false){
                    inreload = true;
                    //Animation 
                    animator.SetBool("inreloadShotgun", true);
                    //Reload
                    while(Inv.Shotgun_Selected && Inv.slot3_mag_ammo < 5 && Inv.shotgun_ammo > 0 && Inv.Slot3_Selected){
                        yield return new WaitForSeconds(1.2f);
                        Inv.shotgun_ammo--;
                        Inv.slot3_mag_ammo++;
                    }
                    animator.SetBool("inreloadShotgun", false);
                    inreload = false;
                }
            }
        }
    }
}
