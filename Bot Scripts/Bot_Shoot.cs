using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bot_Shoot : MonoBehaviour
{
    Bot_Inventory Inv;
    public GameObject Bot; //Bot
    public GameObject Bullet;
    public GameObject Feuerpunkt; 
    public bool isshooting;
    public bool inreload;
    [SerializeField] private GameObject Impactanimation;
    [SerializeField] private LineRenderer lineRenderer;
    private Bot_Behavior bot_behavior;
    // Start is called before the first frame update
    void Awake()
    {
        Inv = Bot.GetComponent<Bot_Inventory>();
        bot_behavior = Bot.GetComponent<Bot_Behavior>();
    }

    // Update is called once per frame

    public IEnumerator Shoot(){
        //Glock-18
        if(Inv.Glock_18_Selected == true && isshooting == false && Inv.slot1_mag_ammo > 0){
            isshooting = true;
            //Raycast schuss
            RaycastHit2D hitinfo = Physics2D.Raycast(Feuerpunkt.transform.position, Feuerpunkt.transform.up, 50f); //Glock schießt 50f weit
            if(hitinfo){
                if(hitinfo.collider.gameObject.tag == "Player"){
                    hitinfo.collider.gameObject.GetComponent<Player_Health>().health -= 15; //Player nimmt 15 Schaden
                }else if(hitinfo.collider.gameObject.tag == "Bot"){
                    hitinfo.collider.gameObject.GetComponent<Bot_Health>().health -= 15; //Bot nimmt 15 Schaden
                }
                
                //Einschuss Animation (Blut)
                if(hitinfo.collider.gameObject.tag == "Player" || hitinfo.collider.gameObject.tag == "Bot"){
                    Instantiate(Impactanimation, hitinfo.point, Quaternion.identity);
                }
                lineRenderer.SetPosition(0, Feuerpunkt.transform.position);
                lineRenderer.SetPosition(1, hitinfo.point);
            }else{
                lineRenderer.SetPosition(0, Feuerpunkt.transform.position);
                lineRenderer.SetPosition(1, Feuerpunkt.transform.position + Feuerpunkt.transform.up * 50f);
            }
            lineRenderer.enabled = true;
            //Wait one frame
            yield return new WaitForSeconds(0.05f);
            lineRenderer.enabled = false;
            Inv.slot1_mag_ammo--;
            yield return new WaitForSeconds(1f); 
            isshooting = false;
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
                        hitinfo.collider.gameObject.GetComponent<Player_Health>().health -= 20; //Player nimmt 15 Schaden
                    }else if(hitinfo.collider.gameObject.tag == "Bot"){
                        hitinfo.collider.gameObject.GetComponent<Bot_Health>().health -= 20; //Bot nimmt 15 Schaden
                    }
                
                    //Einschuss Animation (Blut)
                    if(hitinfo.collider.gameObject.tag == "Player" || hitinfo.collider.gameObject.tag == "Bot"){
                        Instantiate(Impactanimation, hitinfo.point, Quaternion.identity);
                    }
                    lineRenderer.SetPosition(0, Feuerpunkt.transform.position);
                    lineRenderer.SetPosition(1, hitinfo.point);
                }else{
                    lineRenderer.SetPosition(0, Feuerpunkt.transform.position);
                    lineRenderer.SetPosition(1, Feuerpunkt.transform.position + Feuerpunkt.transform.up * 50f);
                }
                lineRenderer.enabled = true;
                //Wait one frame
                yield return new WaitForSeconds(0.05f);
                lineRenderer.enabled = false;
                yield return new WaitForSecondsRealtime(0.3f);
            }
            isshooting = false;
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
                        hitinfo.collider.gameObject.GetComponent<Player_Health>().health -= 25; //Player nimmt 15 Schaden
                    }else if(hitinfo.collider.gameObject.tag == "Bot"){
                        hitinfo.collider.gameObject.GetComponent<Bot_Health>().health -= 25; //Bot nimmt 15 Schaden
                    }
                
                    //Einschuss Animation (Blut)
                    if(hitinfo.collider.gameObject.tag == "Player" || hitinfo.collider.gameObject.tag == "Bot"){
                        Instantiate(Impactanimation, hitinfo.point, Quaternion.identity);
                    }
                    lineRenderer.SetPosition(0, Feuerpunkt.transform.position);
                    lineRenderer.SetPosition(1, hitinfo.point);
                }else{
                    lineRenderer.SetPosition(0, Feuerpunkt.transform.position);
                    lineRenderer.SetPosition(1, Feuerpunkt.transform.position + Feuerpunkt.transform.up * 50f);
                }
                lineRenderer.enabled = true;
                //Wait one frame
                yield return new WaitForSeconds(0.05f);
                lineRenderer.enabled = false;
                yield return new WaitForSecondsRealtime(0.3f);
            }
            isshooting = false;
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
                    hitinfo.collider.gameObject.GetComponent<Player_Health>().health -= 150; //Player nimmt 150 Schaden
                }else if(hitinfo.collider.gameObject.tag == "Bot"){
                    hitinfo.collider.gameObject.GetComponent<Bot_Health>().health -= 150; //Bot nimmt 150 Schaden
                }
                
                //Einschuss Animation (Blut)
                if(hitinfo.collider.gameObject.tag == "Player" || hitinfo.collider.gameObject.tag == "Bot"){
                    Instantiate(Impactanimation, hitinfo.point, Quaternion.identity);
                }
                lineRenderer.SetPosition(0, Feuerpunkt.transform.position);
                lineRenderer.SetPosition(1, hitinfo.point);
            }else{
                lineRenderer.SetPosition(0, Feuerpunkt.transform.position);
                lineRenderer.SetPosition(1, Feuerpunkt.transform.position + Feuerpunkt.transform.up * 150f);
            }
            lineRenderer.enabled = true;
            //Wait one frame
            yield return new WaitForSeconds(0.2f);
            lineRenderer.enabled = false;
            Inv.slot1_mag_ammo--;
            yield return new WaitForSeconds(4f); 
            isshooting = false;
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
    }
}
