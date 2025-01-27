using System.Collections;
using System.Collections.Generic;
//using UnityEditorInternal;
using UnityEngine;

public class Bot_Inventory : MonoBehaviour
{
    public string Slot1_Item;
    public string Slot2_Item;
    public string Slot3_Item;
    public bool Slot1_Selected;
    public bool Slot2_Selected;
    public bool Slot3_Selected;
    public bool Slot1, Slot2, Slot3;
    public int small_ammo;
    public int mid_ammo;
    public int big_ammo;
    public int shotgun_ammo;
    public int slot1_mag_ammo, slot2_mag_ammo, slot3_mag_ammo;
    public int lootcount, lootcount2, Player_Heal, Player_Heal2;
    public bool Glock_18_Selected, M4_Selected, Ak47_Selected, Sniper_Selected, Mp7_Selected, Shotgun_Selected;
    public GameObject Bot;
    public GameObject Glock_18_Top_Sprite, Ak47_Top_Sprite, M4_Top_Sprite, Sniper_Top_Sprite, Mp7_Top_Sprite, Shotgun_Top_Sprite;
    public Bot_Behavior behavior;
    
    void Awake(){
        int rand = Random.Range(0, 2);
        if(rand == 1){ // 50% chance
            Slot1 = true;
            Slot1_Item = "Glock_18"; //Bot bekommt zum Start eine Glock
            slot1_mag_ammo = 25;
            Slot1_Selected = true;
            lootcount++;
            lootcount2++;
            //Wähle ersten Slot aus
            ChangeSelectedSlot(1);
            behavior.forceflee = false;
            return;
        }else if(Random.Range(0, 15) == 1){
            //Give the bot an Ak with an ammo of 30 (10% Chance)
            Slot1 = true;
            Slot1_Item = "Ak47";
            slot1_mag_ammo = 30;
            Slot1_Selected = true;
            lootcount++;
            lootcount2++;
            //Wähle ersten Slot aus
            ChangeSelectedSlot(1);
            behavior.forceflee = false;
        }else if(Random.Range(0, 15) == 1){
            //Give the bot an M4 with an ammo of 30 (10% Chance)
            Slot1 = true;
            Slot1_Item = "M4";
            slot1_mag_ammo = 30;
            Slot1_Selected = true;
            lootcount++;
            lootcount2++;
            //Wähle ersten Slot aus
            ChangeSelectedSlot(1);
            behavior.forceflee = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        Weapon_Visibility();
    }

    void LateUpdate() {
        lootcount = lootcount2; //Ist dazu da den Lootcount ein wenig später zu aktualisieren damit langsame handys keine Probleme beim Lootcount haben
        Player_Heal = Player_Heal2; 
    }
    
    private void OnTriggerEnter2D(Collider2D collision) {
        //Slot1
        if(Slot1 == false){
        if(collision.gameObject.tag == "Glock_18" && lootcount2 <= 3){
            Slot1_Item = "Glock_18";
            Slot1 = true;
            slot1_mag_ammo = collision.gameObject.GetComponent<Weapon_Info>().Currentammo;
            
        }else if(collision.gameObject.tag == "M4" && lootcount2 <= 3){
            Slot1_Item = "M4";
            Slot1 = true;
            slot1_mag_ammo = collision.gameObject.GetComponent<Weapon_Info>().Currentammo;
        }else if(collision.gameObject.tag == "AK_47" && lootcount2 <= 3){
            Slot1_Item = "Ak47";
            Slot1 = true;
            slot1_mag_ammo = collision.gameObject.GetComponent<Weapon_Info>().Currentammo;
        }else if(collision.gameObject.tag == "Sniper" && lootcount2 <= 3){
            Slot1_Item = "Sniper";
            Slot1 = true;
            slot1_mag_ammo = collision.gameObject.GetComponent<Weapon_Info>().Currentammo;
        }
        else if(collision.gameObject.tag == "Mp7" && lootcount2 <= 3){
            Slot1_Item = "Mp7";
            Slot1 = true;
            slot1_mag_ammo = collision.gameObject.GetComponent<Weapon_Info>().Currentammo;
        }
        else if(collision.gameObject.tag == "Shotgun" && lootcount2 <= 3){
            Slot1_Item = "Shotgun";
            Slot1 = true;
            slot1_mag_ammo = collision.gameObject.GetComponent<Weapon_Info>().Currentammo;
        }
        }
        //Slot2
        else if(Slot2 == false && Slot1 == true){
        if(collision.gameObject.tag == "Glock_18" && lootcount2 <= 3){
            Slot2_Item = "Glock_18";
            Slot2 = true;
            slot2_mag_ammo = collision.gameObject.GetComponent<Weapon_Info>().Currentammo;
        } else if(collision.gameObject.tag == "M4" && lootcount2 <= 3){
            Slot2_Item = "M4";
            Slot2 = true;
            slot2_mag_ammo = collision.gameObject.GetComponent<Weapon_Info>().Currentammo;
        }else if(collision.gameObject.tag == "AK_47" && lootcount2 <= 3){
            Slot2_Item = "Ak47";
            Slot2 = true;
            slot2_mag_ammo = collision.gameObject.GetComponent<Weapon_Info>().Currentammo;
        }else if(collision.gameObject.tag == "Sniper" && lootcount2 <= 3){
            Slot2_Item = "Sniper";
            Slot2 = true;
            slot2_mag_ammo = collision.gameObject.GetComponent<Weapon_Info>().Currentammo;
        }
        else if(collision.gameObject.tag == "Mp7" && lootcount2 <= 3){
            Slot2_Item = "Mp7";
            Slot2 = true;
            slot2_mag_ammo = collision.gameObject.GetComponent<Weapon_Info>().Currentammo;
        }
        else if(collision.gameObject.tag == "Shotgun" && lootcount2 <= 3){
            Slot2_Item = "Shotgun";
            Slot2 = true;
            slot2_mag_ammo = collision.gameObject.GetComponent<Weapon_Info>().Currentammo;
        }
        }
        //Slot3
        else if(Slot3 == false && Slot2 == true && Slot1 == true){
        if(collision.gameObject.tag == "Glock_18" && lootcount2 <= 3){
            Slot3_Item = "Glock_18";
            Slot3 = true;
            slot3_mag_ammo = collision.gameObject.GetComponent<Weapon_Info>().Currentammo;
        } else if(collision.gameObject.tag == "M4" && lootcount2 <= 3){
            Slot3_Item = "M4";
            Slot3 = true;
            slot3_mag_ammo = collision.gameObject.GetComponent<Weapon_Info>().Currentammo;
        }else if(collision.gameObject.tag == "AK_47" && lootcount2 <= 3){
            Slot3_Item = "Ak47";
            Slot3 = true;
            slot3_mag_ammo = collision.gameObject.GetComponent<Weapon_Info>().Currentammo;
        }else if(collision.gameObject.tag == "Sniper" && lootcount2 <= 3){
            Slot3_Item = "Sniper";
            Slot3 = true;
            slot3_mag_ammo = collision.gameObject.GetComponent<Weapon_Info>().Currentammo;
        }
        else if(collision.gameObject.tag == "Mp7" && lootcount2 <= 3){
            Slot3_Item = "Mp7";
            Slot3 = true;
            slot3_mag_ammo = collision.gameObject.GetComponent<Weapon_Info>().Currentammo;
        }
        else if(collision.gameObject.tag == "Shotgun" && lootcount2 <= 3){
            Slot3_Item = "Shotgun";
            Slot3 = true;
            slot3_mag_ammo = collision.gameObject.GetComponent<Weapon_Info>().Currentammo;
        }
        }
        
        //Lootcount
        if(collision.gameObject.tag == "Glock_18" && lootcount < 3){
            lootcount2 += 1;
        }
        else if(collision.gameObject.tag == "M4" && lootcount < 3){
            lootcount2 += 1;
        }
        else if(collision.gameObject.tag == "AK_47" && lootcount < 3){
            lootcount2 += 1;
        }
        else if(collision.gameObject.tag == "Sniper" && lootcount < 3){
            lootcount2 += 1;
        }
        else if(collision.gameObject.tag == "Mp7" && lootcount < 3){
            lootcount2 += 1;
        }
        else if(collision.gameObject.tag == "Shotgun" && lootcount < 3){
            lootcount2 += 1;
        }
        else if(collision.gameObject.tag == "Heal" && Player_Heal <= 4){
            Player_Heal2 += 1;
        }

        if(collision.gameObject.tag == "Small_Ammo"){
            small_ammo += collision.gameObject.GetComponent<Ammo_Info>().Ammo;
        }
        if(collision.gameObject.tag == "Mid_Ammo"){
            mid_ammo += collision.gameObject.GetComponent<Ammo_Info>().Ammo;
        }
        if(collision.gameObject.tag == "Big_Ammo"){
            big_ammo += collision.gameObject.GetComponent<Ammo_Info>().Ammo;
        }
        if(collision.gameObject.tag == "Shotgun_Ammo"){
            shotgun_ammo += collision.gameObject.GetComponent<Ammo_Info>().Ammo;
        }
        DecidenextWeapon();
    }

    public void DecidenextWeapon(){
        //1.Checke ob Waffen noch beldaden sind
        if(slot1_mag_ammo > 0){
            ChangeSelectedSlot(1);
            behavior.forceflee = false;
            return;
        }else if(slot2_mag_ammo > 0){
            ChangeSelectedSlot(2);
            behavior.forceflee = false;
            return;
        }else if(slot3_mag_ammo > 0){
            ChangeSelectedSlot(3);
            behavior.forceflee = false;
            return;
        }
        //2.Checke Welche Waffentypen vorhanden sind und schaue wo reloaded werden kann
        if(Slot1_Item != null){
            if(Slot1_Item == "Glock_18" || Slot1_Item == "Mp7"){
                if(small_ammo > 0){
                    ChangeSelectedSlot(1);
                    behavior.forceflee = false;
                    return;
                }
            }else if(Slot1_Item == "M4" || Slot1_Item == "Ak47"){
                if(mid_ammo > 0){
                    ChangeSelectedSlot(1);
                    behavior.forceflee = false;
                    return;
                }
            }else if(Slot1_Item == "Sniper"){
                if(big_ammo > 0){
                    ChangeSelectedSlot(1);
                    behavior.forceflee = false;
                    return;
                }
            }else if(Slot1_Item == "Shotgun"){
                if(shotgun_ammo > 0){
                    ChangeSelectedSlot(1);
                    behavior.forceflee = false;
                    return;
                }
            }
        }
        if(Slot2_Item != null){
            if(Slot2_Item == "Glock_18" || Slot2_Item == "Mp7"){
                if(small_ammo > 0){
                    ChangeSelectedSlot(2);
                    behavior.forceflee = false;
                    return;
                }
            }else if(Slot2_Item == "M4" || Slot2_Item == "Ak47"){
                if(mid_ammo > 0){
                    ChangeSelectedSlot(2);
                    behavior.forceflee = false;
                    return;
                }
            }else if(Slot2_Item == "Sniper"){
                if(big_ammo > 0){
                    ChangeSelectedSlot(2);
                    behavior.forceflee = false;
                    return;
                }
            }else if(Slot2_Item == "Shotgun"){
                if(shotgun_ammo > 0){
                    ChangeSelectedSlot(2);
                    behavior.forceflee = false;
                    return;
                }
            }
        }
        if(Slot3_Item != null){
            if(Slot3_Item == "Glock_18" || Slot3_Item == "Mp7"){
                if(small_ammo > 0){
                    ChangeSelectedSlot(3);
                    behavior.forceflee = false;
                    return;
                }
            }else if(Slot3_Item == "M4" || Slot3_Item == "Ak47"){
                if(mid_ammo > 0){
                    ChangeSelectedSlot(3);
                    behavior.forceflee = false;
                    return;
                }
            }else if(Slot3_Item == "Sniper"){
                if(big_ammo > 0){
                    ChangeSelectedSlot(3);
                    behavior.forceflee = false;
                    return;
                }
            }else if(Slot3_Item == "Shotgun"){
                if(shotgun_ammo > 0){
                    ChangeSelectedSlot(1);
                    behavior.forceflee = false;
                    return;
                }
            }
        }
        //Wenn keine entscheidung getroffen werden konnte setze forceflee auf true
        behavior.forceflee = true;
    }
    public void ChangeSelectedSlot(int num){
        if(num == 1){
            Slot1_Selected = true;
            Slot2_Selected = false;
            Slot3_Selected = false;
        }else if(num == 2){
            Slot1_Selected = false;
            Slot2_Selected = true;
            Slot3_Selected = false;
        }else if(num == 3){
            Slot1_Selected = false;
            Slot2_Selected = false;
            Slot3_Selected = true;
        }
    }

    void Weapon_Visibility(){
        if(Slot1_Selected == true){
        if(Slot1_Item == "Glock_18"){
            //Aktive Waffe
            Glock_18_Top_Sprite.transform.gameObject.SetActive(true);
            Glock_18_Selected = true;
            //Alle anderen Deaktivieren
            M4_Top_Sprite.transform.gameObject.SetActive(false);
            M4_Selected = false;
            Ak47_Top_Sprite.transform.gameObject.SetActive(false);
            Ak47_Selected = false;
            Sniper_Top_Sprite.transform.gameObject.SetActive(false);
            Sniper_Selected = false;
            Mp7_Top_Sprite.transform.gameObject.SetActive(false);
            Mp7_Selected = false;
            Shotgun_Top_Sprite.transform.gameObject.SetActive(false);
            Shotgun_Selected = false;
        }else if(Slot1_Item == "M4"){
            //Aktive Waffe
            M4_Top_Sprite.transform.gameObject.SetActive(true);
            M4_Selected = true;
            //Alle anderen Deaktivieren
            Glock_18_Top_Sprite.transform.gameObject.SetActive(false);
            Glock_18_Selected = false;
            Ak47_Top_Sprite.transform.gameObject.SetActive(false);
            Ak47_Selected = false;
            Sniper_Top_Sprite.transform.gameObject.SetActive(false);
            Sniper_Selected = false;
            Mp7_Top_Sprite.transform.gameObject.SetActive(false);
            Mp7_Selected = false;
            Shotgun_Top_Sprite.transform.gameObject.SetActive(false);
            Shotgun_Selected = false;
        }else if(Slot1_Item == "Ak47"){
            //Aktive Waffe
            Ak47_Top_Sprite.transform.gameObject.SetActive(true);
            Ak47_Selected = true;
            //Alle anderen Deaktivieren
            M4_Top_Sprite.transform.gameObject.SetActive(false);
            M4_Selected = false;
            Glock_18_Top_Sprite.transform.gameObject.SetActive(false);
            Glock_18_Selected = false;
            Sniper_Top_Sprite.transform.gameObject.SetActive(false);
            Sniper_Selected = false;
            Mp7_Top_Sprite.transform.gameObject.SetActive(false);
            Mp7_Selected = false;
            Shotgun_Top_Sprite.transform.gameObject.SetActive(false);
            Shotgun_Selected = false;
        }else if(Slot1_Item == "Sniper"){
            //Aktive Waffe
            Sniper_Top_Sprite.transform.gameObject.SetActive(true);
            Sniper_Selected = true;
            //Alle anderen Deaktivieren
            M4_Top_Sprite.transform.gameObject.SetActive(false);
            M4_Selected = false;
            Ak47_Top_Sprite.transform.gameObject.SetActive(false);
            Ak47_Selected = false;
            Glock_18_Top_Sprite.transform.gameObject.SetActive(false);
            Glock_18_Selected = false;
            Mp7_Top_Sprite.transform.gameObject.SetActive(false);
            Mp7_Selected = false;
            Shotgun_Top_Sprite.transform.gameObject.SetActive(false);
            Shotgun_Selected = false;
        }else if(Slot1_Item == "Mp7"){
            //Aktive Waffe 
            Mp7_Top_Sprite.transform.gameObject.SetActive(true);
            Mp7_Selected = true;
            //Alle anderen Deaktivieren
            Sniper_Top_Sprite.transform.gameObject.SetActive(false);
            Sniper_Selected = false;
            M4_Top_Sprite.transform.gameObject.SetActive(false);
            M4_Selected = false;
            Ak47_Top_Sprite.transform.gameObject.SetActive(false);
            Ak47_Selected = false;
            Glock_18_Top_Sprite.transform.gameObject.SetActive(false);
            Glock_18_Selected = false;
            Shotgun_Top_Sprite.transform.gameObject.SetActive(false);
            Shotgun_Selected = false;
        }else if(Slot1_Item == "Shotgun"){ 
            //Aktive Waffe 
            Shotgun_Top_Sprite.transform.gameObject.SetActive(true);
            Shotgun_Selected = true;
            //Alle anderen Deaktivieren
            Sniper_Top_Sprite.transform.gameObject.SetActive(false);
            Sniper_Selected = false;
            M4_Top_Sprite.transform.gameObject.SetActive(false);
            M4_Selected = false;
            Ak47_Top_Sprite.transform.gameObject.SetActive(false);
            Ak47_Selected = false;
            Glock_18_Top_Sprite.transform.gameObject.SetActive(false);
            Glock_18_Selected = false;
            Mp7_Top_Sprite.transform.gameObject.SetActive(false);
            Mp7_Selected = false;
        }else if(Slot1_Item == ""){
            //Alle anderen Deaktivieren
            Sniper_Top_Sprite.transform.gameObject.SetActive(false);
            Sniper_Selected = false;
            M4_Top_Sprite.transform.gameObject.SetActive(false);
            M4_Selected = false;
            Ak47_Top_Sprite.transform.gameObject.SetActive(false);
            Ak47_Selected = false;
            Glock_18_Top_Sprite.transform.gameObject.SetActive(false);
            Glock_18_Selected = false;
            Mp7_Top_Sprite.transform.gameObject.SetActive(false);
            Mp7_Selected = false;
            Shotgun_Top_Sprite.transform.gameObject.SetActive(false);
            Shotgun_Selected = false;
        }
        }

        if(Slot2_Selected == true){
        if(Slot2_Item == "Glock_18"){
            //Aktive Waffe
            Glock_18_Top_Sprite.transform.gameObject.SetActive(true);
            Glock_18_Selected = true;
            //Alle anderen Deaktivieren
            M4_Top_Sprite.transform.gameObject.SetActive(false);
            M4_Selected = false;
            Ak47_Top_Sprite.transform.gameObject.SetActive(false);
            Ak47_Selected = false;
            Sniper_Top_Sprite.transform.gameObject.SetActive(false);
            Sniper_Selected = false;
            Mp7_Top_Sprite.transform.gameObject.SetActive(false);
            Mp7_Selected = false;
            Shotgun_Top_Sprite.transform.gameObject.SetActive(false);
            Shotgun_Selected = false;
        }else if(Slot2_Item == "M4"){
            //Aktive Waffe
            M4_Top_Sprite.transform.gameObject.SetActive(true);
            M4_Selected = true;
            //Alle anderen Deaktivieren
            Glock_18_Top_Sprite.transform.gameObject.SetActive(false);
            Glock_18_Selected = false;
            Ak47_Top_Sprite.transform.gameObject.SetActive(false);
            Ak47_Selected = false;
            Sniper_Top_Sprite.transform.gameObject.SetActive(false);
            Sniper_Selected = false;
            Mp7_Top_Sprite.transform.gameObject.SetActive(false);
            Mp7_Selected = false;
            Shotgun_Top_Sprite.transform.gameObject.SetActive(false);
            Shotgun_Selected = false;
        }else if(Slot2_Item == "Ak47"){
            //Aktive Waffe
            Ak47_Top_Sprite.transform.gameObject.SetActive(true);
            Ak47_Selected = true;
            //Alle anderen Deaktivieren
            M4_Top_Sprite.transform.gameObject.SetActive(false);
            M4_Selected = false;
            Glock_18_Top_Sprite.transform.gameObject.SetActive(false);
            Glock_18_Selected = false;
            Sniper_Top_Sprite.transform.gameObject.SetActive(false);
            Sniper_Selected = false;
            Mp7_Top_Sprite.transform.gameObject.SetActive(false);
            Mp7_Selected = false;
            Shotgun_Top_Sprite.transform.gameObject.SetActive(false);
            Shotgun_Selected = false;
        }else if(Slot2_Item == "Mp7"){
            //Aktive Waffe 
            Mp7_Top_Sprite.transform.gameObject.SetActive(true);
            Mp7_Selected = true;
            //Alle anderen Deaktivieren
            Sniper_Top_Sprite.transform.gameObject.SetActive(false);
            Sniper_Selected = false;
            M4_Top_Sprite.transform.gameObject.SetActive(false);
            M4_Selected = false;
            Ak47_Top_Sprite.transform.gameObject.SetActive(false);
            Ak47_Selected = false;
            Glock_18_Top_Sprite.transform.gameObject.SetActive(false);
            Glock_18_Selected = false;
            Shotgun_Top_Sprite.transform.gameObject.SetActive(false);
            Shotgun_Selected = false;
        }else if(Slot2_Item == "Shotgun"){ 
            //Aktive Waffe 
            Shotgun_Top_Sprite.transform.gameObject.SetActive(true);
            Shotgun_Selected = true;
            //Alle anderen Deaktivieren
            Sniper_Top_Sprite.transform.gameObject.SetActive(false);
            Sniper_Selected = false;
            M4_Top_Sprite.transform.gameObject.SetActive(false);
            M4_Selected = false;
            Ak47_Top_Sprite.transform.gameObject.SetActive(false);
            Ak47_Selected = false;
            Glock_18_Top_Sprite.transform.gameObject.SetActive(false);
            Glock_18_Selected = false;
            Mp7_Top_Sprite.transform.gameObject.SetActive(false);
            Mp7_Selected = false;
        }else if(Slot2_Item == "Sniper"){
            //Aktive Waffe
            Sniper_Top_Sprite.transform.gameObject.SetActive(true);
            Sniper_Selected = true;
            //Alle anderen Deaktivieren
            M4_Top_Sprite.transform.gameObject.SetActive(false);
            M4_Selected = false;
            Ak47_Top_Sprite.transform.gameObject.SetActive(false);
            Ak47_Selected = false;
            Glock_18_Top_Sprite.transform.gameObject.SetActive(false);
            Glock_18_Selected = false;
            Mp7_Top_Sprite.transform.gameObject.SetActive(false);
            Mp7_Selected = false;
            Shotgun_Top_Sprite.transform.gameObject.SetActive(false);
            Shotgun_Selected = false;
        }else if(Slot2_Item == ""){
            //Alle anderen Deaktivieren
            Sniper_Top_Sprite.transform.gameObject.SetActive(false);
            Sniper_Selected = false;
            M4_Top_Sprite.transform.gameObject.SetActive(false);
            M4_Selected = false;
            Ak47_Top_Sprite.transform.gameObject.SetActive(false);
            Ak47_Selected = false;
            Glock_18_Top_Sprite.transform.gameObject.SetActive(false);
            Glock_18_Selected = false;
            Mp7_Top_Sprite.transform.gameObject.SetActive(false);
            Mp7_Selected = false;
            Shotgun_Top_Sprite.transform.gameObject.SetActive(false);
            Shotgun_Selected = false;
        }
        }

        if(Slot3_Selected == true){
        if(Slot3_Item == "Glock_18"){
            //Aktive Waffe
            Glock_18_Top_Sprite.transform.gameObject.SetActive(true);
            Glock_18_Selected = true;
            //Alle anderen Deaktivieren
            M4_Top_Sprite.transform.gameObject.SetActive(false);
            M4_Selected = false;
            Ak47_Top_Sprite.transform.gameObject.SetActive(false);
            Ak47_Selected = false;
            Sniper_Top_Sprite.transform.gameObject.SetActive(false);
            Sniper_Selected = false;
            Mp7_Top_Sprite.transform.gameObject.SetActive(false);
            Mp7_Selected = false;
            Shotgun_Top_Sprite.transform.gameObject.SetActive(false);
            Shotgun_Selected = false;
        }else if(Slot3_Item == "M4"){
            //Aktive Waffe
            M4_Top_Sprite.transform.gameObject.SetActive(true);
            M4_Selected = true;
            //Alle anderen Deaktivieren
            Glock_18_Top_Sprite.transform.gameObject.SetActive(false);
            Glock_18_Selected = false;
            Ak47_Top_Sprite.transform.gameObject.SetActive(false);
            Ak47_Selected = false;
            Sniper_Top_Sprite.transform.gameObject.SetActive(false);
            Sniper_Selected = false;
            Mp7_Top_Sprite.transform.gameObject.SetActive(false);
            Mp7_Selected = false;
            Shotgun_Top_Sprite.transform.gameObject.SetActive(false);
            Shotgun_Selected = false;
        }else if(Slot3_Item == "Ak47"){
            //Aktive Waffe
            Ak47_Top_Sprite.transform.gameObject.SetActive(true);
            Ak47_Selected = true;
            //Alle anderen Deaktivieren
            M4_Top_Sprite.transform.gameObject.SetActive(false);
            M4_Selected = false;
            Glock_18_Top_Sprite.transform.gameObject.SetActive(false);
            Glock_18_Selected = false;
            Sniper_Top_Sprite.transform.gameObject.SetActive(false);
            Sniper_Selected = false;
            Mp7_Top_Sprite.transform.gameObject.SetActive(false);
            Mp7_Selected = false;
            Shotgun_Top_Sprite.transform.gameObject.SetActive(false);
            Shotgun_Selected = false;
        }else if(Slot3_Item == "Mp7"){
            //Aktive Waffe 
            Mp7_Top_Sprite.transform.gameObject.SetActive(true);
            Mp7_Selected = true;
            //Alle anderen Deaktivieren
            Sniper_Top_Sprite.transform.gameObject.SetActive(false);
            Sniper_Selected = false;
            M4_Top_Sprite.transform.gameObject.SetActive(false);
            M4_Selected = false;
            Ak47_Top_Sprite.transform.gameObject.SetActive(false);
            Ak47_Selected = false;
            Glock_18_Top_Sprite.transform.gameObject.SetActive(false);
            Glock_18_Selected = false;
            Shotgun_Top_Sprite.transform.gameObject.SetActive(false);
            Shotgun_Selected = false;
        }else if(Slot3_Item == "Shotgun"){ 
            //Aktive Waffe 
            Shotgun_Top_Sprite.transform.gameObject.SetActive(true);
            Shotgun_Selected = true;
            //Alle anderen Deaktivieren
            Sniper_Top_Sprite.transform.gameObject.SetActive(false);
            Sniper_Selected = false;
            M4_Top_Sprite.transform.gameObject.SetActive(false);
            M4_Selected = false;
            Ak47_Top_Sprite.transform.gameObject.SetActive(false);
            Ak47_Selected = false;
            Glock_18_Top_Sprite.transform.gameObject.SetActive(false);
            Glock_18_Selected = false;
            Mp7_Top_Sprite.transform.gameObject.SetActive(false);
            Mp7_Selected = false;
        }else if(Slot3_Item == "Sniper"){
            //Aktive Waffe
            Sniper_Top_Sprite.transform.gameObject.SetActive(true);
            Sniper_Selected = true;
            //Alle anderen Deaktivieren
            M4_Top_Sprite.transform.gameObject.SetActive(false);
            M4_Selected = false;
            Ak47_Top_Sprite.transform.gameObject.SetActive(false);
            Ak47_Selected = false;
            Glock_18_Top_Sprite.transform.gameObject.SetActive(false);
            Glock_18_Selected = false;
            Mp7_Top_Sprite.transform.gameObject.SetActive(false);
            Mp7_Selected = false;
            Shotgun_Top_Sprite.transform.gameObject.SetActive(false);
            Shotgun_Selected = false;
        }else if(Slot3_Item == ""){
            //Alle anderen Deaktivieren
            Sniper_Top_Sprite.transform.gameObject.SetActive(false);
            Sniper_Selected = false;
            M4_Top_Sprite.transform.gameObject.SetActive(false);
            M4_Selected = false;
            Ak47_Top_Sprite.transform.gameObject.SetActive(false);
            Ak47_Selected = false;
            Glock_18_Top_Sprite.transform.gameObject.SetActive(false);
            Glock_18_Selected = false;
            Mp7_Top_Sprite.transform.gameObject.SetActive(false);
            Mp7_Selected = false;
            Shotgun_Top_Sprite.transform.gameObject.SetActive(false);
            Shotgun_Selected = false;
        }
        }
    }
}
