using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mate_PlayerContact : MonoBehaviour
{
    public GameObject Mate;
    public Rigidbody2D rb;
    Quaternion norotation;
    public float Raydistance;
    [SerializeField] private GameObject Wall;
    [SerializeField] private LayerMask layerMask;
    private RaycastHit2D[] contactPoints = new RaycastHit2D[100];
    private Vector2[] endpos = new Vector2[100];
    private string[] Itemtags = new string[] {"Glock_18", "M4", "AK_47", "Sniper", "Mp7", "Shotgun"};
    private string[] Ammotags = new string[] {"Big_Ammo", "Mid_Ammo", "Small_Ammo", "Shotgun_Ammo", "Heal", "Kiste_Oben", "Kiste_Unten", "Kiste_Rechts", "Kiste_Links"}; //Ammo und Heal
    public string[] Kistentags = new string[] {"Kiste_Oben", "Kiste_Unten", "Kiste_Rechts", "Kiste_Links"}; //Kisten
    [SerializeField] public List<int> Entryids = new List<int>();
    public bool Entry2 = false;
    public Mate_Behavior Mate_behavior;
    public Mate_Optimazation Mate_optimazation;
    public Mate_Inventory Inv;
    private int teildurch = 5; //Update EnemyContact() every x frames
    private int x;
    public bool WallDetected, WallDetectedRight, WallDetectedLeft = false;

    private void Awake() {
        Mate_behavior = Mate.GetComponent<Mate_Behavior>();
        Mate_optimazation = this.gameObject.GetComponentInParent<Mate_Optimazation>();
    }
    void Start(){
        norotation = Quaternion.Euler(0, 0, 0);
    }
    
    void EnemyContact(float Raydistance) {
        float x = 0;
        //Setze alle sum variablen wieder auf null
        Mate_behavior.rightsum = 0;
        Mate_behavior.leftsum = 0;
        Mate_behavior.topsum = 0;
        Mate_behavior.bottomsum = 0;

        //Create 4 Rays. Up, Down, Left, Right.
        //1 Oben
        Vector2 endpos0 = Mate.transform.position + Vector3.up * Raydistance;
        contactPoints[0] = Physics2D.Linecast(Mate.transform.position, endpos0, layerMask);
        //Make the rays visible in the Scene
        if(contactPoints[0].collider == null) {
            Debug.DrawLine(Mate.transform.position, endpos0, Color.green);
        }else{
            Debug.DrawLine(Mate.transform.position, contactPoints[0].point, Color.red);
        }
        if(contactPoints[0].collider == null){
            Mate_behavior.topsum += 10;
        }else Mate_behavior.topsum += contactPoints[0].distance;
        
        //2 Unten
        Vector2 endpos1 = Mate.transform.position + Vector3.down * Raydistance;
        contactPoints[1] = Physics2D.Linecast(transform.position, endpos1, layerMask);
        if(contactPoints[1].collider == null) {
            Debug.DrawLine(Mate.transform.position, endpos1, Color.green);
        }else{
            Debug.DrawLine(Mate.transform.position, contactPoints[1].point, Color.red);
        }
        if(contactPoints[1].collider == null){
            Mate_behavior.bottomsum += 10;
        }else Mate_behavior.bottomsum += contactPoints[1].distance;
        
        //3 Rechts
        Vector2 endpos2 = Mate.transform.position + Vector3.right * Raydistance * 2f;
        contactPoints[2] = Physics2D.Linecast(transform.position, endpos2, layerMask);
        if(contactPoints[2].collider == null) {
            Debug.DrawLine(Mate.transform.position, endpos2, Color.green);
        }else{
            Debug.DrawLine(Mate.transform.position, contactPoints[2].point, Color.red);
        }
        if(contactPoints[2].collider == null){
            Mate_behavior.rightsum += 20;
        }else Mate_behavior.rightsum += contactPoints[2].distance;
        
        //4 Links
        Vector2 endpos3 = Mate.transform.position + Vector3.left * Raydistance * 2f;
        contactPoints[3] = Physics2D.Linecast(transform.position, endpos3, layerMask);
        if(contactPoints[3].collider == null) {
            Debug.DrawLine(Mate.transform.position, endpos3, Color.green);
        }else{
            Debug.DrawLine(Mate.transform.position, contactPoints[3].point, Color.red);
        }
        if(contactPoints[3].collider == null){
            Mate_behavior.leftsum += 20;
        }else Mate_behavior.leftsum += contactPoints[3].distance;
        
        //Oben Rechte ecke
        for(int i = 4; i < 20 ;i++){
            x += .125f;
            endpos[i] = Mate.transform.position + Vector3.up * Raydistance + Vector3.right * Raydistance * x;
            contactPoints[i] = Physics2D.Linecast(transform.position, endpos[i], layerMask);
            if(contactPoints[i].collider == null) {
                Debug.DrawLine(Mate.transform.position, endpos[i], Color.green);
            }else{
                Debug.DrawLine(Mate.transform.position, contactPoints[i].point, Color.red);
            }
            if(contactPoints[i].collider == null){
            Mate_behavior.topsum += 10;
            }else Mate_behavior.topsum += contactPoints[i].distance;
        }
        //Rechte Seite
        for(int i = 4; i < 20 ;i++){
            x -= .125f;
            endpos[i] = Mate.transform.position + Vector3.up * Raydistance + Vector3.right * Raydistance * 1.5f + Vector3.right * Raydistance * 0.5f + Vector3.down * Raydistance * x;
            contactPoints[i] = Physics2D.Linecast(transform.position, endpos[i], layerMask);
            if(contactPoints[i].collider == null) {
                Debug.DrawLine(Mate.transform.position, endpos[i], Color.green);
            }else{
                Debug.DrawLine(Mate.transform.position, contactPoints[i].point, Color.red);
            }
            //Addiere alles zur rightsum variable
            if(contactPoints[i].collider == null){
            Mate_behavior.rightsum += 20;
            }else Mate_behavior.rightsum += contactPoints[i].distance;
        }
        x = 0;
        //Oben Linke ecke
        for(int i = 21; i < 37 ;i++){
            x += .125f;
            endpos[i] = Mate.transform.position + Vector3.up * Raydistance + Vector3.left * Raydistance * x;
            contactPoints[i] = Physics2D.Linecast(transform.position, endpos[i], layerMask);
            if(contactPoints[i].collider == null) {
                Debug.DrawLine(Mate.transform.position, endpos[i], Color.green);
            }else{
                Debug.DrawLine(Mate.transform.position, contactPoints[i].point, Color.red);
            }
            if(contactPoints[i].collider == null){
            Mate_behavior.topsum += 10;
            }else Mate_behavior.topsum += contactPoints[i].distance;
        }
        //Linke Seite
        for(int i = 38; i < 54 ;i++){
            x -= .125f;
            endpos[i] = Mate.transform.position + Vector3.up * Raydistance + Vector3.left * Raydistance * 1.5f + Vector3.left * Raydistance * 0.5f + Vector3.down * Raydistance * x;
            contactPoints[i] = Physics2D.Linecast(transform.position, endpos[i], layerMask);
            if(contactPoints[i].collider == null) {
                Debug.DrawLine(Mate.transform.position, endpos[i], Color.green);
            }else{
                Debug.DrawLine(Mate.transform.position, contactPoints[i].point, Color.red);
            }
            if(contactPoints[i].collider == null){
            Mate_behavior.leftsum += 20;
            }else Mate_behavior.leftsum += contactPoints[i].distance;
        }
        x = 0;
        //Unten Linke ecke
        for(int i = 55; i < 71 ;i++){
            x += .125f;
            endpos[i] = Mate.transform.position + Vector3.down * Raydistance + Vector3.left * Raydistance * x;
            contactPoints[i] = Physics2D.Linecast(transform.position, endpos[i], layerMask);
            if(contactPoints[i].collider == null) {
                Debug.DrawLine(Mate.transform.position, endpos[i], Color.green);
            }else{
                Debug.DrawLine(Mate.transform.position, contactPoints[i].point, Color.red);
            }
            if(contactPoints[i].collider == null){
            Mate_behavior.bottomsum += 10;
            }else Mate_behavior.bottomsum += contactPoints[i].distance;
        }
        x = 0;
        //Unten Rechte ecke
        for(int i = 72; i < 88 ;i++){
            x += .125f;
            endpos[i] = Mate.transform.position + Vector3.down * Raydistance + Vector3.right * Raydistance * x;
            contactPoints[i] = Physics2D.Linecast(transform.position, endpos[i], layerMask);
            if(contactPoints[i].collider == null) {
                Debug.DrawLine(Mate.transform.position, endpos[i], Color.green);
            }else{
                Debug.DrawLine(Mate.transform.position, contactPoints[i].point, Color.red);
            }
            if(contactPoints[i].collider == null){
            Mate_behavior.bottomsum += 10;
            }else Mate_behavior.bottomsum += contactPoints[i].distance;
        }
        //WALL DETECTION RAY
        Vector2 endpos4 = Mate.transform.position + Mate.transform.up * 20;
        contactPoints[88] = Physics2D.Linecast(Mate.transform.position, endpos4, layerMask);
        if(contactPoints[88].collider == null) {
            Debug.DrawLine(Mate.transform.position, endpos4, Color.blue);
        }else{
            Debug.DrawLine(Mate.transform.position, contactPoints[88].point, Color.cyan);
        }
        if(contactPoints[88].collider != null && contactPoints[88].collider.gameObject.tag != "Bot" && contactPoints[88].collider.gameObject.tag != "Player"){
            if(Mate_behavior.inhouse == false){
                if(Vector2.Distance(contactPoints[88].point, Mate.transform.position) < 1.8f) WallDetected = true; //Outsinde
            }
            else if(Vector2.Distance(contactPoints[88].point, Mate.transform.position) < 1f) WallDetected = true; //inside
        }else if(Vector2.Distance(contactPoints[88].point, Mate.transform.position) > 1.8f || contactPoints[88].collider.gameObject == null) WallDetected = false;

        //WALL DETECTION RAY SLIGHTLY TO THE RIGHT
        Vector2 endpos5 = Mate.transform.position + Mate.transform.up * 20 + Mate.transform.right * 15f;
        contactPoints[89] = Physics2D.Linecast(Mate.transform.position, endpos5, layerMask);
        if(contactPoints[89].collider == null) {
            Debug.DrawLine(Mate.transform.position, endpos5, Color.blue);
        }else{
            Debug.DrawLine(Mate.transform.position, contactPoints[89].point, Color.cyan);
        }
        if(contactPoints[89].collider != null && contactPoints[89].collider.gameObject.tag != "Bot" && contactPoints[89].collider.gameObject.tag != "Player"){
            if(Mate_behavior.inhouse == false){
                if(Vector2.Distance(contactPoints[89].point, Mate.transform.position) < 1.8f) WallDetectedRight = true; //Outsinde
            }
            else if(Vector2.Distance(contactPoints[89].point, Mate.transform.position) < 1f) WallDetectedRight = true; //inside
        }else if(Vector2.Distance(contactPoints[89].point, Mate.transform.position) > 1.8f || contactPoints[89].collider.gameObject == null) WallDetectedRight = false;

        //WALL DETECTION RAY SLIGHTLY TO THE LEFT
        Vector2 endpos6 = Mate.transform.position + Mate.transform.up * 20 + Mate.transform.right * -15f;
        contactPoints[90] = Physics2D.Linecast(Mate.transform.position, endpos6, layerMask);
        if(contactPoints[90].collider == null) {
            Debug.DrawLine(Mate.transform.position, endpos6, Color.blue);
        }else{
            Debug.DrawLine(Mate.transform.position, contactPoints[90].point, Color.cyan);
        }
        if(contactPoints[90].collider != null && contactPoints[90].collider.gameObject.tag != "Bot" && contactPoints[90].collider.gameObject.tag != "Player"){
            if(Mate_behavior.inhouse == false){
                if(Vector2.Distance(contactPoints[90].point, Mate.transform.position) < 1.8f) WallDetectedLeft = true; //Outsinde
            }
            else if(Vector2.Distance(contactPoints[90].point, Mate.transform.position) < 1f) WallDetectedLeft = true; //inside
        }else if(Vector2.Distance(contactPoints[90].point, Mate.transform.position) > 1.8f || contactPoints[90].collider.gameObject == null) WallDetectedLeft = false;


        //Setze alle Summen auf ihren Protzentsatz
        Mate_behavior.bottomsum = Mate_behavior.bottomsum / 330;
        Mate_behavior.topsum = Mate_behavior.topsum / 330;
        Mate_behavior.rightsum = Mate_behavior.rightsum / 340;
    	Mate_behavior.leftsum = Mate_behavior.leftsum / 340;

        //Setzt den "Contact" bool auf true oder false je nachdem ob ein Objekt getroffen wurde.
        //Scanne jeden Ray nach Kontakt zu Items.
        //Scanne jeden Ray nach Blockaden für den Mate und Setze die Blockade dann als Gameobject ein.
        foreach(RaycastHit2D contact in contactPoints){
            //Gegner Registrieren
            if(contact.collider != null){
                if(contact.collider.gameObject.tag == "Bot"){
                    //Setzt das Collidierte Gameobject zum Aktuellen Gegner.
                    if(Mate_behavior.EnemyContact == false) Mate_behavior.Enemy = contact.collider.gameObject;
                    Mate_behavior.EnemyContact = true;
                    if(contact.collider.gameObject.tag == "Bot") teildurch = 1;
                    else teildurch = 5;
                    break; //Um die Varialble beim nächsten durchlauf nicht wieder auf false zu setzen.
                }

                //Haus Eingang Registrieren
                if(contact.collider.gameObject.tag == "Entry"){
                    //Laufe zum Entry
                    //...
                    teildurch = 2;
                    break; //Um die Varialble beim nächsten durchlauf nicht wieder auf false zu setzen.
                }

                //Loot Registrieren
                //Waffen
                if(Mate_behavior.EnemyContact == false){
                foreach(string i in Itemtags){
                    if(contact.collider.gameObject.tag == i && Mate_behavior.Currloot == null && Inv.lootcount < 3){
                        if(Mate_behavior.Currloot == null && Mate.GetComponent<Mate_Inventory>().lootcount < 3){
                            Mate_behavior.Currloot = contact.collider.gameObject;
                            Mate_behavior.looting = true;
                        }
                        break;
                    }else{
                        Mate_behavior.Currloot = null;
                        Mate_behavior.looting = false;
                    }
                }
                //Ammo
                foreach(string i in Ammotags){
                    if(contact.collider.gameObject.tag == i && Mate_behavior.Currloot == null){
                        Mate_behavior.Currloot = contact.collider.gameObject;
                        Mate_behavior.looting = true;
                        break;
                    }
                }
                //Kisten
                foreach(string i in Kistentags){
                    if(contact.collider.gameObject.tag == i){
                        try{
                        if(contact.collider.gameObject.GetComponent<Kiste>().isopen == false){
                            Mate_behavior.Currloot = contact.collider.gameObject;
                            Mate_behavior.looting = true;
                            break;
                        }else{
                            Mate_behavior.Currloot = null;
                            Mate_behavior.looting = false;
                        }
                        }catch{}
                    }
                }
            }else{
                //Setzt auf null.
                Mate_behavior.Enemy = null;
                Mate_behavior.EnemyContact = false;
                Mate_behavior.looting = false;
                Mate_behavior.Currloot = null;
            }
            }
        }
    }

    void Update(){
        //Damit sich die Hitbox für den Enemy detect nicht dumm bewegt.
        transform.rotation = Quaternion.Euler(0f,0f,Mate.transform.rotation.z - transform.rotation.z);
        if(x % teildurch == 0){
            if(Mate_optimazation.is_object_in_range) EnemyContact(Raydistance);
            if(x > 100) x = 0;
        }
        x++;
    }
}