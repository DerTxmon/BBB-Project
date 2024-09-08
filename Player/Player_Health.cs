using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class Player_Health : MonoBehaviour
{
    public float health;
    public float Maxhealth;
    public TextMeshProUGUI Lebenstext;
    private GameObject World;
    public GameObject Healthbar;
    public UI_Handler UI;
    
    void Awake(){
        UI = gameObject.GetComponent<UI_Handler>();
    }
    void Start()
    {
        health = 200f;
        Lebenstext = GameObject.Find("Lebenstext").GetComponent<TextMeshProUGUI>();
        Maxhealth = 200f;
        World = GameObject.Find("World");
        StartCoroutine(CheckforRegeneration(health));
    }
    public void Damage(int Dmg){
        health -= Dmg;
        //Check for death
        if(health <= 0){
            StartCoroutine(Death());
        }
        //Korrigiere - Zahlen
        if(health < 0 ) health = 0;
        //UI Update
        Lebenstext.text = health.ToString();
        Healthbar.GetComponent<Image>().fillAmount = health / Maxhealth;
        if(health <= 20){
            Healthbar.GetComponent<Image>().color = Color.red;
        }else{
            Healthbar.GetComponent<Image>().color = Color.green;
        }
        this.gameObject.GetComponent<Player_Health>().enabled = false; //Damit kein negatiever Schaden mehr gemacht werden kann
    }
    public IEnumerator CheckforRegeneration(float currenthealth){
        float healthstate1 = currenthealth;
        yield return new WaitForSeconds(5);
        if(healthstate1 == health && health < Maxhealth){ //Wenn der Spieler 5 Sekunden lang keinen Schaden genommen hat und nicht auf 100% ist
            StartCoroutine(Regeneration(currenthealth));
        }
        StartCoroutine(CheckforRegeneration(health)); //recall
    }
    public IEnumerator Regeneration(float lasthealth){
        while(health < Maxhealth){
            yield return new WaitForSeconds(1.5f);
            //abbort when damage is taken
            if(lasthealth != health){
                yield break;
            }
            health += 2;
            Lebenstext.text = health.ToString();
            Healthbar.GetComponent<Image>().fillAmount = health / Maxhealth;
            if(health <= 20){
                Healthbar.GetComponent<Image>().color = Color.red;
            }else{
                Healthbar.GetComponent<Image>().color = Color.green;
            }
        }
        if(health > Maxhealth){
            health = Maxhealth;
            Lebenstext.text = health.ToString();
            Healthbar.GetComponent<Image>().fillAmount = health / Maxhealth;
            if(health <= 20){
                Healthbar.GetComponent<Image>().color = Color.red;
            }else{
                Healthbar.GetComponent<Image>().color = Color.green;
            }
        }
        this.gameObject.GetComponent<Player_Health>().enabled = true;
    }

    public IEnumerator Death(){
        //Langsam Zeit runter schrauben
        while(Time.timeScale > .11f){
            yield return new WaitForSecondsRealtime(.04f);
            float applytotime = Time.timeScale -.1f;
            Time.timeScale = (float)System.Math.Round(applytotime * 100) / 100; //Rundet auf 2 Nachkommastellen
        }
        Map_Manager.DeactivateAllBots();
        StartCoroutine(UI.EndScreen(false));
    }
}
