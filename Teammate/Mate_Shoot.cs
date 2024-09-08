using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Mate_Shoot : MonoBehaviour
{
    Mate_Inventory Inv;
    public GameObject Mate; //Mate
    public GameObject Bullet;
    public GameObject Feuerpunkt; 
    public bool isshooting;
    public bool inreload;
    [SerializeField] private GameObject Impactanimation;
    [SerializeField] private LineRenderer[] lineRenderer;
    private Mate_Behavior Mate_behavior;
    private Color impactobjectcolor;
    // Start is called before the first frame update
    void Awake()
    {
        Inv = Mate.GetComponent<Mate_Inventory>(); 
        Mate_behavior = Mate.GetComponent<Mate_Behavior>();
    }

    // Update is called once per frame

    public IEnumerator Shoot(){
        if(Inv.Slot1_Selected){
            //Glock-18
            if(Inv.Glock_18_Selected == true && isshooting == false && Inv.slot1_mag_ammo > 0){
                isshooting = true;
                //Raycast schuss
                RaycastHit2D hitinfo = Physics2D.Raycast(Feuerpunkt.transform.position, Feuerpunkt.transform.up, 50f); //Glock schießt 50f weit
                if(hitinfo){
                    if(hitinfo.collider.gameObject.tag == "Player"){
                        hitinfo.collider.GetComponent<Player_Health>().Damage(15);
                    }else if(hitinfo.collider.gameObject.tag == "Bot"){
                        hitinfo.collider.GetComponent<Bot_Health>().Damage(15, Mate.GetComponentInChildren<TextMeshPro>().text, 1);
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
                    lineRenderer[0].SetPosition(0, new Vector3(Feuerpunkt.transform.position.x, Feuerpunkt.transform.position.y, 120f));
                    lineRenderer[0].SetPosition(1, new Vector3(hitinfo.point.x, hitinfo.point.y, 120f));
                }else{
                    lineRenderer[0].SetPosition(0, Feuerpunkt.transform.position);
                    lineRenderer[0].SetPosition(1, Feuerpunkt.transform.position + Feuerpunkt.transform.up * 50f);
                    lineRenderer[0].SetPosition(1, new Vector3(lineRenderer[0].GetPosition(1).x, lineRenderer[0].GetPosition(1).y, 120f));
                }
                lineRenderer[0].enabled = true;
                //Wait one frame
                yield return new WaitForSeconds(0.05f);
                lineRenderer[0].enabled = false;
                Inv.slot1_mag_ammo--;
                yield return new WaitForSeconds(1f); 
                isshooting = false;
            }
            //Check ob Waffe Gewechselt werden muss
            if(Inv.Glock_18_Selected == true && Inv.slot1_mag_ammo == 0 && Inv.small_ammo == 0 && isshooting == false){
            StartCoroutine(SwitchtonextWeapon());
            }
            //Reload Glock-18
            if(Inv.slot1_mag_ammo == 0 && inreload == false && Inv.small_ammo > 0 && Inv.Glock_18_Selected == true && isshooting == false){
                inreload = true;
                Inv.small_ammo -= 12;
                if(Inv.small_ammo < 0){
                    Inv.small_ammo += 12;
                    Inv.slot1_mag_ammo = Inv.small_ammo;
                    Inv.small_ammo = 0;
                    yield return new WaitForSeconds (2.5f);
                    goto reloadend;
                }
                yield return new WaitForSeconds (2.5f);
                Inv.slot1_mag_ammo += 12;
                reloadend:
                inreload = false;
            }
            
            //Für Automatische Gewähre. (M4)
            if(Inv.M4_Selected == true && isshooting == false && Inv.slot1_mag_ammo > 0){
                isshooting = true;
                for( ; Inv.slot1_mag_ammo != 0 ; Inv.slot1_mag_ammo--){
                    //Raycast schuss
                    RaycastHit2D hitinfo = Physics2D.Raycast(Feuerpunkt.transform.position, Feuerpunkt.transform.up, 50f); //Glock schießt 50f weit
                    if(hitinfo){
                        if(hitinfo.collider.gameObject.tag == "Player"){
                        hitinfo.collider.GetComponent<Player_Health>().Damage(30);
                    }else if(hitinfo.collider.gameObject.tag == "Bot"){
                        hitinfo.collider.GetComponent<Bot_Health>().Damage(30, Mate.GetComponentInChildren<TextMeshPro>().text, 2);
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

                        lineRenderer[0].SetPosition(0, new Vector3(Feuerpunkt.transform.position.x, Feuerpunkt.transform.position.y, 120f));
                        lineRenderer[0].SetPosition(1, new Vector3(hitinfo.point.x, hitinfo.point.y, 120f));
                    }else{
                        lineRenderer[0].SetPosition(0, Feuerpunkt.transform.position);
                        lineRenderer[0].SetPosition(1, Feuerpunkt.transform.position + Feuerpunkt.transform.up * 50f);
                        lineRenderer[0].SetPosition(1, new Vector3(lineRenderer[0].GetPosition(1).x, lineRenderer[0].GetPosition(1).y, 120f));
                    }
                    lineRenderer[0].enabled = true;
                    //Wait one frame
                    yield return new WaitForSeconds(0.05f);
                    lineRenderer[0].enabled = false;
                    yield return new WaitForSecondsRealtime(0.3f);
                }
                isshooting = false;
            }
            //Check ob Waffe Gewechselt werden muss
            if(Inv.M4_Selected == true && Inv.slot1_mag_ammo == 0 && Inv.mid_ammo == 0 && isshooting == false){
            StartCoroutine(SwitchtonextWeapon());
            }
            //Reload M4
            if(Inv.slot1_mag_ammo == 0 && inreload == false && Inv.mid_ammo > 0 && Inv.M4_Selected == true && isshooting == false){
                inreload = true;
                Inv.mid_ammo -= 25;
                if(Inv.mid_ammo < 0){
                    Inv.mid_ammo += 25;
                    Inv.slot1_mag_ammo = Inv.mid_ammo; // RELOAD ZU SCHNELL
                    Inv.mid_ammo = 0;
                    yield return new WaitForSeconds (3.5f);
                    goto reloadend;
                }
                yield return new WaitForSeconds (3.5f);
                Inv.slot1_mag_ammo += 25;
                reloadend:
                inreload = false;
            }

            //Für Automatische Gewähre. (Ak47)
            if(Inv.Ak47_Selected == true && isshooting == false && Inv.slot1_mag_ammo > 0){
                isshooting = true;
                for( ; Inv.slot1_mag_ammo != 0 ; Inv.slot1_mag_ammo--){
                    //Raycast schuss
                    RaycastHit2D hitinfo = Physics2D.Raycast(Feuerpunkt.transform.position, Feuerpunkt.transform.up, 50f); //Glock schießt 50f weit
                    if(hitinfo){
                        if(hitinfo.collider.gameObject.tag == "Player"){
                        hitinfo.collider.GetComponent<Player_Health>().Damage(35);
                        }else if(hitinfo.collider.gameObject.tag == "Bot"){
                            hitinfo.collider.GetComponent<Bot_Health>().Damage(35, Mate.GetComponentInChildren<TextMeshPro>().text, 3);
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

                        lineRenderer[0].SetPosition(0, new Vector3(Feuerpunkt.transform.position.x, Feuerpunkt.transform.position.y, 120f));
                        lineRenderer[0].SetPosition(1, new Vector3(hitinfo.point.x, hitinfo.point.y, 120f));
                    }else{
                        lineRenderer[0].SetPosition(0, Feuerpunkt.transform.position);
                        lineRenderer[0].SetPosition(1, Feuerpunkt.transform.position + Feuerpunkt.transform.up * 50f);
                        lineRenderer[0].SetPosition(1, new Vector3(lineRenderer[0].GetPosition(1).x, lineRenderer[0].GetPosition(1).y, 120f));
                    }
                    lineRenderer[0].enabled = true;
                    //Wait one frame
                    yield return new WaitForSeconds(0.05f);
                    lineRenderer[0].enabled = false;
                    yield return new WaitForSecondsRealtime(0.3f);
                }
                isshooting = false;
            }
            //Check ob Waffe Gewechselt werden muss
            if(Inv.Ak47_Selected == true && Inv.slot1_mag_ammo == 0 && Inv.mid_ammo == 0 && isshooting == false){
            StartCoroutine(SwitchtonextWeapon());
            }
            //Reload Ak47
            if(Inv.slot1_mag_ammo == 0 && inreload == false && Inv.mid_ammo > 0 && Inv.Ak47_Selected == true && isshooting == false){
                inreload = true;
                Inv.mid_ammo -= 25;
                if(Inv.mid_ammo < 0){
                    Inv.mid_ammo += 25;
                    Inv.slot1_mag_ammo = Inv.mid_ammo;
                    Inv.mid_ammo = 0;
                    yield return new WaitForSeconds (3.5f);
                    goto reloadend;
                }
                yield return new WaitForSeconds (3.5f);
                Inv.slot1_mag_ammo += 25;
                reloadend:
                inreload = false;
            }

            //Sniper
            if(Inv.Sniper_Selected == true && isshooting == false && Inv.slot1_mag_ammo > 0){
                isshooting = true;
                //Raycast schuss
                RaycastHit2D hitinfo = Physics2D.Raycast(Feuerpunkt.transform.position, Feuerpunkt.transform.up, 150f); //Glock schießt 50f weit
                if(hitinfo){
                    if(hitinfo.collider.gameObject.tag == "Player"){
                        hitinfo.collider.GetComponent<Player_Health>().Damage(150);
                    }else if(hitinfo.collider.gameObject.tag == "Bot"){
                        hitinfo.collider.GetComponent<Bot_Health>().Damage(150, Mate.GetComponentInChildren<TextMeshPro>().text, 4);
                    }
                    
                    //Einschuss Animation (Blut)
                    if(hitinfo.collider.gameObject.tag == "Player" || hitinfo.collider.gameObject.tag == "Bot"){
                        Instantiate(Impactanimation, hitinfo.point, Quaternion.identity);
                    }
                    lineRenderer[0].SetPosition(0, new Vector3(Feuerpunkt.transform.position.x, Feuerpunkt.transform.position.y, 120f));
                    lineRenderer[0].SetPosition(1, new Vector3(hitinfo.point.x, hitinfo.point.y, 120f));
                }else{
                    lineRenderer[0].SetPosition(0, Feuerpunkt.transform.position);
                    lineRenderer[0].SetPosition(1, Feuerpunkt.transform.position + Feuerpunkt.transform.up * 150f);
                    lineRenderer[0].SetPosition(1, new Vector3(lineRenderer[0].GetPosition(1).x, lineRenderer[0].GetPosition(1).y, 120f));
                }
                lineRenderer[0].enabled = true;
                //Wait one frame
                yield return new WaitForSeconds(0.2f);
                lineRenderer[0].enabled = false;
                Inv.slot1_mag_ammo--;
                yield return new WaitForSeconds(4f); 
                isshooting = false;
            }
            //Check ob Waffe Gewechselt werden muss
            if(Inv.Sniper_Selected == true && Inv.slot1_mag_ammo == 0 && Inv.big_ammo == 0 && isshooting == false){
            StartCoroutine(SwitchtonextWeapon());
            }
            //Reload Sniper
            if(Inv.slot1_mag_ammo == 0 && inreload == false && Inv.big_ammo > 0 && Inv.Sniper_Selected == true && isshooting == false){
                inreload = true;
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
                inreload = false;
            }
            //Mp7
            if(Inv.Mp7_Selected == true && isshooting == false && Inv.slot1_mag_ammo > 0){
                isshooting = true;
                for( ; Inv.slot1_mag_ammo != 0 ; Inv.slot1_mag_ammo--){
                    //Raycast schuss
                    RaycastHit2D hitinfo = Physics2D.Raycast(Feuerpunkt.transform.position, Feuerpunkt.transform.up, 50f); //Glock schießt 50f weit
                    if(hitinfo){
                        if(hitinfo.collider.gameObject.tag == "Player"){
                        hitinfo.collider.GetComponent<Player_Health>().Damage(15);
                        }else if(hitinfo.collider.gameObject.tag == "Bot"){
                            hitinfo.collider.GetComponent<Bot_Health>().Damage(15, Mate.GetComponentInChildren<TextMeshPro>().text, 5);
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

                        lineRenderer[0].SetPosition(0, new Vector3(Feuerpunkt.transform.position.x, Feuerpunkt.transform.position.y, 120f));
                        lineRenderer[0].SetPosition(1, new Vector3(hitinfo.point.x, hitinfo.point.y, 120f));
                    }else{
                        lineRenderer[0].SetPosition(0, Feuerpunkt.transform.position);
                        lineRenderer[0].SetPosition(1, Feuerpunkt.transform.position + Feuerpunkt.transform.up * 50f);
                        lineRenderer[0].SetPosition(1, new Vector3(lineRenderer[0].GetPosition(1).x, lineRenderer[0].GetPosition(1).y, 120f));
                    }
                    lineRenderer[0].enabled = true;
                    //Wait one frame
                    yield return new WaitForSeconds(0.05f);
                    lineRenderer[0].enabled = false;
                    yield return new WaitForSecondsRealtime(0.09f);
                }
                isshooting = false;
            }
            //Check ob Waffe Gewechselt werden muss
            if(Inv.Mp7_Selected == true && Inv.slot1_mag_ammo == 0 && Inv.small_ammo == 0 && isshooting == false){
            StartCoroutine(SwitchtonextWeapon());
            }
            //Reload Mp7
            if(Inv.slot1_mag_ammo == 0 && inreload == false && Inv.small_ammo > 0 && Inv.Mp7_Selected == true && isshooting == false){
                inreload = true;
                //Animation 
                //animator.SetFloat("ReloadSpeed", 1f);
                //animator.SetBool("inreload", true);
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
                //animator.SetBool("inreload", false);
                inreload = false;
            }
            //Shotgun
            if(Inv.Shotgun_Selected == true && isshooting == false && Inv.slot1_mag_ammo > 0){
                isshooting = true;
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
                    Shotgunhits[i] = Physics2D.Raycast(raycastOrigin, raycastDirection, 10f);
                }
                //Damage
                foreach(RaycastHit2D hitinfoshotgun in Shotgunhits){
                    if(hitinfoshotgun){
                        if(hitinfoshotgun.collider.gameObject.tag == "Bot"){
                            hitinfoshotgun.collider.GetComponent<Bot_Health>().Damage(9, "Player", 8);
                        }else if(hitinfoshotgun.collider.gameObject.tag == "Player"){
                        hitinfoshotgun.collider.GetComponent<Player_Health>().Damage(9);
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
                    }else{
                        lineRenderer[x].SetPosition(0, Feuerpunkt.transform.position);
                        lineRenderer[x].SetPosition(1, new Vector2(Feuerpunkt.transform.position.x, Feuerpunkt.transform.position.y) + Directions[x] * 10f);
                        lineRenderer[x].SetPosition(1, new Vector3(lineRenderer[x].GetPosition(1).x, lineRenderer[x].GetPosition(1).y, 120f));
                    }
                    x++;
                }
                //Alle einmal anzeigen
                for(int i = 0; i < 10; i++){
                    lineRenderer[i].enabled = true;
                }

                //Wait one frame
                yield return new WaitForSeconds(0.05f);
                
                //Alle wieder ausblenden
                for(int i = 0; i < 10; i++){
                    lineRenderer[i].enabled = false;
                }
                Inv.slot1_mag_ammo--;
                yield return new WaitForSeconds(1f); 
                isshooting = false;
            }
            //Check ob Waffe Gewechselt werden muss
            if(Inv.Shotgun_Selected == true && Inv.slot1_mag_ammo == 0 && Inv.shotgun_ammo == 0 && isshooting == false){
            StartCoroutine(SwitchtonextWeapon());
            }
            //Reload Sotgun
            if(Inv.slot1_mag_ammo == 0 && !inreload && Inv.shotgun_ammo > 0 && Inv.Shotgun_Selected == true && isshooting == false){
                inreload = true;
                //Animation 
                //animator.SetBool("inreloadShotgun", true);
                //Reload
                while(Inv.Shotgun_Selected && Inv.slot1_mag_ammo < 5 && Inv.shotgun_ammo > 0 && Inv.Slot1_Selected){
                    yield return new WaitForSeconds(1.2f);
                    Inv.shotgun_ammo--;
                    Inv.slot1_mag_ammo++;
                }
                //animator.SetBool("inreloadShotgun", false);
                inreload = false;
            }
        }
        if(Inv.Slot2_Selected){
            //Glock-18
            if(Inv.Glock_18_Selected == true && isshooting == false && Inv.slot2_mag_ammo > 0){
                isshooting = true;
                //Raycast schuss
                RaycastHit2D hitinfo = Physics2D.Raycast(Feuerpunkt.transform.position, Feuerpunkt.transform.up, 50f); //Glock schießt 50f weit
                if(hitinfo){
                    if(hitinfo.collider.gameObject.tag == "Player"){
                        hitinfo.collider.GetComponent<Player_Health>().Damage(15);
                    }else if(hitinfo.collider.gameObject.tag == "Bot"){
                        hitinfo.collider.GetComponent<Bot_Health>().Damage(15, Mate.GetComponentInChildren<TextMeshPro>().text, 1);
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
                    lineRenderer[0].SetPosition(0, new Vector3(Feuerpunkt.transform.position.x, Feuerpunkt.transform.position.y, 120f));
                    lineRenderer[0].SetPosition(1, new Vector3(hitinfo.point.x, hitinfo.point.y, 120f));
                }else{
                    lineRenderer[0].SetPosition(0, Feuerpunkt.transform.position);
                    lineRenderer[0].SetPosition(1, Feuerpunkt.transform.position + Feuerpunkt.transform.up * 50f);
                    lineRenderer[0].SetPosition(1, new Vector3(lineRenderer[0].GetPosition(1).x, lineRenderer[0].GetPosition(1).y, 120f));
                }
                lineRenderer[0].enabled = true;
                //Wait one frame
                yield return new WaitForSeconds(0.05f);
                lineRenderer[0].enabled = false;
                Inv.slot2_mag_ammo--;
                yield return new WaitForSeconds(1f); 
                isshooting = false;
            }
            //Check ob Waffe Gewechselt werden muss
            if(Inv.Glock_18_Selected == true && Inv.slot2_mag_ammo == 0 && Inv.small_ammo == 0 && isshooting == false){
            StartCoroutine(SwitchtonextWeapon());
            }
            //Reload Glock-18
            if(Inv.slot2_mag_ammo == 0 && inreload == false && Inv.small_ammo > 0 && Inv.Glock_18_Selected == true && isshooting == false){
                inreload = true;
                Inv.small_ammo -= 12;
                if(Inv.small_ammo < 0){
                    Inv.small_ammo += 12;
                    Inv.slot2_mag_ammo = Inv.small_ammo;
                    Inv.small_ammo = 0;
                    yield return new WaitForSeconds (2.5f);
                    goto reloadend;
                }
                yield return new WaitForSeconds (2.5f);
                Inv.slot2_mag_ammo += 12;
                reloadend:
                inreload = false;
            }
            
            //Für Automatische Gewähre. (M4)
            if(Inv.M4_Selected == true && isshooting == false && Inv.slot2_mag_ammo > 0){
                isshooting = true;
                for( ; Inv.slot2_mag_ammo != 0 ; Inv.slot2_mag_ammo--){
                    //Raycast schuss
                    RaycastHit2D hitinfo = Physics2D.Raycast(Feuerpunkt.transform.position, Feuerpunkt.transform.up, 50f); //Glock schießt 50f weit
                    if(hitinfo){
                        if(hitinfo.collider.gameObject.tag == "Player"){
                        hitinfo.collider.GetComponent<Player_Health>().Damage(30);
                    }else if(hitinfo.collider.gameObject.tag == "Bot"){
                        hitinfo.collider.GetComponent<Bot_Health>().Damage(30, Mate.GetComponentInChildren<TextMeshPro>().text, 2);
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

                        lineRenderer[0].SetPosition(0, new Vector3(Feuerpunkt.transform.position.x, Feuerpunkt.transform.position.y, 120f));
                        lineRenderer[0].SetPosition(1, new Vector3(hitinfo.point.x, hitinfo.point.y, 120f));
                    }else{
                        lineRenderer[0].SetPosition(0, Feuerpunkt.transform.position);
                        lineRenderer[0].SetPosition(1, Feuerpunkt.transform.position + Feuerpunkt.transform.up * 50f);
                        lineRenderer[0].SetPosition(1, new Vector3(lineRenderer[0].GetPosition(1).x, lineRenderer[0].GetPosition(1).y, 120f));
                    }
                    lineRenderer[0].enabled = true;
                    //Wait one frame
                    yield return new WaitForSeconds(0.05f);
                    lineRenderer[0].enabled = false;
                    yield return new WaitForSecondsRealtime(0.3f);
                }
                isshooting = false;
            }
            //Check ob Waffe Gewechselt werden muss
            if(Inv.M4_Selected == true && Inv.slot2_mag_ammo == 0 && Inv.mid_ammo == 0 && isshooting == false){
            StartCoroutine(SwitchtonextWeapon());
            }
            //Reload M4
            if(Inv.slot2_mag_ammo == 0 && inreload == false && Inv.mid_ammo > 0 && Inv.M4_Selected == true && isshooting == false){
                inreload = true;
                Inv.mid_ammo -= 25;
                if(Inv.mid_ammo < 0){
                    Inv.mid_ammo += 25;
                    Inv.slot2_mag_ammo = Inv.mid_ammo; // RELOAD ZU SCHNELL
                    Inv.mid_ammo = 0;
                    yield return new WaitForSeconds (3.5f);
                    goto reloadend;
                }
                yield return new WaitForSeconds (3.5f);
                Inv.slot2_mag_ammo += 25;
                reloadend:
                inreload = false;
            }

            //Für Automatische Gewähre. (Ak47)
            if(Inv.Ak47_Selected == true && isshooting == false && Inv.slot2_mag_ammo > 0){
                isshooting = true;
                for( ; Inv.slot2_mag_ammo != 0 ; Inv.slot2_mag_ammo--){
                    //Raycast schuss
                    RaycastHit2D hitinfo = Physics2D.Raycast(Feuerpunkt.transform.position, Feuerpunkt.transform.up, 50f); //Glock schießt 50f weit
                    if(hitinfo){
                        if(hitinfo.collider.gameObject.tag == "Player"){
                        hitinfo.collider.GetComponent<Player_Health>().Damage(35);
                        }else if(hitinfo.collider.gameObject.tag == "Bot"){
                            hitinfo.collider.GetComponent<Bot_Health>().Damage(35, Mate.GetComponentInChildren<TextMeshPro>().text, 3);
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

                        lineRenderer[0].SetPosition(0, new Vector3(Feuerpunkt.transform.position.x, Feuerpunkt.transform.position.y, 120f));
                        lineRenderer[0].SetPosition(1, new Vector3(hitinfo.point.x, hitinfo.point.y, 120f));
                    }else{
                        lineRenderer[0].SetPosition(0, Feuerpunkt.transform.position);
                        lineRenderer[0].SetPosition(1, Feuerpunkt.transform.position + Feuerpunkt.transform.up * 50f);
                        lineRenderer[0].SetPosition(1, new Vector3(lineRenderer[0].GetPosition(1).x, lineRenderer[0].GetPosition(1).y, 120f));
                    }
                    lineRenderer[0].enabled = true;
                    //Wait one frame
                    yield return new WaitForSeconds(0.05f);
                    lineRenderer[0].enabled = false;
                    yield return new WaitForSecondsRealtime(0.3f);
                }
                isshooting = false;
            }
            //Check ob Waffe Gewechselt werden muss
            if(Inv.Ak47_Selected == true && Inv.slot2_mag_ammo == 0 && Inv.mid_ammo == 0 && isshooting == false){
            StartCoroutine(SwitchtonextWeapon());
            }
            //Reload Ak47
            if(Inv.slot2_mag_ammo == 0 && inreload == false && Inv.mid_ammo > 0 && Inv.Ak47_Selected == true && isshooting == false){
                inreload = true;
                Inv.mid_ammo -= 25;
                if(Inv.mid_ammo < 0){
                    Inv.mid_ammo += 25;
                    Inv.slot2_mag_ammo = Inv.mid_ammo;
                    Inv.mid_ammo = 0;
                    yield return new WaitForSeconds (3.5f);
                    goto reloadend;
                }
                yield return new WaitForSeconds (3.5f);
                Inv.slot2_mag_ammo += 25;
                reloadend:
                inreload = false;
            }

            //Sniper
            if(Inv.Sniper_Selected == true && isshooting == false && Inv.slot2_mag_ammo > 0){
                isshooting = true;
                //Raycast schuss
                RaycastHit2D hitinfo = Physics2D.Raycast(Feuerpunkt.transform.position, Feuerpunkt.transform.up, 150f); //Glock schießt 50f weit
                if(hitinfo){
                    if(hitinfo.collider.gameObject.tag == "Player"){
                        hitinfo.collider.GetComponent<Player_Health>().Damage(150);
                    }else if(hitinfo.collider.gameObject.tag == "Bot"){
                        hitinfo.collider.GetComponent<Bot_Health>().Damage(150, Mate.GetComponentInChildren<TextMeshPro>().text, 4);
                    }
                    
                    //Einschuss Animation (Blut)
                    if(hitinfo.collider.gameObject.tag == "Player" || hitinfo.collider.gameObject.tag == "Bot"){
                        Instantiate(Impactanimation, hitinfo.point, Quaternion.identity);
                    }
                    lineRenderer[0].SetPosition(0, new Vector3(Feuerpunkt.transform.position.x, Feuerpunkt.transform.position.y, 120f));
                    lineRenderer[0].SetPosition(1, new Vector3(hitinfo.point.x, hitinfo.point.y, 120f));
                }else{
                    lineRenderer[0].SetPosition(0, Feuerpunkt.transform.position);
                    lineRenderer[0].SetPosition(1, Feuerpunkt.transform.position + Feuerpunkt.transform.up * 150f);
                    lineRenderer[0].SetPosition(1, new Vector3(lineRenderer[0].GetPosition(1).x, lineRenderer[0].GetPosition(1).y, 120f));
                }
                lineRenderer[0].enabled = true;
                //Wait one frame
                yield return new WaitForSeconds(0.2f);
                lineRenderer[0].enabled = false;
                Inv.slot2_mag_ammo--;
                yield return new WaitForSeconds(4f); 
                isshooting = false;
            }
            //Check ob Waffe Gewechselt werden muss
            if(Inv.Sniper_Selected == true && Inv.slot2_mag_ammo == 0 && Inv.big_ammo == 0 && isshooting == false){
            StartCoroutine(SwitchtonextWeapon());
            }
            //Reload Sniper
            if(Inv.slot2_mag_ammo == 0 && inreload == false && Inv.big_ammo > 0 && Inv.Sniper_Selected == true && isshooting == false){
                inreload = true;
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
                inreload = false;
            }
            //Mp7
            if(Inv.Mp7_Selected == true && isshooting == false && Inv.slot2_mag_ammo > 0){
                isshooting = true;
                for( ; Inv.slot2_mag_ammo != 0 ; Inv.slot2_mag_ammo--){
                    //Raycast schuss
                    RaycastHit2D hitinfo = Physics2D.Raycast(Feuerpunkt.transform.position, Feuerpunkt.transform.up, 50f); //Glock schießt 50f weit
                    if(hitinfo){
                        if(hitinfo.collider.gameObject.tag == "Player"){
                        hitinfo.collider.GetComponent<Player_Health>().Damage(15);
                        }else if(hitinfo.collider.gameObject.tag == "Bot"){
                            hitinfo.collider.GetComponent<Bot_Health>().Damage(15, Mate.GetComponentInChildren<TextMeshPro>().text, 5);
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

                        lineRenderer[0].SetPosition(0, new Vector3(Feuerpunkt.transform.position.x, Feuerpunkt.transform.position.y, 120f));
                        lineRenderer[0].SetPosition(1, new Vector3(hitinfo.point.x, hitinfo.point.y, 120f));
                    }else{
                        lineRenderer[0].SetPosition(0, Feuerpunkt.transform.position);
                        lineRenderer[0].SetPosition(1, Feuerpunkt.transform.position + Feuerpunkt.transform.up * 50f);
                        lineRenderer[0].SetPosition(1, new Vector3(lineRenderer[0].GetPosition(1).x, lineRenderer[0].GetPosition(1).y, 120f));
                    }
                    lineRenderer[0].enabled = true;
                    //Wait one frame
                    yield return new WaitForSeconds(0.05f);
                    lineRenderer[0].enabled = false;
                    yield return new WaitForSecondsRealtime(0.09f);
                }
                isshooting = false;
            }
            //Check ob Waffe Gewechselt werden muss
            if(Inv.Mp7_Selected == true && Inv.slot2_mag_ammo == 0 && Inv.small_ammo == 0 && isshooting == false){
            StartCoroutine(SwitchtonextWeapon());
            }
            //Reload Mp7
            if(Inv.slot2_mag_ammo == 0 && inreload == false && Inv.small_ammo > 0 && Inv.Mp7_Selected == true && isshooting == false){
                inreload = true;
                //Animation 
                //animator.SetFloat("ReloadSpeed", 1f);
                //animator.SetBool("inreload", true);
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
                //animator.SetBool("inreload", false);
                inreload = false;
            }
            //Shotgun
            if(Inv.Shotgun_Selected == true && isshooting == false && Inv.slot2_mag_ammo > 0){
                isshooting = true;
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
                    Shotgunhits[i] = Physics2D.Raycast(raycastOrigin, raycastDirection, 10f);
                }
                //Damage
                foreach(RaycastHit2D hitinfoshotgun in Shotgunhits){
                    if(hitinfoshotgun){
                        if(hitinfoshotgun.collider.gameObject.tag == "Bot"){
                            hitinfoshotgun.collider.GetComponent<Bot_Health>().Damage(9, "Player", 8);
                        }else if(hitinfoshotgun.collider.gameObject.tag == "Player"){
                        hitinfoshotgun.collider.GetComponent<Player_Health>().Damage(9);
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
                    }else{
                        lineRenderer[x].SetPosition(0, Feuerpunkt.transform.position);
                        lineRenderer[x].SetPosition(1, new Vector2(Feuerpunkt.transform.position.x, Feuerpunkt.transform.position.y) + Directions[x] * 10f);
                        lineRenderer[x].SetPosition(1, new Vector3(lineRenderer[x].GetPosition(1).x, lineRenderer[x].GetPosition(1).y, 120f));
                    }
                    x++;
                }
                //Alle einmal anzeigen
                for(int i = 0; i < 10; i++){
                    lineRenderer[i].enabled = true;
                }

                //Wait one frame
                yield return new WaitForSeconds(0.05f);
                
                //Alle wieder ausblenden
                for(int i = 0; i < 10; i++){
                    lineRenderer[i].enabled = false;
                }
                Inv.slot2_mag_ammo--;
                yield return new WaitForSeconds(1f); 
                isshooting = false;
            }
            //Check ob Waffe Gewechselt werden muss
            if(Inv.Shotgun_Selected == true && Inv.slot2_mag_ammo == 0 && Inv.shotgun_ammo == 0 && isshooting == false){
            StartCoroutine(SwitchtonextWeapon());
            }
            //Reload Sotgun
            if(Inv.slot2_mag_ammo == 0 && !inreload && Inv.shotgun_ammo > 0 && Inv.Shotgun_Selected == true && isshooting == false){
                inreload = true;
                //Animation 
                //animator.SetBool("inreloadShotgun", true);
                //Reload
                while(Inv.Shotgun_Selected && Inv.slot2_mag_ammo < 5 && Inv.shotgun_ammo > 0 && Inv.Slot1_Selected){
                    yield return new WaitForSeconds(1.2f);
                    Inv.shotgun_ammo--;
                    Inv.slot2_mag_ammo++;
                }
                //animator.SetBool("inreloadShotgun", false);
                inreload = false;
            }
        }
        if(Inv.Slot3_Selected){
            //Glock-18
            if(Inv.Glock_18_Selected == true && isshooting == false && Inv.slot3_mag_ammo > 0){
                isshooting = true;
                //Raycast schuss
                RaycastHit2D hitinfo = Physics2D.Raycast(Feuerpunkt.transform.position, Feuerpunkt.transform.up, 50f); //Glock schießt 50f weit
                if(hitinfo){
                    if(hitinfo.collider.gameObject.tag == "Player"){
                        hitinfo.collider.GetComponent<Player_Health>().Damage(15);
                    }else if(hitinfo.collider.gameObject.tag == "Bot"){
                        hitinfo.collider.GetComponent<Bot_Health>().Damage(15, Mate.GetComponentInChildren<TextMeshPro>().text, 1);
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
                    lineRenderer[0].SetPosition(0, new Vector3(Feuerpunkt.transform.position.x, Feuerpunkt.transform.position.y, 120f));
                    lineRenderer[0].SetPosition(1, new Vector3(hitinfo.point.x, hitinfo.point.y, 120f));
                }else{
                    lineRenderer[0].SetPosition(0, Feuerpunkt.transform.position);
                    lineRenderer[0].SetPosition(1, Feuerpunkt.transform.position + Feuerpunkt.transform.up * 50f);
                    lineRenderer[0].SetPosition(1, new Vector3(lineRenderer[0].GetPosition(1).x, lineRenderer[0].GetPosition(1).y, 120f));
                }
                lineRenderer[0].enabled = true;
                //Wait one frame
                yield return new WaitForSeconds(0.05f);
                lineRenderer[0].enabled = false;
                Inv.slot3_mag_ammo--;
                yield return new WaitForSeconds(1f); 
                isshooting = false;
            }
            //Check ob Waffe Gewechselt werden muss
            if(Inv.Glock_18_Selected == true && Inv.slot3_mag_ammo == 0 && Inv.small_ammo == 0 && isshooting == false){
            StartCoroutine(SwitchtonextWeapon());
            }
            //Reload Glock-18
            if(Inv.slot3_mag_ammo == 0 && inreload == false && Inv.small_ammo > 0 && Inv.Glock_18_Selected == true && isshooting == false){
                inreload = true;
                Inv.small_ammo -= 12;
                if(Inv.small_ammo < 0){
                    Inv.small_ammo += 12;
                    Inv.slot3_mag_ammo = Inv.small_ammo;
                    Inv.small_ammo = 0;
                    yield return new WaitForSeconds (2.5f);
                    goto reloadend;
                }
                yield return new WaitForSeconds (2.5f);
                Inv.slot3_mag_ammo += 12;
                reloadend:
                inreload = false;
            }
            
            //Für Automatische Gewähre. (M4)
            if(Inv.M4_Selected == true && isshooting == false && Inv.slot3_mag_ammo > 0){
                isshooting = true;
                for( ; Inv.slot3_mag_ammo != 0 ; Inv.slot3_mag_ammo--){
                    //Raycast schuss
                    RaycastHit2D hitinfo = Physics2D.Raycast(Feuerpunkt.transform.position, Feuerpunkt.transform.up, 50f); //Glock schießt 50f weit
                    if(hitinfo){
                        if(hitinfo.collider.gameObject.tag == "Player"){
                        hitinfo.collider.GetComponent<Player_Health>().Damage(30);
                    }else if(hitinfo.collider.gameObject.tag == "Bot"){
                        hitinfo.collider.GetComponent<Bot_Health>().Damage(30, Mate.GetComponentInChildren<TextMeshPro>().text, 2);
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

                        lineRenderer[0].SetPosition(0, new Vector3(Feuerpunkt.transform.position.x, Feuerpunkt.transform.position.y, 120f));
                        lineRenderer[0].SetPosition(1, new Vector3(hitinfo.point.x, hitinfo.point.y, 120f));
                    }else{
                        lineRenderer[0].SetPosition(0, Feuerpunkt.transform.position);
                        lineRenderer[0].SetPosition(1, Feuerpunkt.transform.position + Feuerpunkt.transform.up * 50f);
                        lineRenderer[0].SetPosition(1, new Vector3(lineRenderer[0].GetPosition(1).x, lineRenderer[0].GetPosition(1).y, 120f));
                    }
                    lineRenderer[0].enabled = true;
                    //Wait one frame
                    yield return new WaitForSeconds(0.05f);
                    lineRenderer[0].enabled = false;
                    yield return new WaitForSecondsRealtime(0.3f);
                }
                isshooting = false;
            }
            //Check ob Waffe Gewechselt werden muss
            if(Inv.M4_Selected == true && Inv.slot3_mag_ammo == 0 && Inv.mid_ammo == 0 && isshooting == false){
            StartCoroutine(SwitchtonextWeapon());
            }
            //Reload M4
            if(Inv.slot3_mag_ammo == 0 && inreload == false && Inv.mid_ammo > 0 && Inv.M4_Selected == true && isshooting == false){
                inreload = true;
                Inv.mid_ammo -= 25;
                if(Inv.mid_ammo < 0){
                    Inv.mid_ammo += 25;
                    Inv.slot3_mag_ammo = Inv.mid_ammo; // RELOAD ZU SCHNELL
                    Inv.mid_ammo = 0;
                    yield return new WaitForSeconds (3.5f);
                    goto reloadend;
                }
                yield return new WaitForSeconds (3.5f);
                Inv.slot3_mag_ammo += 25;
                reloadend:
                inreload = false;
            }

            //Für Automatische Gewähre. (Ak47)
            if(Inv.Ak47_Selected == true && isshooting == false && Inv.slot3_mag_ammo > 0){
                isshooting = true;
                for( ; Inv.slot3_mag_ammo != 0 ; Inv.slot3_mag_ammo--){
                    //Raycast schuss
                    RaycastHit2D hitinfo = Physics2D.Raycast(Feuerpunkt.transform.position, Feuerpunkt.transform.up, 50f); //Glock schießt 50f weit
                    if(hitinfo){
                        if(hitinfo.collider.gameObject.tag == "Player"){
                        hitinfo.collider.GetComponent<Player_Health>().Damage(35);
                        }else if(hitinfo.collider.gameObject.tag == "Bot"){
                            hitinfo.collider.GetComponent<Bot_Health>().Damage(35, Mate.GetComponentInChildren<TextMeshPro>().text, 3);
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

                        lineRenderer[0].SetPosition(0, new Vector3(Feuerpunkt.transform.position.x, Feuerpunkt.transform.position.y, 120f));
                        lineRenderer[0].SetPosition(1, new Vector3(hitinfo.point.x, hitinfo.point.y, 120f));
                    }else{
                        lineRenderer[0].SetPosition(0, Feuerpunkt.transform.position);
                        lineRenderer[0].SetPosition(1, Feuerpunkt.transform.position + Feuerpunkt.transform.up * 50f);
                        lineRenderer[0].SetPosition(1, new Vector3(lineRenderer[0].GetPosition(1).x, lineRenderer[0].GetPosition(1).y, 120f));
                    }
                    lineRenderer[0].enabled = true;
                    //Wait one frame
                    yield return new WaitForSeconds(0.05f);
                    lineRenderer[0].enabled = false;
                    yield return new WaitForSecondsRealtime(0.3f);
                }
                isshooting = false;
            }
            //Check ob Waffe Gewechselt werden muss
            if(Inv.Ak47_Selected == true && Inv.slot3_mag_ammo == 0 && Inv.mid_ammo == 0 && isshooting == false){
            StartCoroutine(SwitchtonextWeapon());
            }
            //Reload Ak47
            if(Inv.slot3_mag_ammo == 0 && inreload == false && Inv.mid_ammo > 0 && Inv.Ak47_Selected == true && isshooting == false){
                inreload = true;
                Inv.mid_ammo -= 25;
                if(Inv.mid_ammo < 0){
                    Inv.mid_ammo += 25;
                    Inv.slot3_mag_ammo = Inv.mid_ammo;
                    Inv.mid_ammo = 0;
                    yield return new WaitForSeconds (3.5f);
                    goto reloadend;
                }
                yield return new WaitForSeconds (3.5f);
                Inv.slot3_mag_ammo += 25;
                reloadend:
                inreload = false;
            }

            //Sniper
            if(Inv.Sniper_Selected == true && isshooting == false && Inv.slot3_mag_ammo > 0){
                isshooting = true;
                //Raycast schuss
                RaycastHit2D hitinfo = Physics2D.Raycast(Feuerpunkt.transform.position, Feuerpunkt.transform.up, 150f); //Glock schießt 50f weit
                if(hitinfo){
                    if(hitinfo.collider.gameObject.tag == "Player"){
                        hitinfo.collider.GetComponent<Player_Health>().Damage(150);
                    }else if(hitinfo.collider.gameObject.tag == "Bot"){
                        hitinfo.collider.GetComponent<Bot_Health>().Damage(150, Mate.GetComponentInChildren<TextMeshPro>().text, 4);
                    }
                    
                    //Einschuss Animation (Blut)
                    if(hitinfo.collider.gameObject.tag == "Player" || hitinfo.collider.gameObject.tag == "Bot"){
                        Instantiate(Impactanimation, hitinfo.point, Quaternion.identity);
                    }
                    lineRenderer[0].SetPosition(0, new Vector3(Feuerpunkt.transform.position.x, Feuerpunkt.transform.position.y, 120f));
                    lineRenderer[0].SetPosition(1, new Vector3(hitinfo.point.x, hitinfo.point.y, 120f));
                }else{
                    lineRenderer[0].SetPosition(0, Feuerpunkt.transform.position);
                    lineRenderer[0].SetPosition(1, Feuerpunkt.transform.position + Feuerpunkt.transform.up * 150f);
                    lineRenderer[0].SetPosition(1, new Vector3(lineRenderer[0].GetPosition(1).x, lineRenderer[0].GetPosition(1).y, 120f));
                }
                lineRenderer[0].enabled = true;
                //Wait one frame
                yield return new WaitForSeconds(0.2f);
                lineRenderer[0].enabled = false;
                Inv.slot3_mag_ammo--;
                yield return new WaitForSeconds(4f); 
                isshooting = false;
            }
            //Check ob Waffe Gewechselt werden muss
            if(Inv.Sniper_Selected == true && Inv.slot3_mag_ammo == 0 && Inv.big_ammo == 0 && isshooting == false){
            StartCoroutine(SwitchtonextWeapon());
            }
            //Reload Sniper
            if(Inv.slot3_mag_ammo == 0 && inreload == false && Inv.big_ammo > 0 && Inv.Sniper_Selected == true && isshooting == false){
                inreload = true;
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
                inreload = false;
            }
            //Mp7
            if(Inv.Mp7_Selected == true && isshooting == false && Inv.slot3_mag_ammo > 0){
                isshooting = true;
                for( ; Inv.slot3_mag_ammo != 0 ; Inv.slot3_mag_ammo--){
                    //Raycast schuss
                    RaycastHit2D hitinfo = Physics2D.Raycast(Feuerpunkt.transform.position, Feuerpunkt.transform.up, 50f); //Glock schießt 50f weit
                    if(hitinfo){
                        if(hitinfo.collider.gameObject.tag == "Player"){
                        hitinfo.collider.GetComponent<Player_Health>().Damage(15);
                        }else if(hitinfo.collider.gameObject.tag == "Bot"){
                            hitinfo.collider.GetComponent<Bot_Health>().Damage(15, Mate.GetComponentInChildren<TextMeshPro>().text, 5);
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

                        lineRenderer[0].SetPosition(0, new Vector3(Feuerpunkt.transform.position.x, Feuerpunkt.transform.position.y, 120f));
                        lineRenderer[0].SetPosition(1, new Vector3(hitinfo.point.x, hitinfo.point.y, 120f));
                    }else{
                        lineRenderer[0].SetPosition(0, Feuerpunkt.transform.position);
                        lineRenderer[0].SetPosition(1, Feuerpunkt.transform.position + Feuerpunkt.transform.up * 50f);
                        lineRenderer[0].SetPosition(1, new Vector3(lineRenderer[0].GetPosition(1).x, lineRenderer[0].GetPosition(1).y, 120f));
                    }
                    lineRenderer[0].enabled = true;
                    //Wait one frame
                    yield return new WaitForSeconds(0.05f);
                    lineRenderer[0].enabled = false;
                    yield return new WaitForSecondsRealtime(0.09f);
                }
                isshooting = false;
            }
            //Check ob Waffe Gewechselt werden muss
            if(Inv.Mp7_Selected == true && Inv.slot3_mag_ammo == 0 && Inv.small_ammo == 0 && isshooting == false){
            StartCoroutine(SwitchtonextWeapon());
            }
            //Reload Mp7
            if(Inv.slot3_mag_ammo == 0 && inreload == false && Inv.small_ammo > 0 && Inv.Mp7_Selected == true && isshooting == false){
                inreload = true;
                //Animation 
                //animator.SetFloat("ReloadSpeed", 1f);
                //animator.SetBool("inreload", true);
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
                //animator.SetBool("inreload", false);
                inreload = false;
            }
            //Shotgun
            if(Inv.Shotgun_Selected == true && isshooting == false && Inv.slot3_mag_ammo > 0){
                isshooting = true;
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
                    Shotgunhits[i] = Physics2D.Raycast(raycastOrigin, raycastDirection, 10f);
                }
                //Damage
                foreach(RaycastHit2D hitinfoshotgun in Shotgunhits){
                    if(hitinfoshotgun){
                        if(hitinfoshotgun.collider.gameObject.tag == "Bot"){
                            hitinfoshotgun.collider.GetComponent<Bot_Health>().Damage(9, "Player", 8);
                        }else if(hitinfoshotgun.collider.gameObject.tag == "Player"){
                        hitinfoshotgun.collider.GetComponent<Player_Health>().Damage(9);
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
                    }else{
                        lineRenderer[x].SetPosition(0, Feuerpunkt.transform.position);
                        lineRenderer[x].SetPosition(1, new Vector2(Feuerpunkt.transform.position.x, Feuerpunkt.transform.position.y) + Directions[x] * 10f);
                        lineRenderer[x].SetPosition(1, new Vector3(lineRenderer[x].GetPosition(1).x, lineRenderer[x].GetPosition(1).y, 120f));
                    }
                    x++;
                }
                //Alle einmal anzeigen
                for(int i = 0; i < 10; i++){
                    lineRenderer[i].enabled = true;
                }

                //Wait one frame
                yield return new WaitForSeconds(0.05f);
                
                //Alle wieder ausblenden
                for(int i = 0; i < 10; i++){
                    lineRenderer[i].enabled = false;
                }
                Inv.slot3_mag_ammo--;
                yield return new WaitForSeconds(1f); 
                isshooting = false;
            }
            //Check ob Waffe Gewechselt werden muss
            if(Inv.Shotgun_Selected == true && Inv.slot3_mag_ammo == 0 && Inv.shotgun_ammo == 0 && isshooting == false){
            StartCoroutine(SwitchtonextWeapon());
            }
            //Reload Sotgun
            if(Inv.slot3_mag_ammo == 0 && !inreload && Inv.shotgun_ammo > 0 && Inv.Shotgun_Selected == true && isshooting == false){
                inreload = true;
                //Animation 
                //animator.SetBool("inreloadShotgun", true);
                //Reload
                while(Inv.Shotgun_Selected && Inv.slot3_mag_ammo < 5 && Inv.shotgun_ammo > 0 && Inv.Slot1_Selected){
                    yield return new WaitForSeconds(1.2f);
                    Inv.shotgun_ammo--;
                    Inv.slot3_mag_ammo++;
                }
                //animator.SetBool("inreloadShotgun", false);
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
    public IEnumerator SwitchtonextWeapon(){
        yield return new WaitForSeconds(.8f);
        Inv.DecidenextWeapon();
    }
}
